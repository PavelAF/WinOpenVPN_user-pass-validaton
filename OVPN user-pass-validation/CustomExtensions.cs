using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace WinOVPN_user_pass_validaton
{
    public static class xmlExtensions
    {
        public static XmlNode SSelectSingleNode(this XmlNode xmlNode, string path)
        {
            XmlNode result;
            try
            {
                result = xmlNode.SelectSingleNode(path);
            }
            catch
            {
                result = null;
            }
            return result;
        }
        public static XmlNode SSelectSingleNode(this XmlDocument xmlDoc, string path)
        {

            XmlNode result;
            try
            {
                result = xmlDoc.SelectSingleNode(path);
            }
            catch
            {
                result = null;
            }
            return result;
        }

        public static string SGetAttrVal(this XmlNode xmlNode, string attr)
        {

            string result;
            try
            {
                result = xmlNode.Attributes.GetNamedItem(attr).Value;
            }
            catch
            {
                result = null;
            }
            return result;
        }

        public static XmlNode SGetAttr(this XmlNode xmlNode, string attr)
        {

            XmlNode result;
            try
            {
                result = xmlNode.Attributes.GetNamedItem(attr);
            }
            catch
            {
                result = null;
            }
            return result;
        }

        public static XmlNodeList SSelectNodes(this XmlNode xmlNode, string path)
        {

            XmlNodeList result;
            try
            {
                result = xmlNode.SelectNodes(path);
            }
            catch
            {
                result = null;
            }
            return result;
        }
    }
    //SelectNodes
}
