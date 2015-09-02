using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using KK.DealMaker.Core.Data;

namespace KK.DealMaker.Core.Constraint
{
    public enum UpdateStates
    { 
        Adding,
        Editing,
        Deleting
    }

    public enum CouponType
    { 
        [Display(Name="FIXED")]
        FIXED = 0,
        [Display(Name = "FLOAT")]
        FLOAT = 1
    }

    public enum ProductCode { 
        BOND = 0,
        SWAP = 1,
        FXSPOT = 2,
        FXFORWARD = 3,
        FXSWAP = 4,
        REPO = 5
    }

    //public enum ProductID
    //{
    //    BOND = "f85252d1-bc58-4ac6-8b56-2e228fb0a367",
    //    SWAP = "7526c942-d034-4186-8063-c9ecdb220a10"
    //}

    public enum StatusCode
    {
        [Display(Name = "Open")]
        OPEN,
        [Display(Name = "Matched")]
        MATCHED,
        [Display(Name = "Cancelled")]
        CANCELLED
    }

    public enum InstrumentMarketType
    {
        [Display(Name = "Thailand Corporate")]
        THCORP,
        [Display(Name = "Thailand Goverment")]
        THGOV
    }
    //public enum StatusID
    //{
    //    OPEN = "9161ed18-1298-44fa-ba7d-34522cb40d66",
    //    CANCELLED = "1ccd7506-b98c-4afa-838e-24378d9b3c2e",
    //    MATCHED = "83830862-ebe4-479b-af9e-ef11b24993ce"
    //}

    public enum SourceType
    { 
        INT,
        EXT
    }

    public enum PCCFType
    { 
        FI_GOV,
        FI_CORP,
        KK_PF_BS,
        KK_RF_CF,
        BOT_IRD        
    }

    public enum FrequencyType
    { 
        [Display(Name = "Semi-Annually")]
        S = 6,	
        [Display(Name = "Annually")]
        A = 12,	
        [Display(Name = "Quarterly")]
        Q = 3,	
        [Display(Name = "Monthly")]
        M = 1,
        [Display(Name = "Weekly")]
        W = 0,
        [Display(Name = "Bullet")]
        B = 99,
        [Display(Name = "28")]
        D28 = 28,
        [Display(Name = "91")]
        D91 = 91,
        [Display(Name = "182")]
        D182 = 182	
    }

    public enum Currency
    { 
        THB,
        USD
    }

    public enum LookupFactorTables
    { 
        //DA_LOGGING,
        //DA_LOGIN_AUDIT,
        DA_TRN,
        DA_TRN_CASHFLOW,
        //DA_TRN_CASHFLOW_TEMP,
        //DA_TRN_MATCH,
        MA_BOND_MARKET,
        //MA_CONFIG_ATTRIBUTE,
        MA_COUTERPARTY,
        MA_CTPY_LIMIT,
        MA_CURRENCY,
        MA_FREQ_TYPE,
        //MA_FUNCTIONAL,
        MA_INSTRUMENT,
        MA_LIMIT,
        MA_LIMIT_PRODUCT,
        //MA_PCCF,
        //MA_PCCF_CONFIG,
        MA_PORTFOLIO,
        //MA_PROCESS_DATE,
        MA_PRODUCT,
        //MA_PROFILE_FUNCTIONAL,
        MA_SPOT_RATE,
        //MA_SPOT_RATE_TEMP,
        //MA_STATUS,
        MA_USER,
        //MA_USER_PROFILE
        MA_TEMP_CTPY_LIMIT,
        MA_COUNTRY,
        MA_COUNTRY_LIMIT
    }

    public enum LogEvent
    {
        COUNTERPARTY_AUDIT,
        USER_AUDIT,
        INSTRUMENT_AUDIT
    }

    public enum LimitLogEvent
    {
        LIMIT_AUDIT,
        COUNTRY_LIMIY_AUDIT,
        TEMP_LIMIT_AUDIT,
        TEMP_COUNTRY_LIMIT_AUDIT
    }

    public enum TBMA_PURPOSE
    {
        OUT = 0,
        OUTA = 1,
        FIN = 2,
        FINB = 3,
        FINP = 4,
        DERFW = 5,
        DERFT = 6,
        DEROP = 7,
        OTH = 8
    }

    public enum TBMA_YTYPE
    {
        YTM = 0,
        YTC = 1,
        TYP = 2,
        DM = 3
    }

    public enum TBMA_REPORTBY
    {
        Clean_Price = 0,
        Gross_Price = 1
    }
}
