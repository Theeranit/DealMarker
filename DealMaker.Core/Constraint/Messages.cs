using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KK.DealMaker.Core.Constraint
{
    public class Messages
    {
        public const string DUPLICATE_KEY = "เลขที่นี้มีอยู่ในระบบแล้ว";
        public const string DUPLICATE_DATA = "ข้อมูลนี้มีอยู่ในระบบแล้ว";
        public const string DUPLICATE_DEFAULT_DATA = "Cannot set default value for more than 1 entry";
        
        public const string DATA_NOT_FOUND = "ไม่พบข้อมูลในระบบ";

        public const string SAVED_SUCCESSFULL = "บันทึกข้อมูลเรียบร้อยแล้ว";
        public const string DELETED_SUCCESSFULL = "ลบข้อมูลเรียบร้อยแล้ว";

        public const string USER_NOT_UNAVAILABLE = "This user is not unavailable to access the system, please contact your server administrator to provide this problem as 1900.";
    }
}
