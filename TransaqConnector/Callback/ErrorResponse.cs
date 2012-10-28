using System;
using System.Text;
using System.Xml.Serialization;
using System.IO;

using StockSharp.Transaq.Serialization;

namespace StockSharp.Transaq.Callback
{
    [XmlRoot("ErrorCallback")]
    internal class ErrorResponse: Response
    {
        private static XmlSerializer<ErrorResponse> _serializer = new XmlSerializer<ErrorResponse>();

        [XmlElement("error")]
        public String Error
        {
            get;
            set;
        }

        public static ErrorResponse FromXmlString(String s)
        {
            s = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" + "<ErrorCallback>" + s + "</ErrorCallback>";
            
            using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(s)))
            {
                return (ErrorResponse)_serializer.Deserialize(stream);
            }
        }
    }
}
