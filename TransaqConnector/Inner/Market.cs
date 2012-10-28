using System;
using System.Collections.Generic;

using System.Text;

namespace StockSharp.Transaq.Inner
{
    internal class Market
    {
        public Market() 
        { 
        }

        public Market(string id, string name)
        {
            ID = id;
            Name = name;
        }

        #region Public properties
        public string ID{get; set;}

        public string Name{get; set;}
        #endregion
    }
}
