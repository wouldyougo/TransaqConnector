using System;
using System.Collections.Generic;

using System.Text;

namespace StockSharp.Transaq.Command
{
    internal class ChangePasswordCommand:TXmlCommand
    {
        public ChangePasswordCommand()
        {
            ID = "change_pass";
        }
        public String OldPassword
        {
            get;
            set;
        }

        public String NewPassword
        {
            get;
            set;
        }

        public override string ToXmlString()
        {
            return base.GetXmlBegin() + " oldpass=\"" + OldPassword + "\"" + " newpass=\"" 
                + NewPassword + "\"" + base.GetXmlEnd();
        }
    }
}
