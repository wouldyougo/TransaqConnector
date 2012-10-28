using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Globalization;
using System.Collections;

namespace StockSharp.Transaq.Serialization
{
    public class XmlSerializer<T>  : XmlSerializerBase<T>
    {
       protected Dictionary<MemberInfo, XmlSerializationInfo> _mapping=
            new Dictionary<MemberInfo,XmlSerializationInfo>();

        protected Dictionary<XmlSerializationInfo, MemberInfo> _mappingInversely =
            new Dictionary<XmlSerializationInfo, MemberInfo>();

        protected IXmlSerializerFactory _factory;

        public XmlSerializer()
        {
            GenerateMappings();
            _factory = XmlSerializerFactory.Instance;
            ValueTypeConverter = new ValueParser(CultureInfo.InvariantCulture);
            
        }

        public TypeConverter ValueTypeConverter
        {
            get;
            set;
        }

        public IXmlSerializerFactory SerializerFactory
        {
            get
            {
                return _factory;
            }
            set
            {
                _factory = value;
            }
        }


        protected virtual void GenerateMappings()
        {
            PropertyInfo[] props=typeof(T).GetProperties(BindingFlags.NonPublic  | BindingFlags.Instance|BindingFlags.Public);
            FieldInfo[] fields=typeof(T).GetFields(BindingFlags.NonPublic  | BindingFlags.Instance|BindingFlags.Public);
            List<MemberInfo> mems = new List<MemberInfo>();
            String name;
            mems.AddRange(props);
            mems.AddRange(fields);
            object[] attribs;
            attribs = typeof(T).GetCustomAttributes(typeof(XmlRootAttribute), true);
            if (attribs != null && attribs.Length != 0)
                _mapping.Add( typeof(T),new XmlSerializationInfo()
                {
                     Type=XmlNodeType.Element,
                     HasName=true,
                     Name=((XmlRootAttribute)attribs[0]).ElementName
                });
            for (int i = 0; i < mems.Count; i++)
            {
                attribs = mems[i].GetCustomAttributes(typeof(XmlAttributeAttribute), true);
                if (attribs != null && attribs.Length != 0)
                {
                    name = ((XmlAttributeAttribute)attribs[0]).AttributeName;
                    if (String.IsNullOrEmpty(name)) name = mems[i].Name;
                    _mapping.Add(mems[i], new XmlSerializationInfo()
                    {
                        HasName = true,
                        Name = name,
                        Type = XmlNodeType.Attribute
                    });
                }
                else
                {
                    attribs = mems[i].GetCustomAttributes(typeof(XmlElementAttribute), true);
                    if (attribs != null && attribs.Length != 0)
                    {
                        name=((XmlElementAttribute)attribs[0]).ElementName;
                        if(String.IsNullOrEmpty(name)) name=mems[i].Name;
                        _mapping.Add(mems[i], new XmlSerializationInfo()
                        {
                            HasName = true,
                            Name = name,
                            Type = XmlNodeType.Element
                        });
                    }
                    else
                    {
                        attribs = mems[i].GetCustomAttributes(typeof(XmlArrayAttribute), true);

                        if (attribs != null && attribs.Length != 0)
                        {
                            name = ((XmlArrayAttribute)attribs[0]).ElementName;
                            if (String.IsNullOrEmpty(name)) name = mems[i].Name;
                            _mapping.Add(mems[i], new XmlSerializationInfo()
                            {
                                HasName = true,
                                Name = name,
                                Type = XmlNodeType.Element
                            });
                        }
                    }
                }
                
            }
            GenerateMappingsInversely();
        }

        protected virtual void assignValueToMemberInfo(MemberInfo mi,object obj,object value)
        {
            Type returnType= getReturnType(mi);
            if (returnType.GetInterface("IList") != null)
            {
                object returnList=null;
                if (mi.MemberType == MemberTypes.Property)
                    returnList=((PropertyInfo)mi).GetValue(obj, null);
                if(mi.MemberType == MemberTypes.Field)
                    returnList=((FieldInfo)mi).GetValue(obj);
                if (returnList == null)
                {
                   
                    if (returnType.IsArray)
                        returnList = Array.CreateInstance(returnType.GetElementType(), ((IList)value).Count);
                    else
                        returnList = Activator.CreateInstance(returnType);
                }
                IList assignValue = (IList)value;
                if(!((IList)returnList).IsFixedSize)
                for (int i = 0; i < assignValue.Count; i++)
                  ((IList)returnList).Add(assignValue[i]);
                else
                    for (int i = 0; i < assignValue.Count; i++)
                        ((IList)returnList)[i]=assignValue[i];
                value = returnList;
           
            }
                

            if (mi.MemberType == MemberTypes.Property)
            {
                PropertyInfo property = mi as PropertyInfo;
                if(property.CanWrite)
                property.SetValue(obj, value, null);
            }
            else if (mi.MemberType == MemberTypes.Field)
            {
                FieldInfo field = mi as FieldInfo;
                field.SetValue(obj, value);
            }
        }

        private Type getReturnType(MemberInfo mi)
        {
            if (mi.MemberType == MemberTypes.Property)
            {
                PropertyInfo property = mi as PropertyInfo;
                return property.PropertyType;
            }
            else if (mi.MemberType == MemberTypes.Field)
            {
                FieldInfo field = mi as FieldInfo;
                return field.FieldType;
            }
            else return null;
        
        }

        protected void GenerateMappingsInversely()
        {
            try
            {
                _mappingInversely.Clear();
                foreach (MemberInfo key in _mapping.Keys)
                {
                    _mappingInversely.Add(_mapping[key], key);
                }
            }
            catch (Exception ex)
            {
                throw new SerializationException(NotValidMapping, ex);
            }
        }

        protected virtual object readInnerObject(XmlDictionaryReader reader, MemberInfo mi)
        {
           Type returnType = getReturnType(mi);
           if (returnType.GetInterface("IList") != null) return readIEnumerable(reader, mi);
           return deserializeObject(reader, returnType);

        }

        protected virtual object deserializeObject(XmlDictionaryReader reader, Type t)
        {
            if (ValueTypeConverter.CanConvertTo(t)) return ValueTypeConverter.ConvertTo(reader.ReadInnerXml(), t);
            return _factory.GetSerializer(t).ReadObject(reader,false);
        }

        protected virtual object readIEnumerable(XmlDictionaryReader reader, MemberInfo mi)
        {
            object[] attribs=mi.GetCustomAttributes(typeof(XmlArrayItemAttribute), true);
            XmlArrayItemAttribute attrib;
            int i;
            Type returnType = getReturnType(mi);
            Dictionary<String,Type> rootNames = new Dictionary<string,Type>();
            Type[] genericArguments = returnType.GetGenericArguments();
            for (i = 0; i < genericArguments.Length; i++)
            {
                rootNames.Add(genericArguments[i].Name, genericArguments[i]);
            }
            if (returnType.IsArray) rootNames.Add(returnType.GetElementType().Name, returnType.GetElementType());
            for (i = 0; i < attribs.Length; i++)
            {
                attrib = (XmlArrayItemAttribute)attribs[i];
                String name;
                if (!String.IsNullOrEmpty(attrib.ElementName))
                {
                    name = attrib.ElementName;
                }
                else
                    name = attrib.Type.Name;
                if (!rootNames.ContainsKey(name))
                    rootNames.Add(name, attrib.Type);
                else
                    rootNames[name] = attrib.Type;
            }
            List<object> newElements=new List<object>();
            reader.ReadStartElement();
            reader.MoveToContent();
            reader.MoveToElement();
            while (reader.IsStartElement())
            {
                if (rootNames.ContainsKey(reader.Name))
                {
                    newElements.Add(deserializeObject(reader, rootNames[reader.Name]));
                }
                else
                {
                    reader.Skip();
                }

                reader.MoveToContent();
                reader.MoveToElement();
                //reader.ReadStartElement();
            }
            return newElements;
        }

        public override bool IsStartObject(System.Xml.XmlDictionaryReader reader)
        {
            if (_mapping.ContainsKey(typeof(T))&&_mapping[typeof(T)].HasName)
            {
                return reader.NodeType == System.Xml.XmlNodeType.Element && reader.Name == _mapping[typeof(T)].Name;
            }
            return true;
        }

        protected override void ReadInnerObject(object o, XmlDictionaryReader reader)
        {
            XmlSerializationInfo curInfo;
            if (reader.NodeType == System.Xml.XmlNodeType.Attribute)
            {
                curInfo = new XmlSerializationInfo()
                {
                    Name = reader.Name,
                    HasName = true,
                    Type = XmlNodeType.Attribute
                };
            }
            else if (reader.NodeType == System.Xml.XmlNodeType.Element)
                curInfo = new XmlSerializationInfo()
                {
                    Name = reader.Name,
                    HasName = true,
                    Type = XmlNodeType.Element
                };
            else
            {
                reader.Skip();
                return;
            }

            if (_mappingInversely.ContainsKey(curInfo))
            {
                MemberInfo curMember = _mappingInversely[curInfo];
                assignValueToMemberInfo(curMember, o, readInnerObject(reader, curMember));
            }
            else
                reader.Skip();
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
