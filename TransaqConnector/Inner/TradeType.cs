using System;
using System.Collections.Generic;

using System.Text;

namespace StockSharp.Transaq.Inner
{
    internal enum TradeType
    {
        /// <summary>
        /// обычная
        /// </summary>
        T,
        /// <summary>
        /// РПС
        /// </summary>
        N,
        /// <summary>
        /// Репо
        /// </summary>
        R,
        /// <summary>
        /// Pазмещение
        /// </summary>
        P
    }
}
