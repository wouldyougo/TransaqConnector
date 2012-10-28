using System;
using System.Collections.Generic;

using System.Text;
using System.Xml.Serialization;

namespace StockSharp.Transaq.Inner
{
	using StockSharp.Transaq.Command;

	[Serializable]
    internal class TransaqOrder
    {
        [XmlAttribute("transactionid")]
        public String TransactionId
        {
            get;
            set;
        }

        [XmlElement("orderno")]
        public long? Number
        {
            get;
            set;
        }

        [XmlElement("secid")]
        public String SecurityID
        {
            get;
            set;
        }

        [XmlElement("board")]
        public String Board
        {
            get;
            set;
        }

        [XmlElement("client")]
        public String Client
        {
            get;
            set;
        }

        [XmlElement("status")]
        public String StatusAsString
        {
            get
            {
                return Status.ToString().ToLower();
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                   Status=(OrderStatusKind)Enum.Parse(typeof(OrderStatusKind), value, true);
                else
                    Status = null;
            }
        }

        [XmlIgnore]
        public OrderStatusKind? Status
        {
            get;
            set;
        }

        [XmlElement("buysell")]
        public TradeOperation? Operation
        {
            get;
            set;
        }

        [XmlElement("time")]
        public string TimeAsString
        {
            get
            {
                if (Time == null) return null;
                else
                    return ((DateTime)Time).ToString(Formats.DateTimeFormat);
            }
            set
            {
                if (String.IsNullOrEmpty(value)) Time = null;
                else
                    Time = DateTime.Parse(value, Formats.DateTimeProvider);
            }
            
        }

        /// <summary>
        /// Время регистрации заявки биржей
        /// </summary>
        [XmlIgnore]
        public DateTime? Time
        {
            get;
            set;
        }

        [XmlElement("accepttime")]
        public string AcceptTimeAsString
        {
            get
            {
                if (AcceptTime == null) return null;
                else
                    return ((DateTime)AcceptTime).ToString(Formats.DateTimeFormat);
            }
            set
            {
                if (String.IsNullOrEmpty(value)) AcceptTime = null;
                else
                    AcceptTime = DateTime.Parse(value, Formats.DateTimeProvider);
            }

        }

        /// <summary>
        /// Время регистрации заявки сервером Transaq(только для условных заявок)
        /// </summary>
        [XmlIgnore]
        public DateTime? AcceptTime
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
        /// Объем заявки в копейках
        /// </summary>
        [XmlElement("value")]
        public decimal? Value
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
        /// Код поставки (значение биржи, определяющее правила расчетов - смотрите подробнее в документах биржи)
        /// </summary>
        [XmlElement("settlecode")]
        public String SettleCode
        {
            get;
            set;
        }

        /// <summary>
        /// Неудовлетворенный остаток объема заявки в лотах (контрактах)
        /// </summary>
        [XmlElement("balance")]
        public int? Balance
        {
            get;
            set;
        }

        [XmlElement("price")]
        public decimal? Price
        {
            get;
            set;
        }

        [XmlElement("quantity")]
        public int? Quantity
        {
            get;
            set;
        }

        [XmlElement("yield")]
        public decimal? Yield
        {
            get;
            set;
        }

        [XmlElement("withdrawtime")]
        public String WithDrawTimeAsString
        {
            get;
            set;
        }

        /// <summary>
        /// Время снятия заявки
        /// </summary>
        [XmlIgnore]
        public DateTime? WithDrawTime
        {
            get
            {
                if (WithDrawTimeAsString != "0" && !String.IsNullOrEmpty(WithDrawTimeAsString))
                    return DateTime.Parse(WithDrawTimeAsString, Formats.DateTimeProvider);
                else
                    return null;
            }
            
        }

        [XmlElement("condition")]
        public TransaqStopConditionKinds? Condition
        {
            get;
            set;
        }


        [XmlElement("conditionvalue")]
        public String ConditionValue
        {
            get;
            set;
        }

        [XmlElement("validafter")]
        public String ValidAfter
        {
            get;
            set;
        }


        [XmlElement("validbefore")]
        public String ValidBefore
        {
            get;
            set;
        }

        [XmlIgnore]
        public DateTime? ValidAfterAsDateTime
        {
            get
            {
                if (ValidAfter != "0" && !String.IsNullOrEmpty(ValidAfter))
                    return DateTime.Parse(ValidAfter, Formats.DateTimeProvider);
                else
                    return null;
            }
        }

        public double? ConditionValueAsDouble
        {
            get
            {
                double price;
                if (double.TryParse(ConditionValue, System.Globalization.NumberStyles.Float, Formats.NumericProvider, out price))
                    return price;
                else
                    return null;
            }
        }

        public DateTime? ConditionValueAsDateTime
        {
            get
            {
                DateTime dt;
                if (DateTime.TryParse(ConditionValue, Formats.DateTimeProvider, System.Globalization.DateTimeStyles.AssumeLocal, out dt))
                    return dt;
                else
                    return null;

                
            }
        }

        [XmlIgnore]
        public DateTime? ValidBeforeAsDateTime
        {
            get
            {
                if (ValidBefore != "0" && !String.IsNullOrEmpty(ValidBefore))
                    return DateTime.Parse(ValidBefore, Formats.DateTimeProvider);
                else
                    return null;
            }
        }

        [XmlIgnore]
        public bool? IsValidTillCancelled
        {
            get
            {
                if(String.IsNullOrEmpty(ValidBefore)) return null;
                else
                    if(ValidBefore=="till_canceled") return true;
                    else
                        return false;
             }
        }

        [XmlElement("maxcomission")]
        public double? MaxComission
        {
            get;
            set;
        }

        [XmlElement("result")]
        public String Result
        {
            get;
            set;
        }
    }

    public enum OrderStatusKind
    {
        None,
        /// <summary>
        /// Активная
        /// </summary>
        Active,
        /// <summary>
        ///  Снята трейдером (заявка уже попала на рынок и была отменена)
        /// </summary>
        Cancelled,
        /// <summary>
        ///  Отклонена Брокером
        /// </summary>
        Denied,
        /// <summary>
        ///Прекращена трейдером (условная заявка, которую сняли до наступления условия)
        /// </summary>
        Disabled,
        /// <summary>
        ///  Время действия истекло
        /// </summary>
        Expired,
        /// <summary>
        ///  Не удалось выставить на биржу
        /// </summary>
        Failed,
        /// <summary>
        /// Выставляется на биржу 
        /// </summary>
        Forwarding,
        /// <summary>
        ///  Исполнена
        /// </summary>
        Matched,
        /// <summary>
        ///  Отклонена контрагентом
        /// </summary>
        Refused,
        /// <summary>
        /// Отклонена биржей 
        /// </summary>
        Rejected,
        /// <summary>
        /// Аннулирована биржей
        /// </summary>
        Removed,
        /// <summary>
        ///  Не наступило время активации
        /// </summary>
        Wait,
        /// <summary>
        ///  Ожидает наступления условия
        /// </summary>
        Watching
    }
}
