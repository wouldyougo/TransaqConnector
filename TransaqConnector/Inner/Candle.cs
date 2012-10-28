using System;
using System.Collections.Generic;

using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Xml;

namespace StockSharp.Transaq.Inner
{
    [Serializable]
    [XmlRoot("candle")]
    internal class TransaqCandle
    {
        [XmlAttribute("date")]
        public DateTime Date
        {
            get;
            set;
        }

        [XmlAttribute("open")]
        public decimal Open
        {
            get;
            set;
        }

        [XmlAttribute("high")]
        public decimal High
        {
            get;
            set;
        }

        [XmlAttribute("low")]
        public decimal Low
        {
            get;
            set;
        }

        [XmlAttribute("close")]
        public decimal Close
        {
            get;
            set;
        }

        [XmlAttribute("volume")]
        public int Volume
        {
            get;
            set;
        }

        public static TransaqCandle FromXmlReader(XmlReader reader)
        {
            TransaqCandle result=new TransaqCandle();
            reader.MoveToFirstAttribute();
            if (reader.Name == "date")
            {
                result.Date = DateTime.Parse(reader.Value,Formats.DateTimeProvider);
                reader.MoveToNextAttribute();
                result.Open = decimal.Parse(reader.Value, Formats.NumericProvider);
                reader.MoveToNextAttribute();
                result.High = decimal.Parse(reader.Value, Formats.NumericProvider);
                reader.MoveToNextAttribute();
                result.Low = decimal.Parse(reader.Value, Formats.NumericProvider);
                reader.MoveToNextAttribute();
                result.Close = decimal.Parse(reader.Value, Formats.NumericProvider);
                reader.MoveToNextAttribute();
                result.Volume = int.Parse(reader.Value, Formats.NumericProvider);
                reader.MoveToElement();
               
            }
            
         
            return result;
        }
    }
}
