namespace StockSharp.Transaq.Callback
{
	using System;
	using System.Text;
	using System.Xml.Serialization;
	using System.IO;

	using StockSharp.Transaq.Serialization;
	using StockSharp.Transaq.Inner;

	[Serializable]
    [XmlRoot("AllTradesCallback")]
    internal class AllTradesResponse: Response
    {
		private static XmlSerializer<AllTradesResponse> _serializer = new XmlSerializer<AllTradesResponse>();

        [XmlArray(ElementName="alltrades")]
        [XmlArrayItem(Type=typeof(TransaqTrade),ElementName="trade")]
        public TransaqTrade[] AllTrades
        {
            get;
            set;
        }

        public static AllTradesResponse FromXmlString(String s)
        {
            s = "<?xml version=\"1.0\" encoding=\"utf-8\"?> " +
                "<AllTradesCallback>" + s +
                "</AllTradesCallback>";
            MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(s));
			AllTradesResponse result = (AllTradesResponse)_serializer.Deserialize(stream);
            stream.Close();
            return result;
        }



    }
}
