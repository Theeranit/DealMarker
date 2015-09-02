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
    public class LimitProductBusiness : BaseBusiness
    {
         public List<MA_LIMIT_PRODUCT> GetLimitProductByFilter(SessionInfo sessioninfo,string strproduct,string strlimit, string sorting)
        {
            try
            {
                IEnumerable<MA_LIMIT_PRODUCT> sortedRecords;
                using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
                {
                    var query = unitOfWork.MA_LIMIT_PRODUCTRepository.GetAll().AsQueryable();
                    Guid guTemp;

                    //Filters
                    if (Guid.TryParse(strproduct, out guTemp)) //Function
                    {
                        query = query.Where(p => p.PRODUCT_ID == Guid.Parse(strproduct));
                    }
                    if (Guid.TryParse(strlimit, out guTemp)) //profile
                    {
                        query = query.Where(p => p.LIMIT_ID == Guid.Parse(strlimit));
                    }

                    //Sorting
                    string[] sortsp = sorting.Split(' ');
                    IQueryable<MA_LIMIT_PRODUCT> orderedRecords = query.OrderBy(sortsp[0], sortsp[1]);
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

         public MA_LIMIT_PRODUCT CreateLimitProduct(SessionInfo sessioninfo, MA_LIMIT_PRODUCT limitproduct)
        {
            using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
            {
                if (ValidateProfileFunction(limitproduct))
                {
                    unitOfWork.MA_LIMIT_PRODUCTRepository.Add(limitproduct);
                    unitOfWork.Commit();
                }
                else
                    throw this.CreateException(new Exception(), Messages.DUPLICATE_DATA);
            }

            return limitproduct;
        }

         public MA_LIMIT_PRODUCT UpdateLimitProduct(SessionInfo sessioninfo, MA_LIMIT_PRODUCT limitproduct)
        {

            using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
            {
                var checkDuplicate = unitOfWork.MA_LIMIT_PRODUCTRepository.GetAll().FirstOrDefault(p => p.LIMIT_ID == limitproduct.LIMIT_ID & p.PRODUCT_ID == limitproduct.PRODUCT_ID && p.ID != limitproduct.ID);
                if (checkDuplicate != null)
                    throw this.CreateException(new Exception(), Messages.DUPLICATE_DATA);

                var foundlimitproduct = unitOfWork.MA_LIMIT_PRODUCTRepository.All().FirstOrDefault(p => p.ID == limitproduct.ID);
                if (foundlimitproduct == null)
                    throw this.CreateException(new Exception(), Messages.DATA_NOT_FOUND);
                else
                {

                    foundlimitproduct.ID = limitproduct.ID;
                    foundlimitproduct.LIMIT_ID = limitproduct.LIMIT_ID;
                    foundlimitproduct.MA_LIMIT = limitproduct.MA_LIMIT;
                    foundlimitproduct.MA_PRODUCT = limitproduct.MA_PRODUCT;
                    foundlimitproduct.PRODUCT_ID = limitproduct.PRODUCT_ID;

                    unitOfWork.Commit();

                }
            }

            return limitproduct;
        }

         private bool ValidateProfileFunction(MA_LIMIT_PRODUCT data)
        {
            List<MA_LIMIT_PRODUCT> oldData = null;

            using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
            {
                oldData = unitOfWork.MA_LIMIT_PRODUCTRepository.GetAll().Where(t => t.PRODUCT_ID == data.PRODUCT_ID && t.LIMIT_ID == data.LIMIT_ID).ToList();
                if (oldData.Count > 0)
                    return false;
                else
                    return true;
            }
        }
    }
}
