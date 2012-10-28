using System;
using System.Collections.Generic;

using System.Text;
using System.Xml.Serialization;

namespace StockSharp.Transaq.Inner
{
    [Serializable]
    [XmlRoot("money_position")]
    internal class MoneyPosition:ClientPosition
    {
        /// <summary>
        /// Код вида средств
        /// </summary>
        [XmlElement("asset")]
        public String Asset
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

        /// <summary>
        /// Входящий остаток
        /// </summary>
        [XmlElement("saldoin")]
        public double? SaldoIn
        {
            get;
            set;
        }

        /// <summary>
        /// Куплено
        /// </summary>
        [XmlElement("bought")]
        public double? Bought
        {
            get;
            set;
        }

        /// <summary>
        /// Продано
        /// </summary>
        [XmlElement("sold")]
        public double? Sold
        {
            get;
            set;
        }

        /// <summary>
        /// Текущее сальдо
        /// </summary>
        [XmlElement("saldo")]
        public double? Saldo
        {
            get;
            set;
        }

        /// <summary>
        /// В заявках на покупку + комиссия
        /// </summary>
        [XmlElement("ordbuy")]
        public double? OrdersBuy
        {
            get;
            set;
        }

        /// <summary>
        /// В условных заявках на покупку
        /// </summary>
        [XmlElement("ordbuycond")]
        public double? CondOrdersBuy
        {
            get;
            set;
        }

        /// <summary>
        /// Сумма списанной комиссии
        /// </summary>
        [XmlElement("comission")]
        public double? Comission
        {
            get;
            set;
        }
    }
}
