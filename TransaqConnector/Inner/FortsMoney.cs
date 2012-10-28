using System;
using System.Collections.Generic;

using System.Text;
using System.Xml.Serialization;

namespace StockSharp.Transaq.Inner
{
    [Serializable]
    [XmlRoot("forts_money")]
    internal class FortsMoney:ClientPosition
    {
        
        [XmlElement("current")]
        public double? Current
        {
            get;
            set;
        }

        [XmlElement("blocked")]
        public double? Blocked
        {
            get;
            set;
        }

        [XmlElement("free")]
        public double? Free
        {
            get;
            set;
        }

        [XmlElement("varmargin")]
        public double? VarMargin
        {
            get;
            set;
        }


    }
}
