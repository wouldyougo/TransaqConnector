using System;
using System.Collections.Generic;

using System.Text;
using System.IO;
using System.Xml;

namespace StockSharp.Transaq.Callback
{
	using StockSharp.Transaq.Inner;

	internal class MarketsResponse: Response
    {
        private List<Market> _markets = new List<Market>();

        public List<Market> Markets
        {
            get
            {
                return _markets;
            }
        }

        public static MarketsResponse FromXmlString(string s)
        {
            MarketsResponse result = new MarketsResponse();
            
            s = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" + s;
            MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(s));
            XmlReader reader = XmlReader.Create(stream);
            reader.MoveToContent();
            reader.ReadStartElement();
            String id;
            while (reader.Name == "market")
            {
                reader.MoveToFirstAttribute();
                id = reader.Value;
                reader.MoveToElement();
                result.Markets.Add(new Market(id,reader.ReadInnerXml()));
                //  reader.ReadStartElement();
            }
            reader.Close();
            stream.Close();
            return result;
        }

    }
}
