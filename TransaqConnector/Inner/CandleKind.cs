using System;
using System.Collections.Generic;

using System.Text;
using System.Xml.Serialization;

namespace StockSharp.Transaq.Inner
{
   
    [Serializable]
    [XmlRoot("kind")]
    internal class CandleKind
    {
        [XmlElement("id")]
        public String ID
        {
            get;
            set;
        }

        [XmlElement("period")]
        public int Period
        {
            get;
            set;
        }

        [XmlElement("name")]
        public String Name
        {
            get;
            set;
        }
    }
}
