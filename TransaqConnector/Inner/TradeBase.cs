using System;
using System.Collections.Generic;

using System.Text;
using System.Xml.Serialization;

namespace StockSharp.Transaq.Inner
{
	using StockSharp.Transaq.Command;

	[Serializable]
    internal class TransaqTradeBase
    {
        [XmlIgnore]
        public virtual String SecurityId
        {
            get;
            set;
        }

      /// <summary>
        /// Номер сделки на бирже
      /// </summary>
        [XmlElement("tradeno")]
        public int Number
        {
            get;
            set;
        }

        [XmlElement("time")]
        public String TimeAsString
        {
            get
            {
                return Time.ToString(Formats.DateTimeFormat);
            }
            set
            {
                Time = DateTime.Parse(value, Formats.DateTimeProvider);
            }
        }

        [XmlElement("board")]
        public String Board
        {
            get;
            set;
        }

        [XmlIgnore]
        public DateTime Time
        {
            get;
            set;
        }

        [XmlElement("price")]
        public decimal Price
        {
            get;
            set;
        }

        [XmlElement("quantity")]
        public int Quantity
        {
            get;
            set;
        }



        [XmlElement("buysell")]
        public TradeOperation Operation
        {
            get;
            set;

        }
    }
}
