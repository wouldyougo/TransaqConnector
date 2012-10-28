using System;
using System.Text;
using System.Xml.Serialization;
using System.IO;

using StockSharp.Transaq.Serialization;

namespace StockSharp.Transaq.Callback
{
	using StockSharp.Transaq.Inner;

	[XmlRoot("leverage_control")]
    internal class LeverageControlCallback
    {
        private static XmlSerializer<LeverageControlCallback> _serializer = 
            new XmlSerializer<LeverageControlCallback>();

        [XmlAttribute("client")]
        public String Client
        {
            get;
            set;
        }

        [XmlAttribute(AttributeName="leverage_plan",DataType="double")]
        public double LeveragePlan
        {
            get;
            set;
        }

        [XmlAttribute(AttributeName="leverage_fact",DataType="double")]
        public double LeverageFact
        {
            get;
            set;
        }

        [XmlArray("Leverages")]
        [XmlArrayItem(Type=typeof(SecurityLeverage),ElementName="security")]
        public SecurityLeverage[] Leverages
        {
            get;
            set;
        }
        
        public static LeverageControlCallback FromXmlString(String s)
        {
           // s=s.Insert(s.IndexOf('>')," xmlns=\"\"");
            s=s.Insert(s.IndexOf('>') + 1, "<Leverages>");
            s=s.Insert(s.LastIndexOf("</"),"</Leverages>");
            s = "<?xml version=\"1.0\" encoding=\"utf-8\"?> " + s;
            using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(s)))
            {
                return (LeverageControlCallback)_serializer.Deserialize(stream);
                
                
            }
        }

    }
}
