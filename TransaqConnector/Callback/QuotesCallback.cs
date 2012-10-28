using System;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace StockSharp.Transaq.Callback
{
	using StockSharp.Transaq.Inner;
	using StockSharp.Transaq.Serialization;

	[XmlRoot("QuotesCallback")]
    internal class QuotesCallback
    {
        private static XmlSerializer<QuotesCallback> _serializer = new XmlSerializer<QuotesCallback>();

       
        [XmlArrayItem(Type=typeof(TransaqQuote),ElementName="quote")]
        [XmlArray(ElementName="quotes")]
        public TransaqQuote[] Quotes
        {
            get;
            set;
        }

        public static QuotesCallback FromXmlString(String s)
        {
            QuotesCallback result;
            s = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" + "<QuotesCallback>" +
                s + "</QuotesCallback>";
            using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(s)))
            {
                result = (QuotesCallback)_serializer.Deserialize(stream);
                //stream.Close();
                return result;
            }
        }
    }
}
