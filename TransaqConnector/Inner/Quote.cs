using System;
using System.Collections.Generic;

using System.Text;
using System.Xml.Serialization;

namespace StockSharp.Transaq.Inner
{
    [Serializable]
    [XmlRoot("quote")]
    internal class TransaqQuote
    {
        [XmlAttribute("secid")]
        public String SecurityID
        {
            get;
            set;
        }

        [XmlElement("price")]
        public decimal? Price
        {
            get;
            set;
        }

        /// <summary>
        /// доходность (актуально только для облигаций)
        /// </summary>
        [XmlElement("yield")]
        public double? Yield
        {
            get;
            set;
        }

        /// <summary>
        /// количество бумаг к покупке
        /// </summary>
        [XmlElement("buy")]
        public int? Buy
        {
            get;
            set;
        }

        /// <summary>
        /// количество бумаг к продаже
        /// </summary>
        [XmlElement("sell")]
        public int? Sell
        {
            get;
            set;
        }
    }
}
