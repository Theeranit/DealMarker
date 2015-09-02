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

namespace KK.DealMaker.Business.Master
{
    public class AuditBusiness : BaseBusiness
    {
        public void TraceAuditLoginUser(SessionInfo sessioninfo)
        {
            using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
            {

                List<DA_LOGIN_AUDIT> foundaudits = unitOfWork.DA_LOGIN_AUDITRepository.GetByUserID(sessioninfo.CurrentUserId);
                if (foundaudits.Count > 0)
                {
                    foreach (DA_LOGIN_AUDIT foundaudit in foundaudits.Where(p => !p.LOGOUT_DATE.HasValue))
                    {
                        foundaudit.LOGOUT_DATE = DateTime.Now;
                        foundaudit.RESULT = String.Format("Logout the system on {0}, because someone use this user to login the system from another machine or computer place.", DateTime.Now.ToString());
                    }
                }
                DA_LOGIN_AUDIT audit = new DA_LOGIN_AUDIT();
                audit.ID = sessioninfo.ID;
                audit.LOG.INSERTBYUSERID = sessioninfo.CurrentUserId;
                audit.LOG.INSERTDATE = DateTime.Now;
                audit.LOGON_DATE = DateTime.Now;
                audit.RESULT = "Logon System";
                audit.TERMINAL = sessioninfo.IPAddress;
                audit.USER_ID = sessioninfo.CurrentUserId;
                unitOfWork.DA_LOGIN_AUDITRepository.Add(audit);
                unitOfWork.Commit();
            }

        }

        public void TraceAuditLogoutUser(SessionInfo sessioninfo)
        {
            using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
            {
                //DA_LOGIN_AUDIT foundaudit = unitOfWork.DA_LOGIN_AUDITRepository.GetBySessionInfo(sessioninfo);
                DA_LOGIN_AUDIT foundaudit = unitOfWork.DA_LOGIN_AUDITRepository.GetByID(sessioninfo.ID);
                if (foundaudit != null)
                {
                    foundaudit.LOGOUT_DATE = DateTime.Now;
                    foundaudit.RESULT = String.Format("Logout the system on {0}", DateTime.Now.ToString());
                }
                unitOfWork.Commit();
            }
        }

        public DA_LOGIN_AUDIT GetUserLogged(SessionInfo sessioninfo)
        {
            using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
            {
                //return unitOfWork.DA_LOGIN_AUDITRepository.GetBySessionInfo(sessioninfo);
                return unitOfWork.DA_LOGIN_AUDITRepository.GetByID(sessioninfo.ID);
            }
        }
    }
}
