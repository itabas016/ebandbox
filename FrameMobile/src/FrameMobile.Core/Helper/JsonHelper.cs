using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Web.Script.Serialization;
using System.Xml;
using System.Xml.Serialization;

namespace FrameMobile.Core
{
    public class JsonHelper<T>
    {
        public string SerializerToJson(T t)
        {
            if (t == null) return string.Empty;
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(t);
        }

        public T DeserializeFromJson(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                return default(T);
            }
            try
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                return serializer.Deserialize<T>(json);
            }
            catch
            {
                return default(T);
            }
        }

        public string SerializerToXml(T t)
        {
            XmlSerializer serializer = new XmlSerializer(t.GetType());
            using (Stream stream = new MemoryStream())
            {
                XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
                namespaces.Add(string.Empty, string.Empty);
                serializer.Serialize(stream, t, namespaces);
                stream.Position = 0;
                StreamReader streamReader = new StreamReader(stream);
                return streamReader.ReadToEnd();
            }
        }

        public T DeserializeFromXml(string xml)
        {
            try
            {
                using (TextReader reader = new StringReader(xml))
                {
                    using (XmlTextReader xmlReader = new XmlTextReader(reader))
                    {
                        xmlReader.Normalization = false;
                        XmlSerializer serializer = new XmlSerializer(typeof(T));
                        return (T)serializer.Deserialize(xmlReader);
                    }
                }
            }
            catch
            {
                return default(T);
            }

        }

        public void SerializerToFile(T t, string path)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                using (Stream stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
                    namespaces.Add(string.Empty, string.Empty);
                    serializer.Serialize(stream, t, namespaces);
                }
            }
            catch (SecurityException exception)
            {
                throw new SecurityException(exception.Message, exception.DenySetInstance, exception.PermitOnlySetInstance, exception.Method, exception.Demanded, exception.FirstPermissionThatFailed);
            }
        }

        public T DeserializeFromFile(string path)
        {
            using (Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                using (XmlTextReader reader = new XmlTextReader(stream))
                {
                    reader.Normalization = false; //解决反序列化 \r 丢失
                    XmlSerializer serializer = new XmlSerializer(typeof(T));
                    return (T)serializer.Deserialize(reader);
                }
            }
        }
    }
}
