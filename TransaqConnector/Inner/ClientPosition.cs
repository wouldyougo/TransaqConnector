using System;
using System.Collections.Generic;

using System.Text;
using System.Xml.Serialization;

namespace StockSharp.Transaq.Inner
{
    [Serializable]
    internal class ClientPosition
    {
        /// <summary>
        /// Идентификатор клиента
        /// </summary>
        [XmlElement("client")]
        public String Client
        {
            get;
            set;
        }

    }
}
