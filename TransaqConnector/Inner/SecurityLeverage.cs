using System;
using System.Collections.Generic;

using System.Text;
using System.Xml.Serialization;

namespace StockSharp.Transaq.Inner
{
    [Serializable]
    [XmlRoot("security")]
    internal class SecurityLeverage
    {
        [XmlAttribute("secid")]
        public String SecurityID
        {
            get;
            set;
        }

        [XmlAttribute(AttributeName="maxbuy",DataType="double")]
        public double MaxBuy
        {
            get;
            set;
        }

        [XmlAttribute(AttributeName="maxsell",DataType="double")]
        public double MaxSell
        {
            get;
            set;
        }
    }
}
