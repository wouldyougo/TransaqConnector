using System;
using System.Text;
using System.Xml.Serialization;
using System.IO;

using StockSharp.Transaq.Serialization;

namespace StockSharp.Transaq.Callback
{
	using StockSharp.Transaq.Inner;

	[XmlRoot("OrdersCallback")]
    internal class OrdersCallback
    {
        private static XmlSerializer<OrdersCallback> _serializer=new XmlSerializer<OrdersCallback>();

        [XmlArray("orders")]
        [XmlArrayItem(Type=typeof(TransaqOrder),ElementName="order")]
        public TransaqOrder[] Orders
        {
            get;
            set;
        }

        public static OrdersCallback FromXmlString(String s)
        {
            OrdersCallback result;
            s = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" + "<OrdersCallback>" +
                s + "</OrdersCallback>";
            using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(s)))
            {
                result = (OrdersCallback)_serializer.Deserialize(stream);
                //stream.Close();
                return result;
            }
        }
    }
}
