using System;
using System.Text;
using System.Xml.Serialization;
using System.IO;

using StockSharp.Transaq.Serialization;

namespace StockSharp.Transaq.Callback
{
    [XmlRoot("marketord")]
    internal class MarketOrderPermitCallback
    {
        private static XmlSerializer<MarketOrderPermitCallback> _serializer =
            new XmlSerializer<MarketOrderPermitCallback>();

        [XmlAttribute("secid")]
        public String SecurityID
        {
            get;
            set;
        }

        [XmlIgnore]
        public bool Permit
        {
            get;
            set;
        }

        [XmlAttribute("permit")]
        public String PermitAsString
        {
            get
            {
                if (Permit) return "yes";
                else 
                    return "no";
            }
            set
            {
                if (value == "yes") Permit = true;
                else
                    if (value == "no") Permit = false;
                    else
                        throw new ArgumentOutOfRangeException();
            }
        }

        public static MarketOrderPermitCallback FromXmlString(String s)
        {
            s = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" + s;
            using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(s)))
            {
               return (MarketOrderPermitCallback)_serializer.Deserialize(stream);
       
            }
        }
    }

}
