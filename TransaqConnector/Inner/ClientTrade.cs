using System;
using System.Collections.Generic;

using System.Text;
using System.Xml.Serialization;

namespace StockSharp.Transaq.Inner
{
    [XmlRoot("trade")]
    internal class ClientTrade:TransaqTradeBase
    {
        [XmlElement("secid")]
        public override string SecurityId
        {
            get
            {
                return base.SecurityId;
            }
            set
            {
                base.SecurityId = value;
            }
        }
        /// <summary>
        /// Номер заявки на бирже
        /// </summary>
        [XmlElement("orderno")]
        public int OrderNumber
        {
            get;
            set;
        }
        /// <summary>
        /// Идентификатор клиента
        /// </summary>
        [XmlElement("client")]
        public String Client
        {
            get;
            set;
        }

        /// <summary>
        /// Примечание
        /// </summary>
        [XmlElement("brokerref")]
        public String Reference
        {
            get;
            set;
        }

        /// <summary>
        /// Объем сделки
        /// </summary>
        [XmlElement("value")]
        public double Value
        {
            get;
            set;
        }

        /// <summary>
        /// комиссия
        /// </summary>
        [XmlElement("comission")]
        public double? Comission
        {
            get;
            set;
        }

        /// <summary>
        /// доходность
        /// </summary>
        [XmlElement("yield")]
        public double? Yield
        {
            get;
            set;
        }

        /// <summary>
        /// НКД
        /// </summary>
        [XmlElement("accruedint")]
        public int? AccruedIntValue
        {
            get;
            set;
        }

        /// <summary>
        /// тип сделки: ‘T’ – обычная ‘N’ – РПС ‘R’ – РЕПО ‘P’ – размещение
        /// </summary>
        [XmlElement("tradetype")]
        public TradeType TradeType
        {
            get;
            set;
        }

        [XmlElement("settlecode")]
        public String SettleCode
        {
            get;
            set;
        }

        /// <summary>
        /// Текущая позиция на ФОРТС
        /// </summary>
        [XmlElement("currentpos")]
        public double? CurrentPos
        {
            get;
            set;
        }
    }
}
