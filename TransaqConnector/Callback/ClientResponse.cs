using System;
using System.Text;
using System.Xml.Serialization;
using System.IO;

using StockSharp.Transaq.Serialization;

namespace StockSharp.Transaq.Callback
{
	using StockSharp.Transaq.Inner;

	[Serializable]
    [XmlRoot("client")]
    internal class ClientResponse: Response
    {
        private static readonly XmlSerializer<ClientResponse> _serializer = new XmlSerializer<ClientResponse>();

        public ClientResponse()
        {
            Remove = false;
        }

       
        [XmlAttribute("id")]
        public String ID
        {
            get;
            set;
        }

        [XmlElement("type")]
        public ClientKind? Type
        {
            get;
            set;
        }

        [XmlElement("currency")]
        public CurrencyKind? Currency
        {
            get;
            set;
        }

        [XmlElement("ml_overnight")]
        public int? Overnight
        {
            get;
            set;
        }

        [XmlElement("ml_intraday")]
        public int? Intraday
        {
            get;
            set;
        }

        [XmlElement("ml_restrict")]
        public double? Restrict
        {
            get;
            set;
        }

        [XmlElement("ml_call")]
        public double? Call
        {
            get;
            set;
        }

        [XmlElement("ml_close")]
        public double? Close
        {
            get;
            set;
        }

        [XmlAttribute("remove")]
        public bool Remove
        {
            get;
            set;
        }

        public static ClientResponse FromXmlString(String s)
        {
             s = "<?xml version=\"1.0\" encoding=\"utf-8\"?> " + s
                ;
            MemoryStream stream=new MemoryStream(Encoding.UTF8.GetBytes(s));
            ClientResponse result = _serializer.Deserialize(stream);
            stream.Close();
            return result;
        }


        
    }

    public enum ClientKind
    {
        spot,
        margin_level,
        leverage
    }
}
