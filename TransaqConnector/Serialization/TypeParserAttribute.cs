using System;

namespace StockSharp.Transaq.Serialization
{
    public class TypeParserAttribute : Attribute
    {
        public TypeParserAttribute(Type t)
        {
            Type = t;
        }

        public Type Type
        {
            get;
            set;
        }

    }
}
