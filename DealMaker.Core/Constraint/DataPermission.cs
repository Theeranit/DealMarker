using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KK.DealMaker.Core.Constraint
{
    /// <summary>
    /// Summary description for data premission
    /// </summary>
    public class DataPermission
    {
        /// <summary>
        /// Type Of Permission is readable
        /// </summary>
        /// <history>
        /// </history>
        public const string PERMISSION_READABLE = "R";
        /// <summary>
        /// Type Of Permission is readable and writable
        /// </summary>
        /// <history>
        /// </history>
        public const string PERMISSION_READABLE_WRITABLE = "RW";
        /// <summary>
        /// Type Of Permission is approvable
        /// </summary>
        /// <history>
        /// </history>
        public const string PERMISSION_APPROVABLE = "X";

    }
}
