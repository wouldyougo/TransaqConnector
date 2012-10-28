using System;
using System.Collections.Generic;

using System.Text;
using System.Xml.Serialization;

namespace StockSharp.Transaq.Inner
{
    [Serializable]
    [XmlRoot("forts_collaterals")]
    internal class FortsCollaterals:ClientPosition
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


    }
}
