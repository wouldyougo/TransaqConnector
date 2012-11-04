using System;
using System.Runtime.InteropServices;

namespace StockSharp.Transaq.Inner
{

    internal class TXMLConnector
    {
        //locker objects
        private object _sendCommandSync = new object();
        private static object syncRoot = new object();

        private static volatile TXMLConnector _instance;
        private delegate void CallBackDelegate(IntPtr pData);
        private CallBackDelegate _callback;


        [DllImport("txmlconnector64.dll", CharSet = CharSet.Unicode, PreserveSig = true, CallingConvention = CallingConvention.Winapi)]
        private static extern bool SetCallback(CallBackDelegate pCallback);

        [DllImport("txmlconnector64.dll", CharSet = CharSet.Unicode, PreserveSig = true, CallingConvention = CallingConvention.Winapi)]
        private static extern IntPtr SendCommand(IntPtr pData);

        [DllImport("txmlconnector64.dll", CharSet = CharSet.Unicode, PreserveSig = true, CallingConvention = CallingConvention.Winapi)]
        private static extern bool FreeMemory(IntPtr pData);


        public event EventHandler<CallbackEventArgs> ResponseHandler;


        private TXMLConnector()
        {
            _callback = new CallBackDelegate(ReceiveResponse);

            if (!SetCallback(_callback))
                throw (new ApplicationException("Unable set CallBack function."));
        }

        public static TXMLConnector Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                            _instance = new TXMLConnector();
                    }
                }

                return _instance;
            }
        }




        public String SendCommand(String command)
        {
            //Локирует, поскольку у транзаковской библиотеки нет синхронизации при отправке команд!
            lock (_sendCommandSync)
            {
                IntPtr pData = MarshalUTF8.StringToHGlobalUTF8(command);
                IntPtr pResult = SendCommand(pData);

                String result = MarshalUTF8.PtrToStringUTF8(pResult);

                Marshal.FreeHGlobal(pData);
                FreeMemory(pResult);

                return result;
            }
        }



        protected void OnCallback(string response)
        {
            if (ResponseHandler != null)
                ResponseHandler(this, new CallbackEventArgs(response));
        }

        private void ReceiveResponse(IntPtr pData)
        {
            string data = MarshalUTF8.PtrToStringUTF8(pData);
            FreeMemory(pData);
            OnCallback(data);
        }




    }


}
