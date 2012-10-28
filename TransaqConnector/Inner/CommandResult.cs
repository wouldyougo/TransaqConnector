using System;
using System.Text;
using System.Xml.Serialization;
using System.IO;

using StockSharp.Transaq.Serialization;

namespace StockSharp.Transaq.Inner
{
    [XmlRoot(ElementName = "result")]
    internal class CommandResult 
    {
        public CommandResult() { }

        private static XmlSerializer<CommandResult> _serializer;

        protected static XmlSerializer<CommandResult> Serializer
        {
            get
            {
                if (_serializer == null)
                {
                    _serializer = new XmlSerializer<CommandResult>();
                }
                return _serializer;
            }
        }

        [XmlAttribute("success")]
       
        //[DefaultValueAttribute(null)]
        public bool Success
        {
            get;
            set;
        }

        [XmlAttribute("transactionid")]
        public String TransactionId
        {
            get;
            set;
        }

        [XmlElement("message")]
        public String Message
        {
            get;
            set;
        }

        //[DefaultValue(null)]
        //[XmlAttribute("diff")]
        [XmlIgnore]
        public int? Difference
        {
            get;
            set;
        }

        [XmlAttribute("diff")]
        public String DifferenceAsString
        {
            get
            {
                if (Difference == null) return null;
                else
                    return ((int)Difference).ToString();
            }
            set
            {
                if (value == null) Difference = null;
                else
                    Difference = int.Parse(value);
            }

        }

        public static CommandResult FromXmlString(string s)
        {
            if (s.StartsWith("<error>"))
            {
                s = s.Replace("<error>", "<message>");
                s = s.Replace("</error>", "</message>");
                s = "<result success=\"false\">" + s + "</result>";
            }
            s = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" + s;

            using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(s)))
            {
                
                CommandResult result = Serializer.Deserialize(stream);
                #warning Проверить замену. После проверки убрать класс сериализатора
                //XmlSerializer serializer = new XmlSerializer(typeof(CommandResult));
                //CommandResult result = (CommandResult)serializer.Deserialize(stream);
                return result;
            }
            
        }

     
    }
}
