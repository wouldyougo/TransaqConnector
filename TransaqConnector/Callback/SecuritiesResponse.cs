using System;
using System.Text;
using System.IO;
using System.Xml.Serialization;

using StockSharp.Transaq.Serialization;

namespace StockSharp.Transaq.Callback
{
	using StockSharp.Transaq.Inner;

	[XmlRoot("SecuritiesCallback")]
    internal class SecuritiesResponse: Response
    {
        private static XmlSerializer<SecuritiesResponse> _serializer = new XmlSerializer<SecuritiesResponse>();

        [XmlArray("securities")]
        [XmlArrayItem("security", Type = typeof(TransaqSecurity))]
        public TransaqSecurity[] Securities{get; set;}


        public static SecuritiesResponse FromXmlString(String s)
        {
            SecuritiesResponse result;
            s = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" + "<SecuritiesCallback>"+
                s + "</SecuritiesCallback>";
            MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(s));
            result = (SecuritiesResponse)_serializer.Deserialize(stream);
            stream.Close();
            return result;
        }
    }
}
