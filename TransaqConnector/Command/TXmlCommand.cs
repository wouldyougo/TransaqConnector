using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;

namespace StockSharp.Transaq.Command
{
	using StockSharp.Transaq.Inner;

	[Serializable]
    internal class TXmlCommand
    {
        public static readonly String DateTimeFormat =Formats.DateTimeFormat;

        protected String ID
        {
            get;
            set;
        }

        protected String GetXmlBegin()
        {
            String result = String.Empty;
            result = String.Format("<command id=\"{0}\">", ID);
            return result;
        }

        protected String GetXmlEnd()
        {
            return "</command>";
        }

        public virtual String ToXmlString()
        {
            return String.Format("<command id=\"{0}\"/>",ID);
        }

    }
}
