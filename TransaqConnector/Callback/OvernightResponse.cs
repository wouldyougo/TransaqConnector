using System;
using System.Text;
using System.Xml.Serialization;
using System.IO;

using StockSharp.Transaq.Serialization;

namespace StockSharp.Transaq.Callback
{
    [XmlRoot("overnight")]
   internal class OvernightResponse: Response
    {
        private static XmlSerializer<OvernightResponse> _serializer = new XmlSerializer<OvernightResponse>();

        [XmlAttribute(AttributeName="status",DataType="boolean")]
        public bool Status
        {
            get;
            set;
        }

        public static OvernightResponse FromXmlString(String s)
        {
            s = "<?xml version=\"1.0\" encoding=\"utf-8\"?>"+s ;
            using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(s)))
            {
                return (OvernightResponse)_serializer.Deserialize(stream);
            }
        }
    }
}
