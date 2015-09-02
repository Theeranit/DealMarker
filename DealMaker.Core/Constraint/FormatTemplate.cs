using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KK.DealMaker.Core.Constraint
{
    /// <summary>
    /// Summary description for format template
    /// </summary>
    public class FormatTemplate
    {
        #region Constants

        public const string SINGLE_DATA_VALUE = "{0}";
        public const string DATETIME_LABEL = "MM/dd/yyyy";
        public const string DATE_DMY_LABEL = "dd/MM/yyyy";
        public const string DATEANDTIME_DB_LABEL = "MM/dd/yyyy HH:mm";
        public const string DATEANDTIME_DB_FULL_LABEL = "MM/dd/yyyy HH:mm:ss.mmm";
        public const string DATEANDTIME_TH_LABEL = "dd/MM/yyyy HH:mm";
        public const string TIMESTAMP_LABEL = "HH:mm";
        public const string TIMESTAMP_FULL_LABEL = "HH:mm:ss";
        public const string TIMESTAMP_24_LABEL = "HH:mm tt";
        public const string CULTURE_LABEL = "en-AU";
        public const string CULTURE_EN_LABEL = "en-US";
        public const string CULTURE_THAI_LABEL = "th-TH";
        public const string SHORT_YEAR = "yy";

        public const string EMPLOYEE_LABEL = "{0} - {1}";

        public const string SQL_ERROR_NO = "sqlerrorno:[{0}] ";

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FormatTemplate"/> class.
        /// </summary>
        public FormatTemplate()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        #endregion Constructors
    }
}
