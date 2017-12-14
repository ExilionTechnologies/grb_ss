using System;
using System.Collections.Generic;
using System.Text;

namespace Exilion.GRB.ShapeShift.Models
{
    public class SSMarketInfo
    {
        public string rate { get; set; }
        public decimal limit { get; set; }
        public string pair { get; set; }
        public decimal maxLimit { get; set; }
        public decimal min { get; set; }
        public decimal minerFee { get; set; }
        private Pair _pair;
        public Pair Pair
        {
            get
            {
                if (_pair == null)
                    _pair = new Pair(pair);
                return _pair;
            }
        }
    }
}
