using System;
using System.Collections.Generic;

using System.Text;
using System.Xml.Serialization;

namespace StockSharp.Transaq.Inner
{
    [Serializable]
    [XmlRoot("sec_position")]
    internal class SecurityPosition:ClientPosition
    {
        /// <summary>
        /// Код инструмента
        /// </summary>
        [XmlElement("secid")]
        public String SecurityID
        {
            get;
            set;
        }

        /// <summary>
        /// Наименование инструмента
        /// </summary>
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
        public int? SaldoIn
        {
            get;
            set;
        }

        /// <summary>
        /// Неснижаемый остаток
        /// </summary>
        [XmlElement("saldomin")]
        public int? SaldoMin
        {
            get;
            set;
        }

        /// <summary>
        /// Куплено
        /// </summary>
        [XmlElement("bought")]
        public int? Bought
        {
            get;
            set;
        }

        /// <summary>
        /// Продано
        /// </summary>
        [XmlElement("sold")]
        public int? Sold
        {
            get;
            set;
        }

        /// <summary>
        /// Текущее сальдо
        /// </summary>
        [XmlElement("saldo")]
        public int? Saldo
        {
            get;
            set;
        }

        /// <summary>
        /// В заявках на покупку
        /// </summary>
        [XmlElement("ordbuy")]
        public int? OrdersBuy
        {
            get;
            set;
        }

        /// <summary>
        /// В заявках на продажу
        /// </summary>
        [XmlElement("ordsell")]
        public int? OrdersSell
        {
            get;
            set;
        }


    }
}
