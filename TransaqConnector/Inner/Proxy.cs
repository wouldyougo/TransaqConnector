using System;
using System.Collections.Generic;

using System.Text;

namespace StockSharp.Transaq.Inner
{
    [Serializable]
    public class Proxy
    {
        public ProxyType Type
        {
            get;
            set;
        }

        public String Address
        {
            get;
            set;
        }

        public String Port
        {
            get;
            set;
        }

        public String Login
        {
            get;
            set;
        }

        public String Password
        {
            get;
            set;
        }

        internal String ToXmlString()
        {
           return String.Format("<proxy type=\"{0}\" addr=\"{1}\""+ 
               "port=\"{2}\" login=\"{3}\" password=\"{4}\"/>",Type,Address,Port,Login,
               Password);

        }
    }

    public enum ProxyType
    {
        SOCKS4, 
        SOCKS5, 
        HTTPCONNECT
    }
}
