using System;
using System.Runtime.Serialization;

namespace StockSharp.Transaq.Serialization
{
    public interface IXmlSerializerFactory
    {
        XmlObjectSerializer GetSerializer(Type t);
    }
}
