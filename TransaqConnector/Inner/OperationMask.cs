using System;
using System.Collections.Generic;

using System.Text;
using System.Xml.Serialization;

namespace StockSharp.Transaq.Inner
{
    [Serializable]
    [XmlRoot("opmask")]
    internal class OperationMask
    {
        [XmlIgnore]
        public bool UseCredit
        {
            get;
            set;
        }

        [XmlAttribute("usecredit")]
        public String UseCreditAsString
        {
            get
            {
                return UseCredit ? "yes" : "no";
            }
            set
            {
                switch (value)
                {
                    case "yes": UseCredit = true; break;
                    case "no": UseCredit = false; break;
                    default: throw new ArgumentOutOfRangeException();
                }
            }
        }

        [XmlIgnore]
        public bool ByMarket
        {
            get;
            set;
        }

        [XmlAttribute("bymarket")]
        public String ByMarketAsString
        {
            get
            {
                return ByMarket ? "yes" : "no";
            }
            set
            {
                switch (value)
                {
                    case "yes": ByMarket = true; break;
                    case "no": ByMarket = false; break;
                    default: throw new ArgumentOutOfRangeException();
                }
            }
        }

        [XmlIgnore]
        public bool NoSplit
        {
            get;
            set;
        }

        [XmlAttribute("nosplit")]
        public String NoSplitAsString
        {
            get
            {
                return NoSplit ? "yes" : "no";
            }
            set
            {
                switch (value)
                {
                    case "yes": NoSplit = true; break;
                    case "no": NoSplit = false; break;
                    default: throw new ArgumentOutOfRangeException();
                }
            }
        }

        [XmlIgnore]
        public bool ImmOrCancel
        {
            get;
            set;
        }

        [XmlAttribute("immorcancel")]
        public String ImmOrCancelAsString
        {
            get
            {
                return ImmOrCancel ? "yes" : "no";
            }
            set
            {
                switch (value)
                {
                    case "yes": ImmOrCancel = true; break;
                    case "no": ImmOrCancel = false; break;
                    default: throw new ArgumentOutOfRangeException();
                }
            }
        }

        [XmlIgnore]
        public bool CancelBalance
        {
            get;
            set;
        }

        [XmlAttribute("cancelbalance")]
        public String CancelBalanceAsString
        {
            get
            {
                return CancelBalance? "yes" : "no";
            }
            set
            {
                switch (value)
                {
                    case "yes": CancelBalance = true; break;
                    case "no": CancelBalance = false; break;
                    default: throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}
