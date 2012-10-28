using System;
using System.Text;
using System.Xml.Serialization;
using System.IO;

using StockSharp.Transaq.Serialization;

namespace StockSharp.Transaq.Callback
{
	using StockSharp.Transaq.Inner;

	[Serializable]
    [XmlRoot("CandleKindsCallback")]
    internal class CandleKindsResponse: Response
    {
        private static XmlSerializer<CandleKindsResponse> _serializer = new XmlSerializer<CandleKindsResponse>();

        [XmlArray(ElementName = "candlekinds", IsNullable = true)]
        [XmlArrayItem(ElementName = "kind", Type = typeof(CandleKind))]
        public CandleKind[] CandleKinds{get; set;}


        public static CandleKindsResponse FromXmlString(String s)
        {
            s = "<?xml version=\"1.0\" encoding=\"utf-8\"?> " +
                "<CandleKindsCallback>" + s +
                "</CandleKindsCallback>";
            MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(s));
            CandleKindsResponse result = (CandleKindsResponse)_serializer.Deserialize(stream);
            stream.Close();
            return result;
        }

       
    }
}
