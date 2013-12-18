using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace BaiduAppStoreCap.Service
{
    public class DataConvertService
    {
        /// <summary>
        /// 反序列化为对象
        /// </summary>
        /// <param name="xmlDoc">XmlDocument类型</param>
        /// <param name="t">对象</param>
        /// <returns></returns>
        public T Deserialize<T>(XmlDocument xmlDoc)
        {
            StringReader reader = new StringReader(xmlDoc.OuterXml);
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            T t = (T)serializer.Deserialize(reader);
            return t;
        }
        /// <summary>
        /// 反序列化为对象
        /// </summary>
        /// <param name="xmlNode">XmlDocument类型</param>
        /// <param name="t">对象</param>
        /// <returns></returns>
        public T Deserialize<T>(XmlNode xmlNode)
        {
            StringReader reader = new StringReader(xmlNode.OuterXml);
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            T t = (T)serializer.Deserialize(reader);
            return t;
        }

        public XmlDocument GetXmlDocument(string input)
        {
            var doc = new XmlDocument();
            try
            {
                doc.LoadXml(input);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message + ex.StackTrace);
            }
            return doc;
        }
    }
}
