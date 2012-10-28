using System;
using System.Collections.Generic;

using System.Text;
using System.Xml.Serialization;

namespace StockSharp.Transaq.Inner
{
    /// <summary>
    /// Котировка инструмента
    /// </summary>
    [XmlRoot("quotation")]
    internal class Quotation
    {
        [XmlAttribute("secid")]
        public String SecurityID
        {
            get;
            set;
        }

        /// <summary>
        /// НКД на дату торгов в расчете на одну бумагу, руб.
        /// </summary>
        [XmlElement("accruedintvalue")]
        public double? AccruedIntValue
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [XmlElement("open")]
        public double? Open
        {
            get;
            set;
        }

        /// <summary>
        /// Средневзвешенная цена
        /// </summary>
        [XmlElement("waprice")]
        public double? WapPrice
        {
            get;
            set;
        }

        /// <summary>
        /// Совокупный спрос
        /// </summary>
        [XmlElement("biddeptht")]
        public double? BidDepth
        {
            get;
            set;
        }

        /// <summary>
        /// Заявок на покупку
        /// </summary>
        [XmlElement("numbids")]
        public int? BidsNumber
        {
            get;
            set;
        }

        /// <summary>
        /// Совокупное предложение
        /// </summary>
        [XmlElement("offerdeptht")]
        public double? OfferDepth
        {
            get;
            set;
        }

        /// <summary>
        /// Лучшая котировка на покупку
        /// </summary>
        [XmlElement("bid")]
        public double? Bid
        {
            get;
            set;
        }

        /// <summary>
        /// Лучшая котировка на продажу
        /// </summary>
        [XmlElement("offer")]
        public double? Offer
        {
            get;
            set;
        }

        /// <summary>
        /// Заявок на продажу
        /// </summary>
        [XmlElement("numoffers")]
        public int? OffersNumber
        {
            get;
            set;
        }

        /// <summary>
        /// Сделок
        /// </summary>
        [XmlElement("numtrades")]
        public int? TradesNumber
        {
            get;
            set;
        }

        /// <summary>
        /// Объем совершенных сделок в лотах
        /// </summary>
        [XmlElement("voltoday")]
        public int? VolumeToday
        {
            get;
            set;
        }

        /// <summary>
        /// Общее кол-во открытых позиций(Forts)
        /// </summary>
        [XmlElement("openpositions")]
        public int? PositionsOpen
        {
            get;
            set;
        }

        /// <summary>
        /// Изменение открытых позиций(Forts)
        /// </summary>
        [XmlElement("deltapositions")]
        public int? PositionsDelta
        {
            get;
            set;
        }

        /// <summary>
        /// Цена последней сделки
        /// </summary>
        [XmlElement("last")]
        public double? Last
        {
            get;
            set;
        }

        /// <summary>
        /// Объем последней сделки, в лотах
        /// </summary>
        [XmlElement("quantity")]
        public int? Quantity
        {
            get;
            set;
        }

        
        [XmlElement("time")]
        public String TimeAsString
        {
            get
            {
                return   Time==null?String.Empty:((DateTime)Time).ToString(Formats.DateTimeProvider);
            }
            set
            {
                if (String.IsNullOrEmpty(value)) Time = null;
                else
                Time = DateTime.Parse(value, Formats.DateTimeProvider);
            }
        }

        /// <summary>
        /// Время заключения последней сделки
        /// </summary>
        [XmlIgnore]
        public DateTime? Time
        {
            get;
            set;
        }

        /// <summary>
        /// Изменение цены последней сделки по отношению
        ///к цене последней сделки предыдущего торгового
        ///дня
        /// </summary>

        [XmlElement("change")]
        public double? Change
        {
            get;
            set;
        }

        /// <summary>
        /// Цена последней сделки к оценке предыдущего дня
        /// </summary>
        [XmlElement("priceminusprevwaprice")]
        public double? PriceMinusPrevWaprice
        {
            get;
            set;
        }

        /// <summary>
        /// Объем совершенных сделок, руб
        /// </summary>
        [XmlElement("valtoday")]
        public double? VolumeTodayInCurrency
        {
            get;
            set;
        }

        /// <summary>
        /// Доходность, по цене последней сделки
        /// </summary>
        [XmlElement("yield")]
        public double? Yield
        {
            get;
            set;
        }

        /// <summary>
        /// Доходность по средневзвешенной цене
        /// </summary>
        [XmlElement("yieldatwaprice")]
        public double? YieldAtWaPrice
        {
            get;
            set;
        }

        /// <summary>
        /// Рыночная цена по результатам торгов сегодняшнего дня
        /// </summary>
        [XmlElement("marketpricetoday")]
        public double? MarketPriceToday
        {
            get;
            set;
        }

        /// <summary>
        /// Наибольшая цена спроса в течение торговой сессии
        /// </summary>
        [XmlElement("highbid")]
        public double? BidHigh
        {
            get;
            set;
        }

        /// <summary>
        /// Наименьшая цена предложения в течение торговой сессии
        /// </summary>
        [XmlElement("lowoffer")]
        public double? OfferLow
        {
            get;
            set;
        }

        /// <summary>
        /// Максимальная цена сделки
        /// </summary>
        [XmlElement("high")]
        public double? High
        {
            get;
            set;
        }

        /// <summary>
        /// Минимальная цена сделки
        /// </summary>
        [XmlElement("low")]
        public double? Low
        {
            get;
            set;
        }

        [XmlElement("closeprice")]
        public double? Close
        {
            get;
            set;
        }

        [XmlElement("closeyield")]
        public double? CloseYield
        {
            get;
            set;
        }

        [XmlElement("status")]
        public QuotationStatusKind? Status
        {
            get;
            set;
        }

        [XmlIgnore]
        public bool AreTradingOpAllowed
        {
            get
            {
                return Status == QuotationStatusKind.A;
            }
        }

        /// <summary>
        /// Состояние торговой сессии по инструменту
        /// </summary>
        [XmlElement("tradingstatus")]
        public QuotationTradingStatusKind? TradingStatus
        {
            get;
            set;
        }


    }
}
