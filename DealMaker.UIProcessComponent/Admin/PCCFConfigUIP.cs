using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KK.DealMaker.Core.Common;
using KK.DealMaker.Core.Data;
using KK.DealMaker.Business.Master;
using KK.DealMaker.Core.SystemFramework;
using KK.DealMaker.Core.Helper;
using KK.DealMaker.Core.Constraint;

namespace KK.DealMaker.UIProcessComponent.Admin
{
    public class PCCFConfigUIP : BaseUIP
    {
        public static List<Factor> iFactors { get; private set; }

        #region MA_CONFIG_ATTRIBUTE
        public static object GetTables(SessionInfo sessioninfo)
        { 
            try
            {
                iFactors = XmlHelper.GetFactors();
                //Get data from database
                var tables = iFactors.Select(c => new { DisplayText = c.Name, Value = c.Value });
                var list = tables.ToList();
                list.Insert(0, new { DisplayText = "Please select...", Value = string.Empty });
                //Return result to jTable
                return new { Result = "OK", Options = list };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }
        }

        public static object GetColumns(SessionInfo sessioninfo, string tablename)
        {
            try
            {
                //Get data from database

                if (!String.IsNullOrEmpty(tablename))
                {
                    var table = iFactors.FirstOrDefault(p => p.Value.Equals(tablename));
                    var columns = table.Childs.Select(c => new { DisplayText = c.Name, Value = c.Value });
                    var list = columns.ToList();
                    list.Insert(0, new { DisplayText = "Please select...", Value = string.Empty });
                    return new { Result = "OK", Options = list };
                }
                else
                {
                    //Return result to jTable
                    return new { Result = "OK", Options = new { DisplayText = "Please select...", Value = string.Empty } };
                }
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }
        }

        public static object GetFactorByFilter(SessionInfo sessioninfo, Guid ID)
        {
            try
            {
                LoggingHelper.Debug("Begin Get Factor by " + ID);
                //Return result to jTable
                PCCFConfigBusiness _factorBusiness = new PCCFConfigBusiness();
                //Get data from database
                List<MA_CONFIG_ATTRIBUTE> factors = _factorBusiness.GetFactorByFilter(sessioninfo, ID);
                LoggingHelper.Debug("End Get Factor");
                //Return result to jTable
                return new { Result = "OK", Records = factors, TotalRecordCount = factors.Count };
            }
            catch (BusinessWorkflowsException bex)
            {
                return new { Result = "ERROR", Message = bex.Message };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }
        }

        public static object CreateFactor(SessionInfo sessioninfo, MA_CONFIG_ATTRIBUTE record)
        {
            try
            {
                LoggingHelper.Debug("Create Factor by " + record.ATTRIBUTE);

                PCCFConfigBusiness _factorBusiness = new PCCFConfigBusiness();
                record.ID = Guid.NewGuid();
                record.ISACTIVE = record.ISACTIVE == null || !record.ISACTIVE ? false : true;
                record.LOG.INSERTBYUSERID = sessioninfo.CurrentUserId;
                record.LOG.INSERTDATE = DateTime.Now;

                LoggingHelper.Debug("End Create");
                var added = _factorBusiness.CreateFactor(sessioninfo, record);
                return new { Result = "OK", Record = added };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }
        }

        public static object UpdateFactor(SessionInfo sessioninfo, MA_CONFIG_ATTRIBUTE record)
        {
            try
            {
                LoggingHelper.Debug("Update Factor by " + record.ATTRIBUTE);

                PCCFConfigBusiness _factorBusiness = new PCCFConfigBusiness();
                record.ISACTIVE = record.ISACTIVE == null || !record.ISACTIVE ? false : true;
                record.LOG.MODIFYBYUSERID = sessioninfo.CurrentUserId;
                record.LOG.MODIFYDATE = DateTime.Now;
                var updated = _factorBusiness.UpdateFactor(sessioninfo, record);
                LoggingHelper.Debug("End Update");
                return new { Result = "OK" };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }
        }
        #endregion

        #region MA_PCCF_CONFIG
        public static object GetConfigByFilter(SessionInfo sessioninfo, string name, int jtStartIndex, int jtPageSize, string jtSorting)
        {
            try
            {
                LoggingHelper.Debug("Begin Get config by " + name);
                //Return result to jTable
                PCCFConfigBusiness _factorBusiness = new PCCFConfigBusiness();
                //Get data from database
                List<MA_PCCF_CONFIG> factors = _factorBusiness.GetConfigByFilter(sessioninfo, name, jtSorting);
                LoggingHelper.Debug("End Get config");
                //Return result to jTable
                return new
                {
                    Result = "OK"
                 ,
                    Records = jtPageSize > 0 ? factors.Skip(jtStartIndex).Take(jtPageSize).ToList() : factors
                 ,
                    TotalRecordCount = factors.Count
                };
            }
            catch (BusinessWorkflowsException bex)
            {
                return new { Result = "ERROR", Message = bex.Message };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }
        }

        public static object CreateConfig(SessionInfo sessioninfo, MA_PCCF_CONFIG record)
        {
            try
            {
                LoggingHelper.Debug("Create Config by " + record.LABEL);

                PCCFConfigBusiness _factorBusiness = new PCCFConfigBusiness();
                record.ID = Guid.NewGuid();
                record.LABEL = record.LABEL.ToUpper();
                record.DESCRIPTION = record.DESCRIPTION.ToUpper();
                record.ISACTIVE = record.ISACTIVE == null || !record.ISACTIVE.Value ? false : true;
                record.LOG.INSERTBYUSERID = sessioninfo.CurrentUserId;
                record.LOG.INSERTDATE = DateTime.Now;

                LoggingHelper.Debug("End Create");
                var added = _factorBusiness.CreateConfig(sessioninfo, record);
                return new { Result = "OK", Record = added };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }
        }

        public static object UpdateConfig(SessionInfo sessioninfo, MA_PCCF_CONFIG record)
        {
            try
            {
                LoggingHelper.Debug("Update Config by " + record.LABEL);

                PCCFConfigBusiness _factorBusiness = new PCCFConfigBusiness();
                record.ISACTIVE = record.ISACTIVE == null || !record.ISACTIVE.Value ? false : true;
                record.LABEL = record.LABEL.ToUpper();
                record.DESCRIPTION = record.DESCRIPTION.ToUpper();
                record.LOG.MODIFYBYUSERID = sessioninfo.CurrentUserId;
                record.LOG.MODIFYDATE = DateTime.Now;
                var updated = _factorBusiness.UpdateConfig(sessioninfo, record);
                LoggingHelper.Debug("End Update");
                return new { Result = "OK" };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }
        }

        #endregion

    }
}
