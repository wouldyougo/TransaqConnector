using System;
using System.Collections.Generic;

using System.Text;

namespace StockSharp.Transaq.Command
{
	using StockSharp.Transaq.Inner;

	internal class ConnectCommand : TXmlCommand
    {
        public ConnectCommand()
            : base()
        {
            ID = "connect";
            AutoPos = false;
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

        public String Host
        {
            get;
            set;
        }

        public String Port
        {
            get;
            set;
        }

        public String LogsDir
        {
            get;
            set;
        }

        /// <summary>
        /// loglevel может принимать значения 1, 2 и 3 в зависимости от 
        /// требуемого уровня детализации логов. Значение 3 соответствует
        /// наивысшей степени детализации. Loglevel является не обязательным параметром,
        /// если он не указан, логирование не осуществляется
        /// </summary>
        public int? LogLevel
        {
            get;
            set;
        }
        /// <summary>
        /// autopos указывает на необходимость автоматического запроса клиентских позиций 
        /// на срочном рынке после каждой клиентской сделки. 
        /// Если autopos не указан, по умолчанию он принимается равным true. 
        /// Задание false при активной торговле  ускоряет взаимодействие с сервером.
        /// </summary>
        public bool AutoPos
        {
            get;
            set;
        }

        /// <summary>
        /// Задает имя файла, в котором будут храниться примечания к заявкам. 
        /// Если не указать имя файла, по умолчанию будет использоваться файл notes.xml. 
        /// Примечания хранятся в xml-формате.
        /// </summary>
        public String NotesFile
        {
            get;
            set;
        }

        public Proxy Proxy
        {
            get;
            set;
        }

        public override string ToXmlString()
        {
            StringBuilder result = new StringBuilder();
            result.Append(base.GetXmlBegin());
            result.Append(String.Format("<login>{0}</login>", Login));
            result.Append(String.Format("<password>{0}</password>", Password));
            result.Append(String.Format("<host>{0}</host>", Host));
            result.Append(String.Format("<port>{0}</port>", Port));
            result.Append(String.Format("<logsdir>{0}</logsdir>", LogsDir));
            if (LogLevel != null) result.Append(String.Format("<loglevel>{0}</loglevel>", LogLevel));
            result.Append(String.Format("<autopos>{0}</autopos>", AutoPos));
            if (NotesFile != null) result.Append(String.Format("<notes_file>{0}</notes_file>", NotesFile));
            if (Proxy != null) result.Append(Proxy.ToXmlString());
            result.Append(base.GetXmlEnd());
            return result.ToString();
        }
    }
}
