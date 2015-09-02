using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;

using KK.DealMaker.Core.Helper;
using KK.DealMaker.Core.Oracle;
using KK.DealMaker.Core.OpicsData;
using KK.DealMaker.Core.Constraint;

namespace KK.DealMaker.Business.External
{
    public class OpicsBusiness : BaseBusiness
    {
        #region Property
        private OracleHelper _oracle = null;

        public OracleHelper Oracle
        {
            get { return _oracle; }
        }
        #endregion
        #region Instance Constructor

        public OpicsBusiness()
        {
            try
            {
                if (_oracle == null)
                    _oracle = OracleHelper.GetOracleInstance(OracleConnectionString);
            }
            catch (Exception excep)
            {
                LoggingHelper.Debug("Error BaseBusiness: " + excep.Message);
            }
        }

        #endregion

        #region Method
        public List<CUSTModel> GetOPICSCustomerByName(string name)
        {
            List<CUSTModel> custs = null;

            try
            {                
                using (IDbConnection connection = Oracle.CreateOpenConnection())
                {
                    using (IDbCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "SELECT trim(CNO) CNO, trim(CMNE) CMNE, trim(SN) SN, trim(CTYPE) CTYPE, trim(CCODE) CCODE FROM OPICS.CUST WHERE CMNE LIKE '" + name + "%' ";
                        command.CommandType = CommandType.Text;
                        LoggingHelper.Debug("====" + command.CommandText + "======");
                        using (IDataReader reader = command.ExecuteReader())
                        {
                            custs = this.MapDataReaderToList<CUSTModel>(reader);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LoggingHelper.Debug("Error: " + ex.Message);
                
            }
            return custs;
        }

        public List<COUNModel> GetOPICSCountryByLabel(string label)
        {
            List<COUNModel> countries = null;

            try
            {
                using (IDbConnection connection = Oracle.CreateOpenConnection())
                {
                    using (IDbCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "SELECT TRIM(CCODE) AS CCODE , TRIM(COUN) AS COUN FROM OPICS.COUN WHERE CCODE LIKE '" + label + "%' ";
                        command.CommandType = CommandType.Text;
                        LoggingHelper.Debug("====" + command.CommandText + "======");
                        using (IDataReader reader = command.ExecuteReader())
                        {
                            countries = this.MapDataReaderToList<COUNModel>(reader);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LoggingHelper.Debug("Error: " + ex.Message);

            }
            return countries;
        }

        public List<SECMModel> GetOPICSInstrumentByLabel(string label)
        {
            List<SECMModel> secm = null;
            try
            {

                using (IDbConnection connection = Oracle.CreateOpenConnection())
                {
                    using (IDbCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "SELECT trim(SECID) SECID, trim(ISSUER) ISSUER, DENOM, COUPRATE_8, TO_CHAR(MDATE , 'mm/dd/yyyy') MDATE, INTCALCRULE, PMTFREQ, CCY FROM OPICS.SECM WHERE SECID LIKE '" + label + "%' ";
                        command.CommandType = CommandType.Text;
                        LoggingHelper.Debug("====" + command.CommandText + "======");
                        using (IDataReader reader = command.ExecuteReader())
                        {
                            secm = this.MapDataReaderToList<SECMModel>(reader);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LoggingHelper.Debug("Error: " + ex.Message);
            }
            return secm;
        }
        public List<DEALModel> GetOPICSDealExternal(DateTime processdate)
        {
            List<DEALModel> deals = null;

            try
            {
                Oracle.OpenConnection();
                Oracle.CreateCommand(string.Format("{0}.{1}", Owner, ProcedureName.SP_KKB_TRO_DMK_OS), CommandType.StoredProcedure);
                Oracle.SetInputParameter("p_date", processdate.ToString(FormatTemplate.DATETIME_LABEL));
                DataSet result = Oracle.ExecuteReader("ref_rpt_cur", "DataSet");

                deals = CollectionHelper.ConvertTo<DEALModel>(result.Tables[0]).ToList();
                
            }
            catch (Exception ex)
            {
                LoggingHelper.Debug("Error: " + ex.Message);

            }
            return deals;
        }

        public List<CASHFLOWModel> GetOPICSCashflow(DateTime processdate)
        {
            List<CASHFLOWModel> cashflows = null;

            try
            {
                using (IDbConnection connection = Oracle.CreateOpenConnection())
                {
                    using (IDbCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "SELECT * FROM OPICS.V_DMK_CASHFLOW WHERE MATDATE > TO_DATE('" + processdate.ToString("yyyyMMdd") + "', 'YYYYMMDD')" ;
                        command.CommandType = CommandType.Text;
                        LoggingHelper.Debug("====" + command.CommandText + "======");
                        using (IDataReader reader = command.ExecuteReader())
                        {
                            cashflows = this.MapDataReaderToList<CASHFLOWModel>(reader);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LoggingHelper.Debug("Error: " + ex.Message);

            }
            return cashflows;
        }
        #endregion
    }
}
