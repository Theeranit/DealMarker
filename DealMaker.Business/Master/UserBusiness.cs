using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.EnterpriseServices;
using KK.DealMaker.Core.Common;
using KK.DealMaker.Core.Constraint;
using KK.DealMaker.Core.Data;
using KK.DealMaker.Core.OpicsData;
using KK.DealMaker.DataAccess.Repositories;
using KK.DealMaker.Core.SystemFramework;
using System.Configuration;
using KK.DealMaker.Core.Helper;
using System.Linq.Expressions;
using KK.DealMaker.Business.External;
using KK.DealMaker.Business.Log;

namespace KK.DealMaker.Business.Master
{
    
    public class UserBusiness : BaseBusiness 
    {
        [AutoComplete()]
        public SessionInfo LogOn(string username, string userIP)
        {
            MA_USER user;
            SessionInfo sessioninfo = new SessionInfo();
            AuditBusiness _auditBusiness = new AuditBusiness();
            LoggingHelper.Debug("Begin Logon by " + username);
            using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
            {
                sessioninfo.ConnectionString = (string)ConfigurationManager.AppSettings[AppSettingName.CONNECTION_STRING];
                sessioninfo.Local = "Localhost";
                sessioninfo.StartSession = DateTime.Now;
                LoggingHelper.Debug("Begin get user data");
                user = unitOfWork.MA_USERRepository.GetByUserCode(username);
                if (user == null)
                {
                    LoggingHelper.Debug(username + " error: " + Messages.USER_NOT_UNAVAILABLE);
                    throw this.CreateException(new Exception(), Messages.USER_NOT_UNAVAILABLE);
                }
                sessioninfo.ID = Guid.NewGuid();
                sessioninfo.CurrentUserId = user.ID;
                sessioninfo.UserFullName = user.NAME;
                sessioninfo.UserLogon = username;
                sessioninfo.IsActive = user.ISACTIVE.Value;
                sessioninfo.IsLocked = user.ISLOCKED.Value;
                sessioninfo.ProfileId = user.USER_PROFILE_ID;
                sessioninfo.ProfileName = user.MA_USER_PROFILE.LABEL;
                sessioninfo.IPAddress = userIP;
                LoggingHelper.Debug("End get user data");

                //Get from MA_PROCESS_DATE
                //LoggingHelper.Debug("Begin get processing date from OPICS");
                //OpicsBusiness _opicsBusiness = new OpicsBusiness();
                //PROCDateModel prcDate = _opicsBusiness.GetDateProcessing(DateTime.Today);
                //sessioninfo.Process.CurrentDate = prcDate.CURRDATE;
                //sessioninfo.Process.PreviousDate = prcDate.PREVDATE;
                //sessioninfo.Process.NextDate = prcDate.NEXTDATE;
                //LoggingHelper.Debug("End get processing date from OPICS");

                LoggingHelper.Debug("Begin get processing date from MA_PROCESS_DATE");
                MA_PROCESS_DATE PROC = unitOfWork.MA_PROCESS_DATERepository.All().FirstOrDefault();
                sessioninfo.Process.CurrentDate = PROC!=null ? PROC.PROC_DATE : DateTime.Today;
                sessioninfo.Process.PreviousDate = PROC!=null ? PROC.PREV_PROC_DATE : DateTime.Today.AddDays(-1);
                sessioninfo.Process.NextDate = PROC!=null ? PROC.NEXT_PROC_DATE : DateTime.Today.AddDays(1);                
                LoggingHelper.Debug("End get processing date from MA_PROCESS_DATE");

                //AUDIT USER TRANSACTION
                LoggingHelper.Debug("Begin Trace Audit User " + user.NAME);
                _auditBusiness.TraceAuditLoginUser(sessioninfo);
                LoggingHelper.Debug("End Trace Audit User " + user.NAME);

            }

            return sessioninfo;
        }

        public List<MA_USER> GetAll()
        {
            List<MA_USER> userList;
            using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
            {
                userList = unitOfWork.MA_USERRepository.GetAll().OrderBy(p => p.NAME).ToList();
            }
            return userList;
        }

        public MA_USER GetByID(SessionInfo sessioninfo, Guid ID)
        {
            MA_USER user;
            using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
            {
                user = unitOfWork.MA_USERRepository.GetById(ID);
            }
            return user;
        }

        public MA_USER GetByUserCode(SessionInfo sessioninfo, string usercode)
        {
            MA_USER user;
            using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
            {
                user = unitOfWork.MA_USERRepository.GetByUserCode(usercode);
            }
            return user;
        }

        public List<MA_USER> GetByFilter(SessionInfo sessioninfo, string name, string sorting)
        {
            try
            {
                //IEnumerable<MA_USER> query;
                IEnumerable<MA_USER> sortedRecords;
                using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
                {
                    var query = unitOfWork.MA_USERRepository.GetAll().AsQueryable();
                    //Filters
                    if (!string.IsNullOrEmpty(name))
                    {
                        query = query.Where(p => p.NAME.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0);
                    }
                    //Sorting
                    string[] sortsp = sorting.Split(' ');
                    IQueryable<MA_USER> orderedRecords = query.OrderBy(sortsp[0], sortsp[1]);
                    sortedRecords = orderedRecords.ToList();
                }
                //Return result to jTable
                return sortedRecords.ToList();
            
            }
            catch (DataServicesException ex)
            {
                throw this.CreateException(ex, null);
            }
        }

        public MA_USER CreateUser(SessionInfo sessioninfo, MA_USER user)
        {
            using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
            {
                var checkDuplicate = unitOfWork.MA_USERRepository.GetByUserCode(user.USERCODE);
                if (checkDuplicate != null)
                    throw this.CreateException(new Exception(), Messages.DUPLICATE_DATA);
                LogBusiness logBusiness = new LogBusiness();
                unitOfWork.DA_LOGGINGRepository.Add(logBusiness.CreateLogging(sessioninfo, user.ID, LogEvent.USER_AUDIT.ToString(), LookupFactorTables.MA_USER, "User", new { }));
                unitOfWork.MA_USERRepository.Add(user);
                unitOfWork.Commit();
            }

            return user;
        }

        public MA_USER UpdateUser(SessionInfo sessioninfo, MA_USER user)
        {
            
            using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
            {
                var checkDuplicate = unitOfWork.MA_USERRepository.GetAll().FirstOrDefault(p => p.USERCODE.ToLower() == user.USERCODE.ToLower() && p.ID != user.ID);
                if (checkDuplicate != null)
                    throw this.CreateException(new Exception(), Messages.DUPLICATE_DATA);

                var foundUser = unitOfWork.MA_USERRepository.GetAll().FirstOrDefault(p => p.ID == user.ID);
                if (foundUser == null)
                    throw this.CreateException(new Exception(), Messages.DATA_NOT_FOUND);
                else
                {
                    LogBusiness logBusiness = new LogBusiness();
                    var oldRecord = new { 

                        DEPARTMENT = foundUser.DEPARTMENT, ISACTIVE = foundUser.ISACTIVE
                        , ISLOCKED = foundUser.ISLOCKED, NAME = foundUser.NAME
                        , USERCODE = foundUser.USERCODE, USER_OPICS = foundUser.USER_OPICS
                        , USER_PROFILE = foundUser.MA_USER_PROFILE.LABEL
                    };
                    var newRecord = new
                    {
                        DEPARTMENT = user.DEPARTMENT, ISACTIVE = user.ISACTIVE
                        , ISLOCKED = user.ISLOCKED, NAME = user.NAME
                        , USERCODE = user.USERCODE, USER_OPICS = user.USER_OPICS
                        , USER_PROFILE = unitOfWork.MA_USER_PROFILERepository.All().FirstOrDefault(p=> p.ID== user.USER_PROFILE_ID).LABEL                       
                    };
                   
                    var log = logBusiness.UpdateLogging(sessioninfo, foundUser.ID, LogEvent.USER_AUDIT.ToString(), LookupFactorTables.MA_USER, oldRecord, newRecord);
                    if (log != null) unitOfWork.DA_LOGGINGRepository.Add(log);
                    foundUser.ID = user.ID;
                    foundUser.DEPARTMENT = user.DEPARTMENT;
                    foundUser.ISACTIVE = user.ISACTIVE;
                    foundUser.ISLOCKED = user.ISLOCKED;
                    foundUser.LOG.MODIFYBYUSERID = user.LOG.MODIFYBYUSERID;
                    foundUser.LOG.MODIFYDATE = user.LOG.MODIFYDATE;
                    foundUser.NAME = user.NAME;
                    foundUser.USERCODE = user.USERCODE;
                    foundUser.USER_OPICS = user.USER_OPICS;
                    foundUser.USER_PROFILE_ID = user.USER_PROFILE_ID;
                    
                    unitOfWork.Commit();

                }
            }

            return user;
        }

        public void DeleteUser(SessionInfo sessioninfo, Guid ID)
        {
            using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
            {
                var foundUser = unitOfWork.MA_USERRepository.All().FirstOrDefault(p => p.ID == ID);
                unitOfWork.MA_USERRepository.Delete(foundUser);
                unitOfWork.Commit();
            }
        }

    }
}
