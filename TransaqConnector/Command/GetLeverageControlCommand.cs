using System;
using System.Collections.Generic;

using System.Text;

namespace StockSharp.Transaq.Command
{
    internal class GetLeverageControlCommand:TXmlCommand
    {
        private List<String> _securities;

        public GetLeverageControlCommand()
        {
            ID = "get_leverage_control";
            _securities = new List<string>();
        }

        public String Client
        {
            get;
            set;
        }

        public IList<String> Securities
        {
            get
            {
                return _securities;
            }
        }

        public override string ToXmlString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(String.Format("<command id=\"{0}\" client=\"{1}\">",ID,Client));
            for (int i = 0; i < Securities.Count; i++)
            {
                sb.Append("<secid>" + Securities[i] + "</secid>");
            }
            sb.Append("</command>");
            return sb.ToString();
        }
    }
}
