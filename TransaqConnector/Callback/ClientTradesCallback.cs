using System;
using System.Text;
using System.Xml.Serialization;
using System.IO;

using StockSharp.Transaq.Serialization;

namespace StockSharp.Transaq.Callback
{
	using StockSharp.Transaq.Inner;

	[XmlRoot("ClientTradesCallback")]
   internal class ClientTradesCallback
    {

        private static XmlSerializer<ClientTradesCallback> _serializer=
            new XmlSerializer<ClientTradesCallback>();

       
        [XmlArray("trades")]
        [XmlArrayItem(ElementName="trade",Type=typeof(ClientTrade))]
        public ClientTrade[] Trades
        {
            get;
            set;
        }

        public static ClientTradesCallback FromXmlString(String s)
        {
            s = "<?xml version=\"1.0\" encoding=\"utf-8\"?> " +
                "<ClientTradesCallback>" + s +
                "</ClientTradesCallback>";
            ClientTradesCallback result;
            using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(s)))
            {

                 result= (ClientTradesCallback)_serializer.Deserialize(stream);
            }
            return result;
        }
    }
}
