using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Xml;

namespace StockSharp.Transaq.Callback
{
	using StockSharp.Transaq.Inner;

	//   [XmlRoot("candles")]
    [Serializable]
    internal class CandlesCallback 
    {
        private List<TransaqCandle> _candles;

        public CandlesCallback()
        {
            _candles = new List<TransaqCandle>();
        }


        [XmlAttribute("secid")]
        public String SecurityID
        {
            get;
            set;
        }


        [XmlAttribute("period")]
        public String PeriodID
        {
            get;
            set;
        }

        [XmlAttribute("status")]
        public String Status
        {
            get;
            set;
        }

        [XmlIgnore]
        public List<TransaqCandle> Candles
        {
            get
            {
                return _candles;
            }
        

        }


        public static CandlesCallback FromXmlString(String s)
        {
            s = "<?xml version=\"1.0\" encoding=\"utf-8\"?> " + s
                ;
            MemoryStream str = new MemoryStream(Encoding.UTF8.GetBytes(s));
            XmlReader reader = XmlReader.Create(str);
            CandlesCallback result = new CandlesCallback();
            if (reader.MoveToContent() == XmlNodeType.Element)
            {
                if (reader.MoveToFirstAttribute())
                {
                    if (reader.Name == "secid") result.SecurityID = reader.Value;
                    if (reader.MoveToNextAttribute())
                    {
                        if (reader.Name == "period") result.PeriodID = reader.Value;
                        if (reader.MoveToNextAttribute())
                            if (reader.Name == "status") result.Status = reader.Value;
                    }
                }
                reader.ReadStartElement();
                TransaqCandle candle;
                while (reader.Name == "candle")
                {
                    candle = TransaqCandle.FromXmlReader(reader);
                    result.Candles.Add(candle);
                    reader.ReadStartElement();
                }
            }
            reader.Close();
            str.Close();
            return result;
        }

    }
}
