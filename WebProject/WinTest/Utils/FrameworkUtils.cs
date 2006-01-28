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
        /// <param name="objDeserialized">The deseriakized generic object.</param>
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
    }
}
