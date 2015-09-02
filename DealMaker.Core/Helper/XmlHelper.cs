using System;
using System.Collections.Generic;

using System.Text;
using System.Reflection;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Xml;
using System.Diagnostics;
using System.Web;

namespace KK.DealMaker.Core.Helper
{
    public class Factor
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public List<Factor> Childs { get; set; }
        public Factor()
        {
            this.Childs = new List<Factor>();
        }
    }


    public class XmlHelper
    {
        private XmlDocument xmlDoc = new XmlDocument();

        public static List<Factor> GetFactors()
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(HttpContext.Current.Server.MapPath(@"..\\Xml\\FactorProp.xml"));
            List<Factor> factors = new List<Factor>();
            Factor factor = null;
            Factor childFactor = null;
            XmlNodeList nodes = xDoc.SelectNodes("//ListOfFactor//Factor");
            for (int i = 0; i < nodes.Count; i++)
            {

                //Type objectType = GetType(xDoc.DocumentElement.Attributes["ObjectType"].Value);
                var childs = GetChild(nodes[i]);
                factor = new Factor();
                factor.Name = childs[0].InnerText;

                factor.Value = childs[0].SelectSingleNode("@PropertyName").Value;
                if (childs.Count > 0 && childs[1].Attributes[0].Value == "Childs")
                {
                    var items = GetChild(childs[1].SelectSingleNode("Array"));
                    foreach (XmlElement e in items)
                    { 
                        childFactor = new Factor();
                        childFactor.Name = e.SelectSingleNode("Property").InnerText;
                        childFactor.Value = e.SelectSingleNode("Property//@PropertyName").Value;
                        factor.Childs.Add(childFactor);
                    }

                }
                factors.Add(factor);
            }

            return factors;
        }

        private static List<XmlElement> GetChild(XmlNode parent)
        {
            List<XmlElement> list = new List<XmlElement>();
            foreach (XmlNode item in parent.ChildNodes)
            {
                if (item is XmlElement)
                    list.Add(item as XmlElement);
            }
            return list;
        }
       

    }


}
