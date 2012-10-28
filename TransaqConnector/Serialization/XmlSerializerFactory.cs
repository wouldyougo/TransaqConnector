using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace StockSharp.Transaq.Serialization
{
    public class XmlSerializerFactory : IXmlSerializerFactory
    {
        private static XmlSerializerFactory _instance = new XmlSerializerFactory();
        private Dictionary<Type, XmlObjectSerializer> _serializers = new Dictionary<Type, XmlObjectSerializer>();

        private XmlSerializerFactory() { }

        public static XmlSerializerFactory Instance
        {
            get
            {
                return _instance;
            }
        }

        public XmlObjectSerializer GetSerializer(Type t)
        {
            if (!_serializers.ContainsKey(t))
            {
                lock (this)
                {
                    if(!_serializers.ContainsKey(t))
                    {
                        _serializers.Add(t,(XmlObjectSerializer)Activator.CreateInstance( (typeof(XmlSerializer<>)).MakeGenericType(t)));
                    }
                }
            }
            return (XmlObjectSerializer)_serializers[t];
        }
    }
}
