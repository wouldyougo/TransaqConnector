using System;
using System.Collections.Generic;

using System.Text;
using System.Xml.Serialization;

namespace StockSharp.Transaq.Inner
{
    [Serializable]
    [XmlRoot("forts_position")]
    internal class FortsPosition:ClientPosition
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
        /// Входящая позиция по инструменту
        /// </summary>
        [XmlElement("startnet")]
        public double? StartNet
        {
            get;
            set;
        }

        /// <summary>
        /// В заявках на покупку
        /// </summary>
        [XmlElement("openbuys")]
        public double? OpenBuys
        {
            get;
            set;
        }

        /// <summary>
        /// В заявках на продажу
        /// </summary>
        [XmlElement("opensells")]
        public double? OpenSells
        {
            get;
            set;
        }

        /// <summary>
        /// Текущая позиция по инструменту
        /// </summary>
        [XmlElement("totalnet")]
        public double? TotalNet
        {
            get;
            set;
        }

        /// <summary>
        /// Куплено
        /// </summary>
        [XmlElement("todaybuy")]
        public double? TodayBuy
        {
            get;
            set;
        }

        /// <summary>
        /// Продано
        /// </summary>
        [XmlElement("todaysell")]
        public double? TodaySell
        {
            get;
            set;
        }

        /// <summary>
        /// Маржа для маржируемых опционов
        /// </summary>
        [XmlElement("optmargin")]
        public double? OptMargin
        {
            get;
            set;
        }

        /// <summary>
        /// Вариационная маржа
        /// </summary>
        [XmlElement("varmargin")]
        public double? VarMargin
        {
            get;
            set;
        }

        /// <summary>
        /// Опционов в заявках на исполнение
        /// </summary>
        [XmlElement("expirationpos")]
        public int? ExpirationPosition
        {
            get;
            set;
        }

        /// <summary>
        /// Объем использованого спот-лимита на продажу
        /// </summary>
        [XmlElement("usedsellspotlimit")]
        public double? UsedSellSpotLimit
        {
            get;
            set;
        }

        /// <summary>
        /// текущий спот-лимит на продажу, установленный Брокером
        /// </summary>
        [XmlElement("sellspotlimit")]
        public double? SellSpotLimit
        {
            get;
            set;
        }

        /// <summary>
        /// нетто-позиция по всем инструментам данного спота
        /// </summary>
        [XmlElement("netto")]
        public double? Netto
        {
            get;
            set;
        }

        /// <summary>
        /// коэффициент ГО для спота
        /// </summary>
        [XmlElement("kgo")]
        public double KGO
        {
            get;
            set;
        }


    }
}
