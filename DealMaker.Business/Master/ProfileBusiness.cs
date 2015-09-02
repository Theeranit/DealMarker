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
    public class ProfileBusiness : BaseBusiness
    {


        public List<MA_USER_PROFILE> GetProfileOptions()
        {
            List<MA_USER_PROFILE> profileList;
            using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
            {
                profileList = unitOfWork.MA_USER_PROFILERepository.GetAll();
            }
            return profileList;

        }

        public List<MA_USER_PROFILE> GetProfileByFilter(SessionInfo sessioninfo, string name, string sorting)
        {
            try
            {
                //IEnumerable<MA_USER> query;
                IEnumerable<MA_USER_PROFILE> sortedRecords;
                using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
                {
                    var query = unitOfWork.MA_USER_PROFILERepository.GetAll().AsQueryable();

                    //Filters
                    if (!string.IsNullOrEmpty(name))
                    {
                        query = query.Where(p => p.LABEL.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0);
                    }
                    //Sorting
                    string[] sortsp = sorting.Split(' ');
                    IQueryable<MA_USER_PROFILE> orderedRecords = query.OrderBy(sortsp[0], sortsp[1]);
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

        public MA_USER_PROFILE CreateUserProfile(SessionInfo sessioninfo, MA_USER_PROFILE userprofile)
        {
            using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
            {
                var checkDuplicate = unitOfWork.MA_USER_PROFILERepository.GetAll().FirstOrDefault(p => p.LABEL.ToLower().Equals(userprofile.LABEL.ToLower()));
                if (checkDuplicate != null)
                    throw this.CreateException(new Exception(), "Profile is duplicated");

                unitOfWork.MA_USER_PROFILERepository.Add(userprofile);
                unitOfWork.Commit();
            }

            return userprofile;
        }

        public MA_USER_PROFILE UpdateUserProfile(SessionInfo sessioninfo, MA_USER_PROFILE userprofile)
        {

            using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
            {
                var checkDuplicate = unitOfWork.MA_USER_PROFILERepository.GetAll().FirstOrDefault(p => p.LABEL.ToLower().Equals(userprofile.LABEL.ToLower()) && p.ID != userprofile.ID);
                if (checkDuplicate != null)
                    throw this.CreateException(new Exception(), "Profile is duplicated");

                var founduserprofile = unitOfWork.MA_USER_PROFILERepository.All().FirstOrDefault(p => p.ID == userprofile.ID);
                if (founduserprofile == null)
                    throw this.CreateException(new Exception(), "Data not found!");
                else
                {

                    founduserprofile.ID = userprofile.ID;
                    founduserprofile.LABEL = userprofile.LABEL;
                    founduserprofile.ISACTIVE = userprofile.ISACTIVE;
                    
                    unitOfWork.Commit();

                }
            }

            return userprofile;
        }


        

    }
}
