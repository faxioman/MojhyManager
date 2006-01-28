using System;
using System.Collections.Generic;
using System.Text;

namespace Mojhy.Utils
{
    /// <summary>
    /// A static class with some utilities for .NET Framework
    /// </summary>
    public static class FrameworkUtils
    {
        /// <summary>
        /// Serializes a generic object.
        /// </summary>
        /// <param name="objDeserialized">The deserialized generic object.</param>
        /// <returns></returns>
        public static string SerializeObject(object objDeserialized)
        {
            System.Xml.Serialization.XmlSerializer objXmlSerializer;
            System.IO.StringWriter objStringWriter = new System.IO.StringWriter();
            string strSerializedObject;
            try
            {
                objXmlSerializer = new System.Xml.Serialization.XmlSerializer(objDeserialized.GetType());
                objXmlSerializer.Serialize(objStringWriter, objDeserialized);
                strSerializedObject = objStringWriter.ToString();
                objXmlSerializer = null;
            }
            finally
            {
                objStringWriter.Close();
                objStringWriter = null;
            }
            return strSerializedObject;
        }
        /// <summary>
        /// Deserializes a generic object.
        /// </summary>
        /// <param name="strSerialized">The string rappresenting the serialized object.</param>
        /// <param name="tpObjectType">Type of the destination object.</param>
        /// <returns></returns>
        public static object DeserializeObject(string strSerialized, Type tpObjectType)
        {
            System.IO.StringReader objStringReader;
            object objDeserialized = new object();
            System.Xml.Serialization.XmlSerializer objXmlSerializer;
            objStringReader = new System.IO.StringReader(strSerialized);
            try
            {
                objXmlSerializer = new System.Xml.Serialization.XmlSerializer(tpObjectType);
                objDeserialized = (object)objXmlSerializer.Deserialize(objStringReader);
            }
            finally
            {
                objStringReader.Close();
                objStringReader = null;
                objXmlSerializer = null;
            }
            return objDeserialized;
        }
    }
}
