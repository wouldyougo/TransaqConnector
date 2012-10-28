using System.Text;
using System.IO;
using System.Xml;

namespace StockSharp.Transaq.Callback
{
    internal class ServerStatusResponse: Response
    {

        #region public properties

        public bool IsConnected { get; set; }

        public bool IsError { get; set; }

        public string ErrorMessage { get; set; }

        #endregion


        public ServerStatusResponse()
        {
            IsConnected = false;
            IsError = false;
        }


        public static ServerStatusResponse FromXmlString(string s)
        {
            s = "<?xml version=\"1.0\" encoding=\"utf-8\"?> " + s;

            ServerStatusResponse result = new ServerStatusResponse();
            MemoryStream str = new MemoryStream(Encoding.UTF8.GetBytes(s));
            XmlReader reader = XmlReader.Create(str);
            reader.MoveToContent();
            reader.MoveToFirstAttribute();
            if (reader.Value == "error")
            {
                result.IsError = true;
                reader.MoveToElement();
                result.ErrorMessage=reader.ReadInnerXml();
            }
            else if (reader.Value == "true")
                result.IsConnected = true;

            reader.Close();
            str.Close();
            
            return result;
        }

    }
}
