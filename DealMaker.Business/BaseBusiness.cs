using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using KK.DealMaker.Core.Constraint;
using KK.DealMaker.Core.SystemFramework;
using KK.DealMaker.Core.Helper;
using KK.DealMaker.Core.Oracle;
using KK.DealMaker.Core.Data;

using System.Data;
using System.IO;
using System.Reflection;
using System.Collections;
using System.Net;
using System.Net.Sockets;

namespace KK.DealMaker.Business
{
    public class BaseBusiness
    {
        public static string Owner = ConfigurationManager.AppSettings[AppSettingName.OPICS_OWNER].ToString();
        public static string OracleConnectionString = ConfigurationManager.ConnectionStrings[AppSettingName.ORACLE_CONNECTION_STRING].ToString();
            
        public string GetCurrentDateTime()
        {
            return DateTime.Now.ToString(FormatTemplate.DATE_DMY_LABEL);
        }

        public BusinessWorkflowsException CreateException(Exception ex, string message)
        {
            return String.IsNullOrEmpty(message) ? new BusinessWorkflowsException(ex) : new BusinessWorkflowsException(ex, message);
        }

        protected List<T> MapDataReaderToList<T>(IDataReader dr)
        {
            List<T> list = new List<T>();
            T obj = default(T);
            while (dr.Read())
            {
                obj = Activator.CreateInstance<T>();
                foreach (PropertyInfo prop in obj.GetType().GetProperties())
                {
                    if (!object.Equals(dr[prop.Name], DBNull.Value))
                    {
                        if (prop.PropertyType == typeof(decimal))
                            prop.SetValue(obj, decimal.Parse(dr[prop.Name].ToString()), null);
                        else if (prop.PropertyType == typeof(int))
                            prop.SetValue(obj, int.Parse(dr[prop.Name].ToString()), null);
                        else if (prop.PropertyType == typeof(float))
                            prop.SetValue(obj, float.Parse(dr[prop.Name].ToString()), null);
                        else if (prop.PropertyType == typeof(DateTime))
                            prop.SetValue(obj, Convert.ToDateTime(dr[prop.Name].ToString()), null);
                        else 
                            prop.SetValue(obj, dr[prop.Name], null);
                    }
                }
                list.Add(obj);
            }
            return list;
        }

        protected List<T> MapDataToBusinessEntityCollection<T>(IDataReader dr) where T : new()
        {
            Type businessEntityType = typeof(T);
            List<T> entitys = new List<T>();
            Hashtable hashtable = new Hashtable();
            PropertyInfo[] properties = businessEntityType.GetProperties();
            foreach (PropertyInfo info in properties)
            {
                hashtable[info.Name.ToUpper()] = info;
            }
            while (dr.Read())
            {
                T newObject = new T();
                for (int index = 0; index < dr.FieldCount; index++)
                {
                    PropertyInfo info = (PropertyInfo)
                                        hashtable[dr.GetName(index).ToUpper()];
                    if ((info != null) && info.CanWrite)
                    {
                        info.SetValue(newObject, dr.GetValue(index), null);
                    }
                }
                entitys.Add(newObject);
            }
            dr.Close();
            return entitys;
        }

        protected IPAddress LocalIPAddress()
        {
            if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
            {
                return null;
            }

            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());

            return host
                .AddressList
                .FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);
        }
               
    }
}
