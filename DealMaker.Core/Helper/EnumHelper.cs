using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.Reflection;
using System.Linq.Expressions;
namespace KK.DealMaker.Core.Helper
{
    public class EnumHelper
    {
        public static IEnumerable<T> GetItems<T>()
        {
            return System.Enum.GetValues(typeof(T)).Cast<T>();
        }

        public static string GetEnumDescription<TEnum>(TEnum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if ((attributes != null) && (attributes.Length > 0))
                return attributes[0].Description;
            else
                return value.ToString();
        }

    }
}
