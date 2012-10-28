using System;

namespace StockSharp.Transaq.Serialization
{
    public struct XmlSerializationInfo
    {
        public XmlNodeType Type
        {
            get;
            set;
        }

        public bool HasName
        {
            get;
            set;
        }

        public String Name
        {
            get;
            set;
        }

      

    }
}
