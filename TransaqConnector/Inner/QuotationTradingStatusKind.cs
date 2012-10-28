using System;
using System.Collections.Generic;

using System.Text;

namespace StockSharp.Transaq.Inner
{
    internal enum QuotationTradingStatusKind
    {
        /// <summary>
        ///  Недоступно для торгов
        /// </summary>
        N,
         /// <summary>
         /// Период открытия
         /// </summary>
         O,
        /// <summary>
        ///  Торги закрыты
        /// </summary>
        С,
        /// <summary>
        ///  Период закрытия
        /// </summary>
        F,
        /// <summary>
        ///  Перерыв
        /// </summary>
        B,
        /// <summary>
        ///  Торговая сессия
        /// </summary>
        T
    }
}
