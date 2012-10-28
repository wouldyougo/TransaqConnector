using System;
using System.Collections.Generic;

using System.Text;
using System.Xml.Serialization;

namespace StockSharp.Transaq.Inner
{
    [Serializable]
    [XmlRoot("trade")]
    internal class TransaqTrade:TransaqTradeBase
    {
        [XmlAttribute("secid")]
        public override string SecurityId
        {
            get
            {
                return base.SecurityId;
            }
            set
            {
                base.SecurityId = value;
            }
        }
        
        [XmlElement("period")]
        public PeriodKind Period
        {
            get;
            set;
        }

        
    }
}
