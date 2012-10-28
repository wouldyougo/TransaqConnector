using System;
using System.Collections.Generic;

using System.Text;
using System.Xml.Serialization;

namespace StockSharp.Transaq.Inner
{
    [Serializable]
    [XmlRoot("spot_limit")]
    internal class SpotLimit:ClientPosition
    {
        /// <summary>
        /// Текущий лимит
        /// </summary>
        [XmlElement("buylimit")]
        public double? BuyLimit
        {
            get;
            set;
        }

        /// <summary>
        /// Заблокировано лимита
        /// </summary>
        [XmlElement("buylimitused")]
        public double? BuyLimitUsed
        {
            get;
            set;
        }
    }
}
