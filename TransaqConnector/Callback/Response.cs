using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Reflection;
using System.Linq;

namespace StockSharp.Transaq.Callback
{
    internal class Response
    {
        private static Dictionary<ResponeTypes, string> responseToClass = new Dictionary<ResponeTypes,string>();


        static Response()
        {
#warning переделать в поиск по атрибутам классов
			responseToClass.Add(ResponeTypes.error, "StockSharp.Transaq.Callback.ErrorResponse");
			responseToClass.Add(ResponeTypes.server_status, "StockSharp.Transaq.Callback.ServerStatusResponse");
			responseToClass.Add(ResponeTypes.markets, "StockSharp.Transaq.Callback.MarketsResponse");
			responseToClass.Add(ResponeTypes.candlekinds, "StockSharp.Transaq.Callback.CandleKindsResponse");
			responseToClass.Add(ResponeTypes.securities, "StockSharp.Transaq.Callback.SecuritiesResponse");
			responseToClass.Add(ResponeTypes.client, "StockSharp.Transaq.Callback.ClientResponse");
			responseToClass.Add(ResponeTypes.positions, "StockSharp.Transaq.Callback.PositionsResponse");
			responseToClass.Add(ResponeTypes.overnight, "StockSharp.Transaq.Callback.OvernightResponse");
			responseToClass.Add(ResponeTypes.alltrades, "StockSharp.Transaq.Callback.AllTradesResponse");
        }


        public static T Instance<T>(string s) where T:new()
        {
            if (typeof(T).BaseType != typeof(Response))
                return default(T);
            
            MethodInfo methodInfo = typeof(T).GetMethod("FromXmlString");
            return (T)methodInfo.Invoke(null, new object[]{s}); 

        }

        public static Response Instance(string s)
        {
            Type type = DetectType(s);
            MethodInfo method = typeof(Response).GetMethods().SingleOrDefault(m => m.IsGenericMethod && m.Name=="Instance");

            MethodInfo genericMethod = method.MakeGenericMethod(new[] { type }); 

            Response result = genericMethod.Invoke(null, new[] { s }) as Response;
            return result;
        }

        public static Type DetectType(string str)
        {
            var settings = new XmlReaderSettings
            {
                CloseInput = false,
                IgnoreComments = true,
                IgnoreProcessingInstructions = true,
            };

            MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(str));
            long pos = stream.Position;
            try
            {
                using (var reader = XmlReader.Create(stream, settings))
                {
                    reader.MoveToContent();

                    ResponeTypes responseType = (ResponeTypes)Enum.Parse(typeof(ResponeTypes), reader.LocalName);
                    Type T = Type.GetType(responseToClass[responseType]);
                    return T;
                }
            }
            finally { stream.Position = pos; }
        }
    }

    internal enum ResponeTypes
    {
        error,
        server_status,
        markets,
        candlekinds,
        securities,
        client,
        positions,
        overnight,
		alltrades,
        
        candles,
        quotations,
        quotes,
        orders,
        trades,
        marketOrd,
        leverage_Control,
    }

}
