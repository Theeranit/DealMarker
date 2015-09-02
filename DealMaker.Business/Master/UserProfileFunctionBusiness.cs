using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.EnterpriseServices;
using KK.DealMaker.Core.Common;
using KK.DealMaker.Core.Constraint;
using KK.DealMaker.Core.Data;
using KK.DealMaker.DataAccess.Repositories;
using KK.DealMaker.Core.SystemFramework;
using System.Configuration;
using KK.DealMaker.Core.Helper;
using System.Linq.Expressions;


namespace KK.DealMaker.Business.Master
{
    public class ProfileFunctionBusiness : BaseBusiness
    {
        public List<MA_PROFILE_FUNCTIONAL> GetProfileFunctionByFilter(SessionInfo sessioninfo,string strprofile,string strfunction,string sorting)
        {
            try
            {
                IEnumerable<MA_PROFILE_FUNCTIONAL> sortedRecords;
                using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
                {
                    var query = unitOfWork.MA_PROFILE_FUNCTIONALRepository.GetAll().AsQueryable();
                    Guid guTemp;

                    //Filters
                    if (Guid.TryParse(strfunction, out guTemp)) //Function
                    {
                        query = query.Where(p => p.FUNCTIONAL_ID == Guid.Parse(strfunction));
                    }
                    if (Guid.TryParse(strprofile, out guTemp)) //profile
                    {
                        query = query.Where(p => p.USER_PROFILE_ID == Guid.Parse(strprofile));
                    }

                    //Sorting
                    string[] sortsp = sorting.Split(' ');
                    IQueryable<MA_PROFILE_FUNCTIONAL> orderedRecords = query.OrderBy(sortsp[0], sortsp[1]);
                    sortedRecords = orderedRecords.ToList();
                }

                return sortedRecords.ToList();

            }
            catch (DataServicesException ex)
            {
                throw this.CreateException(ex, null);
            }
        }

        public MA_PROFILE_FUNCTIONAL CreateProfileFunction(SessionInfo sessioninfo, MA_PROFILE_FUNCTIONAL profilefunction)
        {
            using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
            {
                if (ValidateProfileFunction(profilefunction))
                {
                    unitOfWork.MA_PROFILE_FUNCTIONALRepository.Add(profilefunction);
                    unitOfWork.Commit();
                }
                else
                    throw this.CreateException(new Exception(), Messages.DUPLICATE_DATA);
            }

            return profilefunction;
        }

        public MA_PROFILE_FUNCTIONAL UpdateProfileFunction(SessionInfo sessioninfo, MA_PROFILE_FUNCTIONAL profilefunction)
        {

            using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
            {
                var checkDuplicate = unitOfWork.MA_PROFILE_FUNCTIONALRepository.GetAll().FirstOrDefault(p => p.USER_PROFILE_ID == profilefunction.USER_PROFILE_ID && p.FUNCTIONAL_ID == profilefunction.FUNCTIONAL_ID && p.ID != profilefunction.ID);
                if (checkDuplicate != null)
                    throw this.CreateException(new Exception(), Messages.DUPLICATE_DATA);

                var foundprofilefunction = unitOfWork.MA_PROFILE_FUNCTIONALRepository.All().FirstOrDefault(p => p.ID == profilefunction.ID);
                if (foundprofilefunction == null)
                    throw this.CreateException(new Exception(), Messages.DATA_NOT_FOUND);
                else
                {

                    foundprofilefunction.ID = profilefunction.ID;
                    foundprofilefunction.ISAPPROVABLE = profilefunction.ISAPPROVABLE;
                    foundprofilefunction.ISREADABLE = profilefunction.ISREADABLE;
                    foundprofilefunction.ISWRITABLE = profilefunction.ISWRITABLE;
                    foundprofilefunction.USER_PROFILE_ID = profilefunction.USER_PROFILE_ID;
                    foundprofilefunction.FUNCTIONAL_ID = profilefunction.FUNCTIONAL_ID;
                    unitOfWork.Commit();

                }
            }

            return profilefunction;
        }

        public void DeleteProfileFunction(SessionInfo sessioninfo, Guid guID)
        {

            using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
            {
                var foundprofilefunction = unitOfWork.MA_PROFILE_FUNCTIONALRepository.All().FirstOrDefault(p => p.ID == guID);
                if (foundprofilefunction == null)
                    throw this.CreateException(new Exception(), Messages.DATA_NOT_FOUND);
                else
                {
                    unitOfWork.MA_PROFILE_FUNCTIONALRepository.Delete(foundprofilefunction);
                    unitOfWork.Commit();
                }
            }
        }

        public List<PermisionModel> GetPermissionByProfileID(Guid guProfileID)
        {
            List<PermisionModel> permissions = null;
            using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
            {
                var query = unitOfWork.MA_PROFILE_FUNCTIONALRepository.GetByUserProfileId(guProfileID)
                                    .Select(p => new PermisionModel
                                    {
                                        FunctionalCode = p.MA_FUNCTIONAL.USERCODE,
                                        FunctionalLabel = p.MA_FUNCTIONAL.LABEL,
                                        IsReadable = p.ISREADABLE,
                                        IsWritable = p.ISWRITABLE,
                                        IsApprovable = p.ISAPPROVABLE
                                    });
                permissions = query.ToList();
            }
            return permissions;
        }

        private bool ValidateProfileFunction(MA_PROFILE_FUNCTIONAL data)
        {
            List<MA_PROFILE_FUNCTIONAL> oldData = null;

            using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
            {
                oldData = unitOfWork.MA_PROFILE_FUNCTIONALRepository.GetAll().Where(t => t.FUNCTIONAL_ID == data.FUNCTIONAL_ID && t.USER_PROFILE_ID == data.USER_PROFILE_ID).ToList();
                if (oldData.Count > 0)
                    return false;
                else
                    return true;
            }
        }
    }
}
