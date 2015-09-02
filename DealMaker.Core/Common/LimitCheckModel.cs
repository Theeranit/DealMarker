using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KK.DealMaker.Core.Common
{

    public enum eLimitCheckStatus
    {
        OK
        , NEEDAPPROVE
        , NOTALLOW
        , ERROR
    }

    public enum eLimitStatusCode
    {
        NORMAL
        , THRESHOLD
        , EXPIRED
        , EXCEED
    }

    public class LimitCheckModel
    {
        public string SNAME { get; set; }
        public Guid CTPY_LIMIT_ID { get; set; }
        public string LIMIT_LABEL { get; set; }
        public bool FLAG_CONTROL { get; set; }
        //public decimal AMOUNT { get; set; }
        public DateTime EXPIRE_DATE { get; set; }
        public DateTime FLOW_DATE { get; set; }
        public DateTime PROCESSING_DATE { get; set; }
        public decimal ORIGINAL_KK_CONTRIBUTE { get; set; }
        public decimal DEAL_CONTRIBUTION { get; set; }
        public int SORT_INDEX { get; set; }
        public decimal TEMP_AMOUNT { get; set; }
        public decimal GEN_AMOUNT { get; set; }


        //Country Limit Detail
        public string COUNTRY_LABEL { get; set; }
        public Guid COUNTRY_ID { get; set; }
        public decimal SET_CONTRIBUTE { get; set; }
        public decimal PCE_CONTRIBUTE { get; set; }

        public decimal AMOUNT
        {
            get
            {
                return GEN_AMOUNT + TEMP_AMOUNT;
            }
        }
        
        public decimal UTILIZATION
        {
            get
            {
                return ORIGINAL_KK_CONTRIBUTE + DEAL_CONTRIBUTION;
            }
        }

        public string UTILIZATION_PERCENT
        {
            get
            {
                return AMOUNT > 0 ? (UTILIZATION / AMOUNT * 100).ToString("0.000") : "NA";
            }
        }

        public decimal AVAILABLE
        {
            get
            {
                if (AMOUNT < UTILIZATION)
                    return 0;
                else
                    return AMOUNT - UTILIZATION;
            }
        }

        public string STATUS
        {
            get
            {
                if (!FLAG_CONTROL)
                {
                    return eLimitStatusCode.NORMAL.ToString();
                }
                else if (PROCESSING_DATE > EXPIRE_DATE)
                {
                    return eLimitStatusCode.EXPIRED.ToString();
                }
                else if (UTILIZATION > AMOUNT)
                {
                    return eLimitStatusCode.EXCEED.ToString();
                }
                else if ((AMOUNT > 0) && (UTILIZATION/AMOUNT * 100 > 80))
                {
                    return eLimitStatusCode.THRESHOLD.ToString();
                }
                else
                {
                    return eLimitStatusCode.NORMAL.ToString();
                }
            }
        }
    }

    public class PCEGroupModel
    {
        public Guid CTPY_ID { get; set; }
        public Guid LIMIT_ID { get; set; }
        public bool FLAG_CONTROL { get; set; }
        public decimal? KK_CONTRIBUTE { get; set; }
        public DateTime ENGINE_DATE { get; set; }
        public decimal? BOT_CONTRIBUTE { get; set; }
    }

    public class SCEGroupModel
    {
        public DateTime ENGINE_DATE { get; set; }
        public Guid CTPY_ID { get; set; }
        public Guid LIMIT_ID { get; set; }
        public DateTime? FLOW_DATE { get; set; }
        public Guid FLOW_CCY { get; set; }
        public decimal? FLOW_AMOUNT { get; set; }
    }
    
    public class CountryLimitModel
    {
        public DateTime ENGINE_DATE { get; set; }
        public Guid COUNTRY_ID { get; set; }
        public DateTime? EXPOSURE_DATE { get; set; }
        public decimal? EXPOSURE { get; set; }
        public Guid CCY { get; set; }
    }

    public class LimitDisplayModel
    {
        public decimal OverAmount { get; set; }
        public eLimitCheckStatus LimitCheckStatus { get; set; }
        public string Message { get; set; }
        public List<LimitCheckModel> LimitDisplayObject { get; set; }
    }

    public class LimitCheckComparer : IEqualityComparer<LimitCheckModel>
    {
        public bool Equals(LimitCheckModel x, LimitCheckModel y)
        {
            if (x.SNAME == y.SNAME
                && x.LIMIT_LABEL == y.LIMIT_LABEL
                && x.PROCESSING_DATE == y.PROCESSING_DATE
                && x.AMOUNT == y.AMOUNT
                && x.EXPIRE_DATE == y.EXPIRE_DATE
                && x.FLAG_CONTROL == y.FLAG_CONTROL
                && x.FLOW_DATE == y.FLOW_DATE
                && x.ORIGINAL_KK_CONTRIBUTE == y.ORIGINAL_KK_CONTRIBUTE
                && x.DEAL_CONTRIBUTION == y.DEAL_CONTRIBUTION)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int GetHashCode(LimitCheckModel obj)
        {
            int hashSName = obj.SNAME.GetHashCode();
            int hashLimitLabel = obj.LIMIT_LABEL.GetHashCode();
            int hashProcessingDate = obj.PROCESSING_DATE.GetHashCode();
            int hashAmount = obj.AMOUNT.GetHashCode();
            int hashExpireDate = obj.EXPIRE_DATE.GetHashCode();
            int hashFlagControl = obj.FLAG_CONTROL.GetHashCode();
            int hashFlowDate = obj.FLOW_DATE.GetHashCode();
            int hashOriCont = obj.ORIGINAL_KK_CONTRIBUTE.GetHashCode();
            int hashDealCont = obj.DEAL_CONTRIBUTION.GetHashCode();

            return hashSName
                    ^ hashLimitLabel
                    ^ hashProcessingDate
                    ^ hashAmount
                    ^ hashExpireDate
                    ^ hashFlagControl
                    ^ hashFlowDate
                    ^ hashOriCont
                    ^ hashDealCont;
        }
    }
}
