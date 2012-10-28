using System;
using System.Text;
using System.Xml.Serialization;
using System.IO;

using StockSharp.Transaq.Serialization;

namespace StockSharp.Transaq.Callback
{
	using StockSharp.Transaq.Inner;

	[XmlRoot("QuotationsCallback")]
   internal class QuotationsCallback
    {
        private static XmlSerializer<QuotationsCallback> _serializer = 
            new XmlSerializer<QuotationsCallback>();
       

        [XmlArray(ElementName="quotations")]
        [XmlArrayItem(Type = typeof(Quotation), ElementName = "quotation")]
        public Quotation[] Quotations
        {
            get;
            set;
        }

        public static QuotationsCallback FromXmlString(String s)
        {
            QuotationsCallback result;
            s = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" + "<QuotationsCallback>" +
                s + "</QuotationsCallback>";
            using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(s)))
            {
                result = (QuotationsCallback)_serializer.Deserialize(stream);
                //stream.Close();
                return result;
            }
        }
    }

   
}
