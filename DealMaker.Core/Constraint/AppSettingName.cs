using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KK.DealMaker.Core.Constraint
{
    public class AppSettingName
    {
        public const string CONNECTION_STRING = "ConnectionString";
        public const string ORACLE_CONNECTION_STRING = "OracleConnectionString";
        public const string TOKEN = "Token";
        public const string OPICS_OWNER = "OPICS_OWNER";
        public const string LDAP_SERVER = "LdapServer";
        public const string LDAP_COMPONENT = "LdapComponent";
        public const string CASHFLOW_SPAN = "CashFlowSpan";
        public const string CHECK_AD_USER = "CheckADUser";
        public const string SYSTEM_KEY = "SystemKey";
        public const string SYSTEM_ID = "SystemID";
        public const string AD_LOGIN = "ADLogin";
        public const string ENABLE_TBMA_CALC_SERVICE = "EnableTBMACalcService";
        public const string PCE_OVERWRITE = "PCEOverwrite";
        public const string SET_OVERWRITE = "SETOverwrite";
        public const string COUNTRY_OVERWRITE = "CountryOverwrite";

        public AppSettingName()
        { 
            //Constructor
            
        }
    }
}
