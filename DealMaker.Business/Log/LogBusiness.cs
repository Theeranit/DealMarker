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

namespace KK.DealMaker.Business.Log
{
    public class LogBusiness : BaseBusiness
    {
        public List<DA_LOGGING> GetLogAll()
        {            
            List<DA_LOGGING> logList;
            using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
            {
                logList = unitOfWork.DA_LOGGINGRepository.All().ToList();
            }
            return logList;
        }
        public DA_LOGGING CreateLogging<T>(SessionInfo sessioninfo, Guid RecordID, string strEvent, LookupFactorTables TableName, string strObjType, T obj)
        {
            DA_LOGGING ret = new DA_LOGGING();
            ret.ID = Guid.NewGuid();
            ret.EVENT = strEvent;
            ret.TABLE_NAME = TableName.ToString();
            ret.RECORD_ID = RecordID;
            ret.LOG_DATE = DateTime.Now;
            StringBuilder strLog = new StringBuilder();
            strLog.Append("Create New " + strObjType);

            foreach (var item in obj.GetType().GetProperties())
            {
                var prop = item.GetValue(obj, null);

                strLog.Append("; ");
                strLog.Append(item.Name + "=");
                if (prop is decimal)
                    strLog.Append(((decimal)prop).ToString("#,##0"));
                else if (prop is DateTime)
                    strLog.Append(((DateTime)prop).ToString("dd-MMM-yyyy"));
                else
                    strLog.Append(prop.ToString());
            }

            ret.LOG_DETAIL = strLog.ToString();
            ret.LOG.INSERTBYUSERID = sessioninfo.CurrentUserId;
            ret.LOG.INSERTDATE = DateTime.Now;
            return ret;
        }

        public DA_LOGGING UpdateLogging<T>(SessionInfo sessioninfo, Guid RecordID, string strEvent, LookupFactorTables TableName, T oldTrn, T newTrn, string strAddDetail = "")
        {
            DA_LOGGING ret = new DA_LOGGING();
            ret.ID = Guid.NewGuid();
            ret.EVENT = strEvent;
            ret.TABLE_NAME = TableName.ToString();
            ret.RECORD_ID = RecordID;
            ret.LOG_DATE = DateTime.Now;
            ret.LOG.INSERTBYUSERID = sessioninfo.CurrentUserId;
            ret.LOG.INSERTDATE = DateTime.Now;
            StringBuilder strLog = new StringBuilder();
            foreach (var item in oldTrn.GetType().GetProperties())
            {
                var oldVal = item.GetValue(oldTrn, null);
                var newVal = newTrn.GetType().GetProperty(item.Name).GetValue(newTrn, null);
                if (!Object.Equals(oldVal, newVal))
                {
                    if (oldVal is Decimal)
                    {
                        int length = oldVal.ToString().Substring(oldVal.ToString().IndexOf(".")).Length;
                        length = length > 0 ? length : 0;
                        string result = new String('0', length);

                        oldVal = Decimal.Round(Decimal.Parse(oldVal.ToString()), length).ToString("0." + result);
                        newVal = Decimal.Round(Decimal.Parse(newVal.ToString()), length).ToString("0." + result);
                    }
                    strLog.Append(item.Name);
                    strLog.Append(" : ");
                    oldVal = oldVal is DateTime ? DateTime.Parse(oldVal.ToString()).ToString("dd-MMM-yyy") : oldVal;
                    strLog.Append(oldVal);
                    strLog.Append(" -> ");
                    newVal = newVal is DateTime ? DateTime.Parse(newVal.ToString()).ToString("dd-MMM-yyy") : newVal;
                    strLog.Append(newVal);
                    strLog.Append("; ");
                }
            }

            ret.LOG_DETAIL = (strAddDetail != "" ? strAddDetail + "; " : strAddDetail) + strLog.ToString();
            if (ret.LOG_DETAIL == string.Empty) ret = null;
            strLog.Clear();
            return ret;
        }
    }
}
