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
using System.Linq.Expressions;
using System.Data;
using System.Reflection;

namespace KK.DealMaker.Business.Master
{
    public class PCCFConfigBusiness : BaseBusiness
    {
        #region MA_CONFIG_ATTRIBUTE
        public List<MA_CONFIG_ATTRIBUTE> GetFactorOptions()
        {
            List<MA_CONFIG_ATTRIBUTE> factorList;
            using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
            {
                factorList = unitOfWork.MA_CONFIG_ATTRIBUTERepository.All().ToList();
            }
            return factorList;

        }

        public List<MA_CONFIG_ATTRIBUTE> GetFactorByFilter(SessionInfo sessioninfo, Guid ID)
        {
            try
            {
                //IEnumerable<MA_USER> query;
                List<MA_CONFIG_ATTRIBUTE> sortedRecords;
                using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
                {
                    sortedRecords = unitOfWork.MA_CONFIG_ATTRIBUTERepository.All().Where(p => p.PCCF_CONFIG_ID.Equals(ID)).OrderBy(r => r.TABLE).ToList();

                }
                //Return result to jTable
                return sortedRecords;

            }
            catch (DataServicesException ex)
            {
                throw this.CreateException(ex, null);
            }
        }

        public MA_CONFIG_ATTRIBUTE CreateFactor(SessionInfo sessioninfo, MA_CONFIG_ATTRIBUTE factor)
        {
            using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
            {
                var checkDuplicate = unitOfWork.MA_CONFIG_ATTRIBUTERepository.GetAll().FirstOrDefault(p => p.PCCF_CONFIG_ID == factor.PCCF_CONFIG_ID && p.TABLE == factor.TABLE && p.ATTRIBUTE == factor.ATTRIBUTE && p.ISACTIVE);
                if (checkDuplicate != null)
                    throw this.CreateException(new Exception(), Messages.DUPLICATE_DATA);

                unitOfWork.MA_CONFIG_ATTRIBUTERepository.Add(factor);
                unitOfWork.Commit();
            }

            return factor;
        }

        public MA_CONFIG_ATTRIBUTE UpdateFactor(SessionInfo sessioninfo, MA_CONFIG_ATTRIBUTE factor)
        {
            using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
            {
                var checkDuplicate = unitOfWork.MA_CONFIG_ATTRIBUTERepository.GetAll().FirstOrDefault(p => p.PCCF_CONFIG_ID == factor.PCCF_CONFIG_ID && p.TABLE == factor.TABLE && p.ATTRIBUTE == factor.ATTRIBUTE && p.ISACTIVE && p.ID != factor.ID);
                if (checkDuplicate != null)
                    throw this.CreateException(new Exception(), Messages.DUPLICATE_DATA);

                var foundfactor = unitOfWork.MA_CONFIG_ATTRIBUTERepository.All().FirstOrDefault(p => p.ID == factor.ID);
                if (foundfactor == null)
                    throw this.CreateException(new Exception(), "Data not found!");
                else
                {

                    foundfactor.ID = factor.ID;
                    foundfactor.TABLE = factor.TABLE;
                    foundfactor.ATTRIBUTE = factor.ATTRIBUTE;
                    foundfactor.VALUE = factor.VALUE;
                    foundfactor.ISACTIVE = factor.ISACTIVE;
                    foundfactor.PCCF_CONFIG_ID = factor.PCCF_CONFIG_ID;
                    foundfactor.LOG.MODIFYBYUSERID = factor.LOG.MODIFYBYUSERID;
                    foundfactor.LOG.MODIFYDATE = factor.LOG.MODIFYDATE;
                    unitOfWork.Commit();

                }
            }

            return factor;
        }

        #endregion

        #region MA_PCCF_CONFIG
        public List<MA_PCCF_CONFIG> GetConfigByFilter(SessionInfo sessioninfo, string name, string sorting)
        {
            try
            {
                //IEnumerable<MA_USER> query;
                IEnumerable<MA_PCCF_CONFIG> sortedRecords;
                using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
                {
                    var query = unitOfWork.MA_PCCF_CONFIGRepository.GetAll().AsQueryable();

                    //Filters
                    if (!string.IsNullOrEmpty(name))
                    {
                        query = query.Where(p => p.LABEL.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0);
                    }

                    //Sorting
                    string[] sortsp = sorting.Split(' ');
                    IQueryable<MA_PCCF_CONFIG> orderedRecords = query.OrderBy(sortsp[0], sortsp[1]);
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

        public MA_PCCF_CONFIG CreateConfig(SessionInfo sessioninfo, MA_PCCF_CONFIG data)
        {
            using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
            {
                var checkDuplicate = unitOfWork.MA_PCCF_CONFIGRepository.GetAll().FirstOrDefault(p => p.LABEL.ToLower().Equals(data.LABEL.ToLower()));
                if (checkDuplicate != null)
                    throw this.CreateException(new Exception(), Messages.DUPLICATE_DATA);

                unitOfWork.MA_PCCF_CONFIGRepository.Add(data);
                unitOfWork.Commit();
            }

            return data;
        }

        public MA_PCCF_CONFIG UpdateConfig(SessionInfo sessioninfo, MA_PCCF_CONFIG data)
        {
            using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
            {
                var checkDuplicate = unitOfWork.MA_PCCF_CONFIGRepository.GetAll().FirstOrDefault(p => p.LABEL.ToLower().Equals(data.LABEL.ToLower()) && p.ID != data.ID);
                if (checkDuplicate != null)
                    throw this.CreateException(new Exception(), Messages.DUPLICATE_DATA);

                var found = unitOfWork.MA_PCCF_CONFIGRepository.All().FirstOrDefault(p => p.ID == data.ID);
                if (found == null)
                    throw this.CreateException(new Exception(), "Data not found!");
                else
                {

                    found.ID = data.ID;
                    found.ISACTIVE = data.ISACTIVE;
                    found.DESCRIPTION = data.DESCRIPTION;
                    found.LABEL = data.LABEL;
                    found.PCCF_ID = data.PCCF_ID;
                    found.PRODUCT_ID = data.PRODUCT_ID;
                    found.PRIORITY = data.PRIORITY;
                    found.LOG.MODIFYBYUSERID = data.LOG.MODIFYBYUSERID;
                    found.LOG.MODIFYDATE = data.LOG.MODIFYDATE;
                    unitOfWork.Commit();

                }
            }

            return data;
        }

        public MA_PCCF ValidatePCCFConfig(SessionInfo sessioninfo, DA_TRN record)
        {
            MA_PCCF val = null;
            bool blnResult;
            //enter conditions
            LoggingHelper.Debug("Begin validate PCCF from config");
            using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
            {
                var props = record.GetType().GetProperties();
                var configs = unitOfWork.MA_PCCF_CONFIGRepository.GetAll()
                                .Where(p => p.PRODUCT_ID.Equals(record.PRODUCT_ID))
                                .OrderBy(p => p.PRIORITY);

                //bool isBond = string.Compare(unitOfWork.MA_PRODUCTRepository.GetById(record.PRODUCT_ID.Value).LABEL, ProductCode.BOND.ToString(), true) == 0;
                                                    
                if (configs != null)
                {
                    List<bool> ismatchs = new List<bool>();
                    object obj;
                    foreach (MA_PCCF_CONFIG p in configs.ToList())
                    {
                        ismatchs.Clear();
                        blnResult = false;

                        //List<MA_CONFIG_ATTRIBUTE> atts = unitOfWork.MA_CONFIG_ATTRIBUTERepository.All().Where(t => t.PCCF_CONFIG_ID == p.ID && t.ISACTIVE == true).ToList() ;

                        foreach (MA_CONFIG_ATTRIBUTE ca in p.MA_CONFIG_ATTRIBUTE.Where(t => t.ISACTIVE == true).ToList())
                        {
                            //Fix for Tables
                            switch (ca.TABLE)
                            {
                                case "DA_TRN":
                                    var sel = props.FirstOrDefault(t => t.Name.Equals(ca.ATTRIBUTE));
                                    if (sel != null)
                                    {
                                        if (sel.GetValue(record, null).GetType() == typeof(Guid))
                                        {
                                            switch (ca.ATTRIBUTE)
                                            {
                                                //Fix for fields
                                                case "INSTRUMENT_ID":
                                                    obj = new MA_INSTRUMENT();
                                                    obj = unitOfWork.MA_INSTRUMENTRepository.GetByIDWithOutInclude(new Guid(sel.GetValue(record, null).ToString()));

                                                    blnResult = ((MA_INSTRUMENT)obj).LABEL.Equals(ca.VALUE, StringComparison.CurrentCultureIgnoreCase);
                                                    //ismatchs.Add(blnResult);

                                                    break;
                                                default:
                                                    break;
                                            }
                                        }
                                        else if (sel.GetValue(record, null).GetType() == typeof(Boolean))
                                            blnResult = sel.GetValue(record, null).Equals(ca.VALUE == "0" ? false : true);// ismatchs.Add(fsel.GetValue(record.FIRST, null).Equals(ca.VALUE=="0" ? false : true));
                                        else
                                        {
                                            blnResult = sel.GetValue(record, null).ToString().Equals(ca.VALUE, StringComparison.CurrentCultureIgnoreCase);
                                            //ismatchs.Add(blnResult);
                                        }
                                    }
                                    break;
                                case "BOND_MARKET":
                                    MA_INSTRUMENT insMkt = unitOfWork.MA_INSTRUMENTRepository.GetByIDWithInsMarket(record.INSTRUMENT_ID.Value);

                                    if (insMkt.MA_BOND_MARKET != null)
                                    {
                                        blnResult = insMkt.MA_BOND_MARKET.LABEL.Equals(ca.VALUE);
                                        //ismatchs.Add(blnResult);
                                    }
                                    //else
                                        //ismatchs.Add(false);

                                    break;
                                case "FIRST":
                                    var fsel = record.FIRST.GetType().GetProperties().FirstOrDefault(f => f.Name.Equals(ca.ATTRIBUTE));
                                    if (fsel != null)
                                    {
                                        if (fsel.GetValue(record.FIRST, null) != null)
                                        {
                                            if (fsel.GetValue(record.FIRST, null).GetType() == typeof(Guid))
                                            {
                                                switch (ca.ATTRIBUTE)
                                                {
                                                    //Fix for fields
                                                    case "CCY_ID":
                                                        obj = new MA_CURRENCY();
                                                        obj = unitOfWork.MA_CURRENCYRepository.GetByID(new Guid(fsel.GetValue(record.FIRST, null).ToString()));

                                                        blnResult = ((MA_CURRENCY)obj).LABEL.Equals(ca.VALUE, StringComparison.CurrentCultureIgnoreCase);
                                                        //ismatchs.Add();

                                                        break;
                                                    default:
                                                        break;
                                                }
                                            }
                                            else if (fsel.GetValue(record.FIRST, null).GetType() == typeof(Boolean))
                                                blnResult = fsel.GetValue(record.FIRST, null).Equals(ca.VALUE == "0" ? false : true);// ismatchs.Add(fsel.GetValue(record.FIRST, null).Equals(ca.VALUE=="0" ? false : true));
                                            else
                                                blnResult = fsel.GetValue(record.FIRST, null).ToString().Equals(ca.VALUE, StringComparison.CurrentCultureIgnoreCase); //ismatchs.Add(fsel.GetValue(record.FIRST, null).ToString().Equals(ca.VALUE, StringComparison.CurrentCultureIgnoreCase));
                                        }
                                        //else ismatchs.Add(false);
                                    }
                                    break;
                                case "SECOND":
                                    //var secprop = props.FirstOrDefault(f => f.Name.Equals("SECOND"));
                                    var ssel = record.SECOND.GetType().GetProperties().FirstOrDefault(f => f.Name.Equals(ca.ATTRIBUTE));
                                    if (ssel != null)
                                    {
                                        if (ssel.GetValue(record.SECOND, null) != null)
                                        {
                                            if (ssel.GetValue(record.SECOND, null).GetType() == typeof(Guid))
                                            {
                                                switch (ca.ATTRIBUTE)
                                                {
                                                    //Fix for fields
                                                    case "CCY_ID":
                                                        obj = new MA_CURRENCY();
                                                        obj = unitOfWork.MA_CURRENCYRepository.GetByID(new Guid(ssel.GetValue(record.SECOND, null).ToString()));

                                                        blnResult = ((MA_CURRENCY)obj).LABEL.Equals(ca.VALUE, StringComparison.CurrentCultureIgnoreCase);
                                                        //ismatchs.Add(((MA_CURRENCY)obj).LABEL.Equals(ca.VALUE, StringComparison.CurrentCultureIgnoreCase));

                                                        break;
                                                    default:
                                                        break;
                                                }
                                            }
                                            else if (ssel.GetValue(record.SECOND, null).GetType() == typeof(Boolean))
                                                blnResult = ssel.GetValue(record.SECOND, null).Equals(ca.VALUE == "0" ? false : true);// ismatchs.Add(ssel.GetValue(record.SECOND, null).Equals(ca.VALUE == "0" ? false : true));
                                            else
                                                blnResult = ssel.GetValue(record.SECOND, null).ToString().Equals(ca.VALUE, StringComparison.CurrentCultureIgnoreCase);// ismatchs.Add(ssel.GetValue(record.SECOND, null).ToString().Equals(ca.VALUE, StringComparison.CurrentCultureIgnoreCase));
                                        }
                                        //else ismatchs.Add(false);
                                    }
                                    break;

                                case "COUNTERPARTY":
                                    MA_COUTERPARTY ctpy = unitOfWork.MA_COUTERPARTYRepository.GetAll().FirstOrDefault(z => z.ID == record.CTPY_ID);

                                    switch (ca.ATTRIBUTE)
                                    {
                                        case "SNAME" :
                                            blnResult = ctpy.SNAME.ToUpper() == ca.VALUE.ToUpper();

                                            break;

                                        case "CSA_TYPE":

                                            MA_CSA_AGREEMENT csa = unitOfWork.MA_CSA_AGREEMENTRepository.GetAll().FirstOrDefault(y => y.ID == record.CTPY_ID);

                                            if (csa != null
                                                && csa.MA_CSA_TYPE.LABEL.ToUpper() == ca.VALUE.ToUpper()
                                                && csa.MA_CSA_PRODUCT.FirstOrDefault(y => y.PRODUCT_ID == record.PRODUCT_ID) != null)

                                                blnResult = true;
                                            //else if (csa == null && ca.VALUE == "")
                                            //    blnResult = true;
                                            else
                                                blnResult = false;

                                            break;
                                        default:
                                            break;
                                    }

                                    break;
                            }

                            ismatchs.Add(blnResult);

                            //Check result -> if false then go to next config
                            if (!blnResult)
                                break;
                        }

                        if (ismatchs.Where(m => m.Equals(false)).Count() == 0)
                        {
                            val = unitOfWork.MA_PCCFRepository.GetByID(p.PCCF_ID);
                            LoggingHelper.Debug("PCCF: " + val.LABEL);
                            break;
                        }
                        else continue;
                    }        
                }
                else
                    val = null;          
            }
            LoggingHelper.Debug("End validate PCCF from config");
            return val;
        }

        
        #endregion
    }
}
