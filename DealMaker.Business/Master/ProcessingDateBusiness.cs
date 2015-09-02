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
    public class ProcessingDateBusiness : BaseBusiness
    {
        public MA_PROCESS_DATE Get()
        {
            try
            {
                MA_PROCESS_DATE prcDate = null;

                using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
                {
                    prcDate = unitOfWork.MA_PROCESS_DATERepository.All().First();
                }

                return prcDate;
            }
            catch (DataServicesException ex)
            {
                throw this.CreateException(ex, null);
            }

        }

        public MA_PROCESS_DATE Update(SessionInfo sessioninfo, MA_PROCESS_DATE processdate)
        {

            using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
            {
                var found = unitOfWork.MA_PROCESS_DATERepository.All().First();
                if (found == null)
                    throw this.CreateException(new Exception(), "Data not found!");
                else
                {

                    found.NEXT_PROC_DATE = processdate.NEXT_PROC_DATE;
                    found.PREV_PROC_DATE = processdate.PREV_PROC_DATE;
                    found.PROC_DATE = processdate.PROC_DATE;
                    found.FLAG_RECONCILE = processdate.FLAG_RECONCILE;
                    found.LOG.MODIFYBYUSERID = processdate.LOG.MODIFYBYUSERID;
                    found.LOG.MODIFYDATE = processdate.LOG.MODIFYDATE;
                    found.FLAG_RECONCILE = processdate.FLAG_RECONCILE;
                    unitOfWork.Commit();

                }
            }

            return processdate;
        }
    }
}
