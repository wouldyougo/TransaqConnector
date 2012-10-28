using System;
using System.Runtime.Serialization;
using System.Xml;
using System.IO;

namespace StockSharp.Transaq.Serialization
{
    public abstract class XmlSerializerBase<T> : XmlObjectSerializer
    {
        protected static String NotValidRootNameString = "Wrong Type";
        protected static String NotValidMapping = "Coincide values in dictionary";

       protected abstract void ReadInnerObject(object o, XmlDictionaryReader reader);
       

        /// <summary>
        /// Reads the XML stream or document with an <see cref="T:System.Xml.XmlDictionaryReader"/> and returns the deserialized object; it also enables you to specify whether the serializer can read the data before attempting to read it.
        /// </summary>
        /// <param name="reader">An <see cref="T:System.Xml.XmlDictionaryReader"/> used to read the XML document.</param>
        /// <param name="verifyObjectName">true to check whether the enclosing XML element name and namespace correspond to the root name and root namespace; otherwise, false to skip the verification.</param>
        /// <returns>The deserialized object.</returns>
        public override object ReadObject(System.Xml.XmlDictionaryReader reader, bool verifyObjectName)
        {
            if (reader.NodeType == System.Xml.XmlNodeType.None)
            {
                reader.MoveToContent();
                reader.MoveToElement();
            }
            if (verifyObjectName & !IsStartObject(reader))
                throw new SerializationException(NotValidRootNameString);

            T result = (T)Activator.CreateInstance(typeof(T));
            reader.MoveToContent();
            reader.MoveToElement();
            reader.MoveToFirstAttribute();
            for (int i = 0; i < reader.AttributeCount; i++)
            {
                ReadInnerObject(result, reader);
                reader.MoveToNextAttribute();
            }
            reader.MoveToElement();
            if (reader.IsEmptyElement)
            {
                reader.ReadOuterXml();
                return result;
            }
            reader.ReadStartElement();
            while (reader.IsStartElement())
            {
                ReadInnerObject(result,reader);
                reader.MoveToContent();
                reader.MoveToElement();
                //reader.ReadStartElement();
            }
            reader.ReadEndElement();
            reader.MoveToContent();
            reader.MoveToElement();
            //if (reader.EOF)
            //   reader.Close();
            return result;
        }

        public T Deserialize(Stream stream)
        {
            return (T)this.ReadObject(stream);
        }

        public override void WriteEndObject(System.Xml.XmlDictionaryWriter writer)
        {
            throw new NotImplementedException();
        }

        public override void WriteObjectContent(System.Xml.XmlDictionaryWriter writer, object graph)
        {
            throw new NotImplementedException();
        }

        public override void WriteStartObject(System.Xml.XmlDictionaryWriter writer, object graph)
        {
            throw new NotImplementedException();
        }


    }
}
