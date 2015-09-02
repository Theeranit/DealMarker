using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.EnterpriseServices;
using KK.DealMaker.Core.Common;
using KK.DealMaker.Core.Constraint;
using KK.DealMaker.Core.Data;
using KK.DealMaker.Core.Helper;
using KK.DealMaker.DataAccess.Repositories;
using KK.DealMaker.Core.SystemFramework;
using System.Configuration;
using KK.DealMaker.Core.Helper;
using System.Linq.Expressions;


namespace KK.DealMaker.Business.Master
{
    public class FunctionBusiness : BaseBusiness
    {
        public List<MA_FUNCTIONAL> GetFunctionOptions()
        {
            List<MA_FUNCTIONAL> functionList;
            using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
            {
                functionList = unitOfWork.MA_FUNCTIONALRepository.GetAll();
            }
            return functionList;

        }

        public List<MA_FUNCTIONAL> GetFunctionByFilter(SessionInfo sessioninfo, string name, string sorting)
        {
            try
            {
                //IEnumerable<MA_USER> query;
                IEnumerable<MA_FUNCTIONAL> sortedRecords;
                using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
                {
                    var query = unitOfWork.MA_FUNCTIONALRepository.GetAll().AsQueryable();

                    //Filters
                    if (!string.IsNullOrEmpty(name))
                    {
                        query = query.Where(p => p.USERCODE.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0);
                    }
                    //IQueryable<MA_FUNCTIONAL> orderedRecords = null;
                    ////Sorting
                    //if (string.IsNullOrEmpty(sorting) || sorting.Equals("LABEL ASC"))
                    //{
                    //    orderedRecords = query.OrderBy(p => p.LABEL).AsQueryable();
                    //}
                    //else if (sorting.Equals("LABEL DESC"))
                    //{
                    //    orderedRecords = query.OrderByDescending(p => p.LABEL).AsQueryable();
                    //}
                    //else
                    //{
                    //    orderedRecords = query.OrderBy(p => p.LABEL).AsQueryable(); //Default!
                    //}
                    //sortedRecords = orderedRecords.ToList();

                    //Sorting
                    string[] sortsp = sorting.Split(' ');
                    IQueryable<MA_FUNCTIONAL> orderedRecords = query.OrderBy(sortsp[0], sortsp[1]);
                    sortedRecords = orderedRecords.ToList();
                    //if (sortsp[1].ToLower() == "desc") sortedRecords = sortedRecords.Reverse();
                }
                //Return result to jTable
                return sortedRecords.ToList();

            }
            catch (DataServicesException ex)
            {
                throw this.CreateException(ex, null);
            }
        }
        
        public MA_FUNCTIONAL CreateFunction(SessionInfo sessioninfo, MA_FUNCTIONAL function)
        {
            using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
            {
                var checkDuplicate = unitOfWork.MA_FUNCTIONALRepository.GetAll().FirstOrDefault(p => p.USERCODE.ToLower().Equals(function.USERCODE.ToLower()));
                if (checkDuplicate != null)
                    throw this.CreateException(new Exception(), Messages.DUPLICATE_DATA);

                unitOfWork.MA_FUNCTIONALRepository.Add(function);
                unitOfWork.Commit();
            }

            return function;
        }

        public MA_FUNCTIONAL UpdateFunction(SessionInfo sessioninfo, MA_FUNCTIONAL function)
        {
            using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
            {
                var checkDuplicate = unitOfWork.MA_FUNCTIONALRepository.GetAll().FirstOrDefault(p => p.USERCODE.ToLower().Equals(function.USERCODE.ToLower()) && p.ID != function.ID);
                if (checkDuplicate != null)
                    throw this.CreateException(new Exception(), Messages.DUPLICATE_DATA);

                var foundfunction = unitOfWork.MA_FUNCTIONALRepository.All().FirstOrDefault(p => p.ID == function.ID);
                if (foundfunction == null)
                    throw this.CreateException(new Exception(), Messages.DATA_NOT_FOUND);
                else
                {

                    foundfunction.ID = function.ID;
                    foundfunction.LABEL = function.LABEL;
                    foundfunction.ISACTIVE = function.ISACTIVE;
                    foundfunction.USERCODE = function.USERCODE;
                    unitOfWork.Commit();

                }
            }

            return function;
        }
    }
}
