using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;

using StockSharp.Transaq.Serialization;

namespace StockSharp.Transaq.Callback
{
	using StockSharp.Transaq.Inner;

	[XmlRoot("PositionsCallback")]
    internal class PositionsResponse: Response
    {
        private static XmlSerializer<PositionsResponse> _serializer = new XmlSerializer<PositionsResponse>();
        private ClientPosition[] _positions;

        public PositionsResponse() 
        {
            MoneyPositions = new List<MoneyPosition>();
            this.FortsCollaterals = new List<FortsCollaterals>();
            this.FortsMoney = new List<FortsMoney>();
            this.FortsPositions = new List<FortsPosition>();
            this.SecurityPositions = new List<SecurityPosition>();
            this.SpotLimits = new List<SpotLimit>();
        }


        [XmlArray(ElementName = "positions")]
        [XmlArrayItem(ElementName = "money_position", Type = typeof(MoneyPosition))]
        [XmlArrayItem(ElementName = "sec_position", Type = typeof(SecurityPosition))]
        [XmlArrayItem(ElementName = "forts_position", Type = typeof(FortsPosition))]
        [XmlArrayItem(ElementName = "forts_money", Type = typeof(FortsMoney))]
        [XmlArrayItem(ElementName = "forts_collaterals", Type = typeof(FortsCollaterals))]
        [XmlArrayItem(ElementName = "spot_limit", Type = typeof(SpotLimit))]
        public ClientPosition[] Positions
        {
            get
            {
                return _positions;
            }
            set
            {
                _positions = value;
                OnSetPositions();
            }
        }

        [XmlIgnore]
        public List<MoneyPosition> MoneyPositions
        {
            get;
            private set;
        }

        [XmlIgnore]
        public List<SecurityPosition> SecurityPositions
        {
            get;
            private set;
        }

        [XmlIgnore]
        public List<FortsPosition> FortsPositions
        {
            get;
            private set;
        }

        [XmlIgnore]
        public List<FortsMoney> FortsMoney
        {
            get;
            private set;
        }

        [XmlIgnore]
        public List<FortsCollaterals> FortsCollaterals
        {
            get;
            private set;
        }

        [XmlIgnore]
        public List<SpotLimit> SpotLimits
        {
            get;
            private set;
        }


        private void OnSetPositions()
        {
            MoneyPositions.Clear();
            this.FortsCollaterals.Clear();
            this.FortsMoney.Clear();
            this.FortsPositions.Clear();
            this.SecurityPositions.Clear();
            this.SpotLimits.Clear();

            foreach (ClientPosition position in _positions)
            {
                if (position is MoneyPosition)
                    MoneyPositions.Add(position as MoneyPosition);
                else
                    if (position is SecurityPosition)
                        SecurityPositions.Add(position as SecurityPosition);
                    else
                        if (position is FortsPosition)
                            FortsPositions.Add(position as FortsPosition);
                        else
                            if (position is FortsCollaterals)
                                FortsCollaterals.Add(position as FortsCollaterals);
                            else
                                if (position is FortsMoney)
                                    FortsMoney.Add(position as FortsMoney);
                                else
                                    if (position is SpotLimit)
                                        SpotLimits.Add(position as SpotLimit);

            }
        }

        public static PositionsResponse FromXmlString(String s)
        {
            PositionsResponse result;
            s = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +"<PositionsCallback>"+
                s+"</PositionsCallback>" ;
            using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(s)))
            {
                result = (PositionsResponse)_serializer.Deserialize(stream);
                return result;
            }

        }
    }
}
