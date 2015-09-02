using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KK.DealMaker.Core.Data;

namespace KK.DealMaker.Core.Helper
{
    public class LimitHelper
    {
        //Need to be split by Product
        //This function can be use for Bond and IRS only
        public static string GetYearBucket(DateTime dteStart, DateTime dteEnd)
        {
            string strBucket = "";

            int intYearDiff = dteEnd.Year - dteStart.Year;

            if (dteEnd.Month > dteStart.Month)
            {
                intYearDiff += 1;
            }
            else if (dteEnd.Month >= dteStart.Month && dteEnd.Day > dteStart.Day)
            {
                intYearDiff += 1;
            }

            if (intYearDiff > 20)
            {
                strBucket = ">20";
            }
            else if (intYearDiff == 0)
            {
                strBucket = "";
            }
            else
            {
                strBucket = intYearDiff.ToString();
            }

            return strBucket;
        }

        public static string GetMonthBucket(DateTime dteStart, DateTime dteEnd)
        {
            string strBucket = "";

            int intMonthDiff = (dteEnd.Month - dteStart.Month) +  12 * (dteEnd.Year - dteStart.Year);

            if (dteEnd.Day > dteStart.Day)
            {
                intMonthDiff += 1;
            }
            
            strBucket = intMonthDiff.ToString();
            
            return strBucket;
        }

        public static decimal? GetPCCFValue(MA_PCCF pccf, string strBucket)
        {
            switch (strBucket)
            {
                case "1" :
                    return pccf.C1;
                case "2":
                    return pccf.C2;
                case "3":
                    return pccf.C3;
                case "4":
                    return pccf.C4;
                case "5":
                    return pccf.C5;
                case "6":
                    return pccf.C6;
                case "7":
                    return pccf.C7;
                case "8":
                    return pccf.C8;
                case "9":
                    return pccf.C9;
                case "10":
                    return pccf.C10;
                case "11":
                    return pccf.C11;
                case "12":
                    return pccf.C12;
                case "13":
                    return pccf.C13;
                case "14":
                    return pccf.C14;
                case "15":
                    return pccf.C15;
                case "16":
                    return pccf.C16;
                case "17":
                    return pccf.C17;
                case "18":
                    return pccf.C18;
                case "19":
                    return pccf.C19;
                case "20":
                    return pccf.C20;
                case "21":
                    return pccf.C21;
                case "22":
                    return pccf.C22;
                case "23":
                    return pccf.C23;
                case "24":
                    return pccf.C24;
                case ">20":
                    return pccf.more20;
                default :
                    return null;
            }
        }

    }
}
