using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KK.DealMaker.Core.Common;
using KK.DealMaker.Core.Data;
using KK.DealMaker.Core.Constraint;
using KK.DealMaker.Business.Deal;
using KK.DealMaker.Business.Master;
using KK.DealMaker.Core.SystemFramework;

namespace KK.DealMaker.UIProcessComponent.Deal
{
    public class CounterpartyUIP : BaseUIP
    {
        #region Counterparty
        public static object GetByFilter(SessionInfo sessioninfo, string name, int jtStartIndex, int jtPageSize, string jtSorting)
        {
            try
            {
                CounterpartyBusiness _counterpartyBusiness = new CounterpartyBusiness();
                //Get data from database
                List<MA_COUTERPARTY> ctpys = _counterpartyBusiness.GetByFilter(sessioninfo, name, jtSorting);

                //Return result to jTable
                return new { Result = "OK", 
                             Records = jtPageSize > 0 ? ctpys.Skip(jtStartIndex).Take(jtPageSize).ToList() : ctpys,
                             TotalRecordCount = ctpys.Count };
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

        public static object GetGroupViewByFilter(SessionInfo sessioninfo, string name, int jtStartIndex, int jtPageSize, string jtSorting)
        {
            try
            {
                CounterpartyBusiness _counterpartyBusiness = new CounterpartyBusiness();
                //Get data from database
                List<CptyLimitModel> ctpys = _counterpartyBusiness.GetGroupViewByFilter(sessioninfo, name, jtSorting);

                //Return result to jTable
                return new
                {
                    Result = "OK",
                    Records = jtPageSize > 0 ? ctpys.Skip(jtStartIndex).Take(jtPageSize).ToList() : ctpys,
                    TotalRecordCount = ctpys.Count
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

        public static object GetCounterpartyOptions(SessionInfo sessioninfo)
        {
            try
            {
                CounterpartyBusiness _counterpartyBusiness = new CounterpartyBusiness();
                //Get data from database
                var counterparties = _counterpartyBusiness.GetCounterpartyAll()
                                            .Where(t => t.ISACTIVE == true)
                                            .OrderBy(p => p.SNAME)
                                            .Select(c => new { DisplayText = c.SNAME, Value = c.ID });

                //Return result to jTable
                return new { Result = "OK", Options = counterparties };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }
        }

        public static object GetCPTYGroupOptions(SessionInfo sessioninfo)
        {
            try
            {
                CounterpartyBusiness _counterpartyBusiness = new CounterpartyBusiness();
                //Get data from database
                var counterparties = _counterpartyBusiness.GetCounterpartyAll();

                var query = counterparties.Where(t => t.ISACTIVE == true)
                                            .OrderBy(p => p.SNAME)
                                            .Select(c => new { DisplayText = c.SNAME, Value = c.ID }).ToList();
              
              query.Insert(0, new { DisplayText = "Default", Value = Guid.Empty });
                //Return result to jTable
                return new { Result = "OK", Options = query };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }
        }
        
        public static object Create(SessionInfo sessioninfo, MA_COUTERPARTY record)
        {
            try
            {
                CounterpartyBusiness _counterpartyBusiness = new CounterpartyBusiness();
                LookupBusiness _lookupBusiness = new LookupBusiness();
                MA_CTPY_LIMIT cplimit;

                record.ID = Guid.NewGuid();
                record.SNAME = record.SNAME.ToUpper();
                record.TBMA_NAME = record.TBMA_NAME.ToUpper();
                record.FNAME = record.FNAME.ToUpper();
                record.BUSINESS = record.BUSINESS.ToUpper();
                record.ISACTIVE = record.ISACTIVE == null || !record.ISACTIVE.Value ? false : true;
                record.LOG.INSERTDATE = DateTime.Now;
                record.LOG.INSERTBYUSERID = sessioninfo.CurrentUserId;
                record.GROUP_CTPY_ID = record.GROUP_CTPY_ID == Guid.Empty ? null : record.GROUP_CTPY_ID;

                //Prepare Counterparty-Limit data
                List<MA_LIMIT> limits = _lookupBusiness.GetLimitAll();

                foreach (MA_LIMIT limit in limits)
                {
                    cplimit = new MA_CTPY_LIMIT();

                    cplimit.ID = Guid.NewGuid();
                    cplimit.CTPY_ID = record.ID;
                    cplimit.LIMIT_ID = limit.ID;
                    cplimit.FLAG_CONTROL = true;
                    cplimit.AMOUNT = 0;
                    cplimit.EXPIRE_DATE = sessioninfo.Process.CurrentDate;
                    cplimit.LOG.INSERTDATE = DateTime.Now;
                    cplimit.LOG.INSERTBYUSERID = sessioninfo.CurrentUserId;

                    record.MA_CTPY_LIMIT.Add(cplimit);
                }
                
                var addedRecord = _counterpartyBusiness.Create(sessioninfo, record);

                addedRecord.MA_CTPY_LIMIT.Clear();

                return new { Result = "OK", Record = addedRecord };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }
        }

        public static object Update(SessionInfo sessioninfo, MA_COUTERPARTY record)
        {
            try
            {
                if (record.ID == record.GROUP_CTPY_ID) {
                    throw new Exception("Group can't same as Counterparty.");
                }
                CounterpartyBusiness _counterpartyBusiness = new CounterpartyBusiness();
                record.SNAME = record.SNAME.ToUpper();
                record.TBMA_NAME = record.TBMA_NAME.ToUpper();
                record.FNAME = record.FNAME.ToUpper();
                record.BUSINESS = record.BUSINESS.ToUpper();
                record.ISACTIVE = record.ISACTIVE == null || !record.ISACTIVE.Value ? false : true;
                record.LOG.MODIFYBYUSERID = sessioninfo.CurrentUserId;
                record.LOG.MODIFYDATE = DateTime.Now;
                record.GROUP_CTPY_ID = record.GROUP_CTPY_ID == Guid.Empty ? null : record.GROUP_CTPY_ID;
                var addedRecord = _counterpartyBusiness.Update(sessioninfo, record);
                return new { Result = "OK", Record = addedRecord };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }
        }
        #endregion

        #region Limit for Counterparty
        public static object GetCtpyLimitByCtpyID(SessionInfo sessioninfo, Guid ID)
        {
            try
            {
                CounterpartyBusiness _counterpartyBusiness = new CounterpartyBusiness();
                //Get data from database
                List<MA_CTPY_LIMIT> limits = _counterpartyBusiness.GetCounterpartyLimitByCtpyID(sessioninfo, ID);

                //Return result to jTable
                return new { Result = "OK", Records = limits, TotalRecordCount = limits.Count };
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
        public static object GetCtpyLimitGroupViewByCtpyID(SessionInfo sessioninfo, Guid ID)
        {
            try
            {
                CounterpartyBusiness _counterpartyBusiness = new CounterpartyBusiness();
                //Get data from database
                List<CptyLimitModel> limits = _counterpartyBusiness.GetCtpyLimitGroupViewByCtpyID(sessioninfo, ID);

                //Return result to jTable
                return new { Result = "OK", Records = limits, TotalRecordCount = limits.Count };
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
        public static object DeleteCtpyLimitByID(SessionInfo sessioninfo, Guid ID)
        {
            try
            {
                CounterpartyBusiness _counterpartyBusiness = new CounterpartyBusiness();
                bool result = _counterpartyBusiness.DeleteCounterpartyLimitByID(sessioninfo, ID);
                return new { Result = "OK", Message = Messages.DELETED_SUCCESSFULL };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }
        }
        public static object UpdateCtpyLimit(SessionInfo sessioninfo, MA_CTPY_LIMIT record)
        {
            try
            {
                CounterpartyBusiness _counterpartyBusiness = new CounterpartyBusiness();
                //record.FLAG_CONTROL = record.FLAG_CONTROL == null ? false : true;
                if (!record.FLAG_CONTROL)
                {
                    record.AMOUNT = 0;
                    record.EXPIRE_DATE = sessioninfo.Process.CurrentDate;
                }
               
                record.LOG.MODIFYBYUSERID = sessioninfo.CurrentUserId;
                record.LOG.MODIFYDATE = DateTime.Now;
                var updateRecord = _counterpartyBusiness.UpdateCounterpartyLimit(sessioninfo, record);

                return new { Result = "OK" , Record = updateRecord };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }
        }
        public static object CreateCtpyLimit(SessionInfo sessioninfo, MA_CTPY_LIMIT record)
        {
            try
            {
                CounterpartyBusiness _counterpartyBusiness = new CounterpartyBusiness();
                record.ID = Guid.NewGuid();
                record.EXPIRE_DATE = record.EXPIRE_DATE.Date;
                //record.FLAG_CONTROL = record.FLAG_CONTROL == null ? false : true;
                record.LOG.INSERTDATE = DateTime.Now;
                record.LOG.INSERTBYUSERID = sessioninfo.CurrentUserId;
                var addedRecord = _counterpartyBusiness.CreateCounterpartyLimit(sessioninfo, record);
                return new { Result = "OK", Record = addedRecord };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }
        }
        public static object GetCounterpartyByName(string name)
        {
            try
            {
                CounterpartyBusiness _counterpartyBusiness = new CounterpartyBusiness();
                //Get data from database
                var counterparties = _counterpartyBusiness.GetCounterpartyAll().Where(t => t.ISACTIVE == true && t.SNAME.StartsWith(name)).OrderBy(t => t.SNAME); ;

                //Return result to jTable
                return new { Result = "OK", Records = counterparties, };
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
        #endregion

        #region Temp Limit
        public static object GetCtpyLimitOptions(SessionInfo sessioninfo)
        {
            try
            {
                CounterpartyBusiness _counterpartyBusiness = new CounterpartyBusiness();
                //Get data from database
                var ctpylimit = _counterpartyBusiness.GetCounterpartyLimitAll();
                var ctpy = _counterpartyBusiness.GetCounterpartyAll();

                var options = from cl in ctpylimit
                              join ct in ctpy on cl.CTPY_ID equals ct.ID
                              select new {
                                  DisplayText = ct.SNAME + " : " + cl.MA_LIMIT.LABEL
                                  , SortOrder = cl.MA_LIMIT.INDEX
                                  , Value = cl.ID
                              };
                
                //Return result to jTable
                return new { Result = "OK", Options = options.OrderBy(t => t.DisplayText).ThenBy(t => t.SortOrder).Select(c => new { DisplayText = c.DisplayText, Value = c.Value }) };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }
        }

        public static object GetTempLimitByFilter(SessionInfo sessioninfo, string strCtpy, string strLimit, string strEffDateFrom, string strEffDateTo
                                                   , string strExpDateFrom, string strExpDateTo, int jtStartIndex, int jtPageSize, string jtSorting)
        {
            try
            {
                CounterpartyBusiness _counterpartyBusiness = new CounterpartyBusiness();
                //Get data from database
                List<MA_TEMP_CTPY_LIMIT> ctpys = _counterpartyBusiness.GetTempLimitByFilter(sessioninfo, strCtpy, strLimit, strEffDateFrom, strEffDateTo
                                                                                            , strExpDateFrom, strExpDateTo, jtSorting);

                //Return result to jTable
                return new
                {
                    Result = "OK",
                    Records = jtPageSize > 0 ? ctpys.Skip(jtStartIndex).Take(jtPageSize).ToList() : ctpys,
                    TotalRecordCount = ctpys.Count
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
        public static object CreateTempLimit(SessionInfo sessioninfo, MA_TEMP_CTPY_LIMIT record)
        {
            try
            {
                CounterpartyBusiness _counterpartyBusiness = new CounterpartyBusiness();

                record.ID = Guid.NewGuid();
                record.ISACTIVE = record.ISACTIVE == null || !record.ISACTIVE ? false : true;
                record.LOG.INSERTDATE = DateTime.Now;
                record.LOG.INSERTBYUSERID = sessioninfo.CurrentUserId;
                
                var addedRecord = _counterpartyBusiness.CreateTempLimit(sessioninfo, record);
                
                return new { Result = "OK", Record = addedRecord };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }
        }

        public static object UpdateTempLimit(SessionInfo sessioninfo, MA_TEMP_CTPY_LIMIT record)
        {
            try
            {
                CounterpartyBusiness _counterpartyBusiness = new CounterpartyBusiness();
                record.ISACTIVE = record.ISACTIVE == null || !record.ISACTIVE ? false : true;
                record.LOG.MODIFYBYUSERID = sessioninfo.CurrentUserId;
                record.LOG.MODIFYDATE = DateTime.Now;
                var addedRecord = _counterpartyBusiness.UpdateTempLimit(sessioninfo, record);
                return new { Result = "OK" };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }
        }
        #endregion

        #region CSA
        public static object GetCSAByCtpyID(SessionInfo sessioninfo, Guid ID)
        {
            try
            {
                CounterpartyBusiness _counterpartyBusiness = new CounterpartyBusiness();
                LookupBusiness _lookupBusiness = new LookupBusiness();
                //Get data from database
                List<CSAAgreementModel> results = new List<CSAAgreementModel>();
                var csa = _counterpartyBusiness.GetCSAByCtpyID(sessioninfo, ID);
                var products = _lookupBusiness.GetProductAll();

                if (csa != null)
                {
                    var csaproduct = from c in csa.MA_CSA_PRODUCT
                                     join product in products on c.PRODUCT_ID equals product.ID
                                     select new
                                     {
                                         LABEL = product.LABEL
                                     };

                    CSAAgreementModel ca = new CSAAgreementModel();
                    ca.CSA_TYPE_ID = csa.CSA_TYPE_ID;
                    ca.ID = csa.ID;
                    ca.ISACTIVE = csa.ISACTIVE;
                    ca.PRODUCTS = String.Join(",", csaproduct.Select(p => p.LABEL));

                    results.Add(ca);
                }

                //Return result to jTable
                return new { Result = "OK", Records = results, TotalRecordCount = results.Count };
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

        public static object CreateCSA(SessionInfo sessioninfo, MA_CSA_AGREEMENT record)
        {
            try
            {
                CounterpartyBusiness _counterpartyBusiness = new CounterpartyBusiness();

                record.LOG.INSERTBYUSERID = sessioninfo.CurrentUserId;
                record.LOG.INSERTDATE = DateTime.Now;

                var addedRecord = _counterpartyBusiness.CreateCSA(sessioninfo, record);
                
                return new { Result = "OK", Record = new CSAAgreementModel { ID = addedRecord.ID, CSA_TYPE_ID = addedRecord.CSA_TYPE_ID, ISACTIVE = addedRecord.ISACTIVE } };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }
        }

        public static object UpdateCSA(SessionInfo sessioninfo, MA_CSA_AGREEMENT record)
        {
            try
            {
                CounterpartyBusiness _counterpartyBusiness = new CounterpartyBusiness();

                record.LOG.MODIFYBYUSERID = sessioninfo.CurrentUserId;
                record.LOG.MODIFYDATE = DateTime.Now;

                var updateRecord = _counterpartyBusiness.UpdateCSA(sessioninfo, record);

                return new { Result = "OK", Record = new CSAAgreementModel { ID = updateRecord.ID, CSA_TYPE_ID = updateRecord.CSA_TYPE_ID, ISACTIVE = updateRecord.ISACTIVE } };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }
        }
        
        public static object GetCSAProducts(SessionInfo sessioninfo, Guid ID)
        {
            try
            {
                CounterpartyBusiness _counterpartyBusiness = new CounterpartyBusiness();

                List<MA_CSA_PRODUCT> csaproducts = _counterpartyBusiness.GetCSAProducts(sessioninfo, ID);
                                    
                //Return result to jTable
                return new { Result = "OK", Records = csaproducts, TotalRecordCount = csaproducts.Count };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR"};
            }
        }

        public static object CreateCSAProduct(SessionInfo sessioninfo, MA_CSA_PRODUCT record)
        {
            try
            {
                CounterpartyBusiness _counterpartyBusiness = new CounterpartyBusiness();
                
                var addedRecord = _counterpartyBusiness.CreateCSAProduct(sessioninfo, record);

                return new { Result = "OK", Record = addedRecord };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }
        }

        public static object DeleteCSAProduct(SessionInfo sessioninfo, Guid CSA_AGREEMENT_ID, Guid PRODUCT_ID)
        {
            try
            {
                CounterpartyBusiness _counterpartyBusiness = new CounterpartyBusiness();

                _counterpartyBusiness.DeleteCSAProduct(sessioninfo, CSA_AGREEMENT_ID, PRODUCT_ID);

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
