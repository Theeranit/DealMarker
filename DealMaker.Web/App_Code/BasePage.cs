using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.Services;
using KK.DealMaker.Core.Common;
using KK.DealMaker.Core.Data;
using KK.DealMaker.Core.Constraint;
using KK.DealMaker.Core.SystemFramework;
using KK.DealMaker.UIProcessComponent.Admin;
using KK.DealMaker.UIProcessComponent.Deal;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Configuration;

namespace KK.DealMaker.Web
{
    public class BasePage : System.Web.UI.Page
    {
        protected string PageName { get; set; }
        protected static string ProductId { get; set; }
        
        public BasePage()
        { 
            //BasePage Contructor
        }

        protected override void OnInit(EventArgs e)
        {
            // code before base oninit
            base.OnInit(e);
            // code after base oninit

            //1. check another access this application
            if (UserAudit == null)
                Response.Redirect("~/sessionexpired.htm");
            else if (UserAudit.LOGOUT_DATE.HasValue)
                Response.Redirect("~/sessionexpired.htm");
            
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            PageName = Path.GetFileNameWithoutExtension(Request.Path);

            //2. check allow access this page
            if (!HasPermission)
                Response.Redirect("~/pagenopermission.htm");
            
        }
        private static SessionInfo _sessionInfo;

        protected static SessionInfo SessionInfo
        {
            get {
                //if (_sessionInfo == null)
                //{
                    _sessionInfo = SessionInfoSerializer.SessionInfoFromCurrentHttpContext();
                //}
                return _sessionInfo;
            }
        }

        protected string CurrentProcessDate
        {
            get
            {
                return SessionInfo.Process.CurrentDate.ToString(FormatTemplate.DATE_DMY_LABEL);
            }
        }

        private List<PermisionModel> _permissions = null;
        protected List<PermisionModel> Permissions
        {
            get
            {
                if (_permissions == null)
                    _permissions = ProfileFunctionalUIP.GetPermissionByProfileID(SessionInfo.ProfileId);

                return _permissions;
            }
        }
        private DA_LOGIN_AUDIT _useraudit;
        protected DA_LOGIN_AUDIT UserAudit
        {
            get
            {
                if (_useraudit == null)
                    _useraudit = UserUIP.GetLogUser(SessionInfo);

                return _useraudit;
            }
        }
        private Boolean _readOnly = true;
        protected Boolean ReadOnly
        {
            get
            {
                if (SessionInfo.IsSystemUser) return true;

                if (Permissions.Count>0)
                {
                    var query = Permissions.FirstOrDefault(p => p.FunctionalCode.Contains(PageName));
                    if (query == null) _readOnly = true;
                    else
                    {
                        if (query.IsReadable) _readOnly = true;
                        else _readOnly = false;
                    }
                }
                else _readOnly = true;

                return _readOnly;
            }
        }
        private Boolean _writable = false;
        protected Boolean Writable
        {
            get
            {
                if (SessionInfo.IsSystemUser) return true;

                if (Permissions.Count > 0)
                {
                    var query = Permissions.FirstOrDefault(p => p.FunctionalCode.Contains(PageName));
                    if (query == null) _writable = true;
                    else
                    {
                        if (query.IsWritable) _writable = true;
                        else _writable = false;
                    }
                }
                else _writable = true;

                return _writable;
            }
        }
        private Boolean _approvable = false;
        protected Boolean Approvable
        {
            get
            {
                if (SessionInfo.IsSystemUser) return true;

                if (Permissions.Count > 0)
                {
                    var query = Permissions.FirstOrDefault(p => p.FunctionalCode.Contains(PageName));
                    if (query == null) _approvable = false;
                    else
                    {
                        if (query.IsApprovable) _approvable = true;
                        else _approvable = false;
                    }
                }
                else _approvable = true;

                return _approvable; 
            }
        }
        private Boolean _hasPermission;
        protected Boolean HasPermission
        {
            get
            {
                if (SessionInfo.IsSystemUser) return true;

                if (Permissions.Count > 0)
                {
                    var query = Permissions.FirstOrDefault(p => p.FunctionalCode.Contains(PageName));
                    if (query == null) _hasPermission = false;
                    else _hasPermission = true;
                }
                else _hasPermission = false;

                return _hasPermission;
            }
        }

        [WebMethod(EnableSession=true)]
        public string GetSystemDatetime()
        {
            return DateTime.Now.ToString();
        }

        #region Lookup methods

        [WebMethod(EnableSession = true)]
        public static object GetProductOptions()
        {
            return LookupUIP.GetProductOptions(SessionInfo);
        }

        [WebMethod(EnableSession = true)]
        public static object GetProductNoBondOptions()
        {
            return LookupUIP.GetProductOptions(SessionInfo, ProductCode.BOND.ToString());
        }

        //[WebMethod(EnableSession = true)]
        //public static object GetCouponTypeOptions()
        //{
        //    return LookupUIP.GetCouponTypeOptions(SessionInfo);
        //}

        [WebMethod(EnableSession = true)]
        public static object GetInstrumentOptions()
        {
            return InstrumentUIP.GetInstrumentOptions(SessionInfo);
        }

        [WebMethod(EnableSession = true)]
        public static object GetPortfolioOptions()
        {
            return LookupUIP.GetPortfolioOptions(SessionInfo);
        }

        [WebMethod(EnableSession = true)]
        public static object GetCounterpartyOptions()
        {
            return CounterpartyUIP.GetCounterpartyOptions(SessionInfo);
        }

        [WebMethod(EnableSession = true)]
        public static object GetCPTYGroupOptions()
        {
            return CounterpartyUIP.GetCPTYGroupOptions(SessionInfo);
        }
        
        [WebMethod(EnableSession = true)]
        public static object GetFIInstrumentOptions()
        {
            return InstrumentUIP.GetOptionsByProduct(SessionInfo, ProductCode.BOND);
        }

        [WebMethod(EnableSession = true)]
        public static object GetSwapInstrumentOptions()
        {
            return InstrumentUIP.GetOptionsByProduct(SessionInfo, ProductCode.SWAP);
        }

        [WebMethod(EnableSession = true)]
        public static object GetFXSpotInstrumentOptions()
        {
            return InstrumentUIP.GetOptionsByProduct(SessionInfo, ProductCode.FXSPOT);
        }

        [WebMethod(EnableSession = true)]
        public static object GetFXForwardInstrumentOptions()
        {
            return InstrumentUIP.GetOptionsByProduct(SessionInfo, ProductCode.FXFORWARD);
        }

        [WebMethod(EnableSession = true)]
        public static object GetFXSwapInstrumentOptions()
        {
            return InstrumentUIP.GetOptionsByProduct(SessionInfo, ProductCode.FXSWAP);
        }

        [WebMethod(EnableSession = true)]
        public static object GetFrequencyOptions()
        {
            return LookupUIP.GetFreqTypeOptions(SessionInfo);
        }

        [WebMethod(EnableSession = true)]
        public static object GetLimitOptions()
        {
            return LookupUIP.GetLimitOptions(SessionInfo);
        }
        [WebMethod(EnableSession = true)]
        public static object GetCurrencyOptions()
        {
            return LookupUIP.GetCurrencyOptions(SessionInfo);
        }
        [WebMethod(EnableSession = true)]
        public static object GetBondMarketOptions()
        {
            return LookupUIP.GetBondMarketOptions(SessionInfo);
        }
        [WebMethod(EnableSession = true)]
        public static object GetPCCFOptions()
        {
            return PCCFUIP.GetPCCFOptions(SessionInfo);
        }
        [WebMethod(EnableSession = true)]
        public static object GetUserOptions()
        {
            return  UserUIP.GetUserOptions(SessionInfo);
        }
                
        [WebMethod(EnableSession = true)]
        public static object GetCountryOptions()
        {
            return  CountryUIP.GetCountryOptions(SessionInfo);
        }
        
        [WebMethod(EnableSession = true)]
        public static object GetYeildTypeOptions()
        {
            return LookupUIP.GetYeildTypeOptions(SessionInfo);
        }
        [WebMethod(EnableSession = true)]
        public static object GetReportByOptions()
        {
            return LookupUIP.GetReportByOptions(SessionInfo);
        }
        [WebMethod(EnableSession = true)]
        public static object GetPurposeOptions()
        {
            return LookupUIP.GetPurposeOptions(SessionInfo);
        }
        [WebMethod(EnableSession = true)]
        public static object GetLimitEventOptions()
        {
            return LookupUIP.GetLimitEventOptions(SessionInfo);
        }
        #endregion
        

        #region Admin method
            
        [WebMethod(EnableSession = true)]
        public static object GetProfileOptions()
        {
            return ProfileUIP.GetProfileOptions(SessionInfo);
        }

        [WebMethod(EnableSession = true)]
        public static object GetFunctionOptions()
        {
            return FunctionUIP.GetFunctionOptions(SessionInfo);
        }

        #endregion

        [WebMethod(EnableSession = true)]
        public static object CheckApproveRight(string strUsername, string strPassword, string strPageName)
        {
            //First validate user at UIP
            //then check permission for this page here
            MA_USER user = UserUIP.ValidateUser(SessionInfo, strUsername, strPassword
                                                , Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings[AppSettingName.AD_LOGIN]));

            if (user == null)
            {
                return new { Result = "ERROR", Message = "Invalid username or password." };
            }
            else
            {
                //Check Permission
                List<PermisionModel> permissions = ProfileFunctionalUIP.GetPermissionByProfileID(user.USER_PROFILE_ID);

                var query = permissions.FirstOrDefault(p => p.FunctionalCode.Contains(strPageName));

                if (query == null || !query.IsApprovable)
                {
                    return new { Result = "ERROR", Message = "User has no right to approve limit." };
                }
                else
                {
                    return new { Result = "OK", Message = "" };
                }
            }

        }

        [WebMethod(EnableSession = true)]
        public static string GetSpotDateString()
        {
            if (SessionInfo.Process.CurrentDate.Date.DayOfWeek == DayOfWeek.Friday || SessionInfo.Process.CurrentDate.Date.DayOfWeek == DayOfWeek.Thursday)
            {
                return SessionInfo.Process.CurrentDate.AddDays(4).ToString(FormatTemplate.DATE_DMY_LABEL);
            }
            else
            {
                return SessionInfo.Process.CurrentDate.AddDays(2).ToString(FormatTemplate.DATE_DMY_LABEL);
            }
        }

        [WebMethod(EnableSession = true)]
        public static object ConsoLimitDisplay(bool blnSubmit, LimitDisplayModel pce, LimitDisplayModel set, LimitDisplayModel country)
        {
            object result = new object();

            if (pce.LimitCheckStatus == eLimitCheckStatus.ERROR)
            {
                result = new
                        {
                            Result = "ERROR",
                            Message = pce.Message,
                        };
            }
            else if (set != null && set.LimitCheckStatus == eLimitCheckStatus.ERROR)
            {
                result = new
                        {
                            Result = "ERROR",
                            Message = set.Message,
                        };
            }
            else if (country.LimitCheckStatus == eLimitCheckStatus.ERROR)
            {
                result = new
                        {
                            Result = "ERROR",
                            Message = country.Message,
                        };
            }
            else if (pce.LimitCheckStatus == eLimitCheckStatus.NOTALLOW
                        || (set != null ? set.LimitCheckStatus == eLimitCheckStatus.NOTALLOW : false)
                        || country.LimitCheckStatus == eLimitCheckStatus.NOTALLOW)
            {
                result = new
                {
                    Result = "NOTALLOW",
                    Message = "",
                    PCERecords = JsonConvert.SerializeObject(pce.LimitDisplayObject, new IsoDateTimeConverter()),
                    SCERecords = JsonConvert.SerializeObject(set != null ? set.LimitDisplayObject : null, new IsoDateTimeConverter()),
                    CountryRecords = JsonConvert.SerializeObject(country.LimitDisplayObject, new IsoDateTimeConverter())
                };
            }
            else if (blnSubmit && (pce.LimitCheckStatus == eLimitCheckStatus.NEEDAPPROVE
                                    || (set != null ? set.LimitCheckStatus == eLimitCheckStatus.NEEDAPPROVE : false)
                                    || country.LimitCheckStatus == eLimitCheckStatus.NEEDAPPROVE))
            {
                result = new
                        {
                            Result = "NEEDAPPROVE",
                            Message = "",
                            PCERecords = JsonConvert.SerializeObject(pce.LimitDisplayObject, new IsoDateTimeConverter()),
                            SCERecords = JsonConvert.SerializeObject(set != null ? set.LimitDisplayObject : null, new IsoDateTimeConverter()),
                            CountryRecords = JsonConvert.SerializeObject(country.LimitDisplayObject, new IsoDateTimeConverter())
                        };
            }
            else
            {
                result = new
                        {
                            Result = "OK",
                            Message = "",
                            PCERecords = JsonConvert.SerializeObject(pce.LimitDisplayObject, new IsoDateTimeConverter()),
                            SCERecords = JsonConvert.SerializeObject(set != null ? set.LimitDisplayObject : null, new IsoDateTimeConverter()),
                            CountryRecords = JsonConvert.SerializeObject(country.LimitDisplayObject, new IsoDateTimeConverter())
                        };
            }

            return result;
        }

        [WebMethod(EnableSession = true)]
        public static string IsTBMACalEnable()
        {
            return ConfigurationManager.AppSettings[AppSettingName.ENABLE_TBMA_CALC_SERVICE].ToString();
        }
    }
}