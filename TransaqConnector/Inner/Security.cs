using System;
using System.Collections.Generic;

using System.Text;
using System.Xml.Serialization;

namespace StockSharp.Transaq.Inner
{
    [Serializable]
    internal class TransaqSecurity
    {
        [XmlAttribute("secid")]
        public String ID
        {
            get;
            set;
        }

        [XmlAttribute("active")]
        public bool Active
        {
            get;
            set;
        }

        [XmlElement("seccode")]
        public String Code
        {
            get;
            set;
        }

        [XmlElement("market")]
        public String Market
        {
            get;
            set;
        }

        [XmlElement("shortname")]
        public String ShortName
        {
            get;
            set;
        }

        [XmlElement("doubles")]
        public int Decimals
        {
            get;
            set;
        }

        [XmlElement("minstep")]
        public decimal? MinStep
        {
            get;
            set;
        }

        [XmlElement("lotsize")]
        public int? LotSize
        {
            get;
            set;
        }

        [XmlElement("opmask")]
        public OperationMask OpMask
        {
            get;
            set;
        }

        [XmlElement("sectype")]
        public SecurityKind Type
        {
            get;
            set;
        }
      
    }
}
