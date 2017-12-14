using System;
using System.Collections.Generic;
using System.Text;

namespace Exilion.GRB.ShapeShift.Models
{
    public class SSRate
    {
        public string pair { get; set; }
        public decimal rate { get; set; }

        private Pair _pair;
        public Pair Pair
        {
            get
            {
                if(_pair == null)
                    _pair = new Pair(pair);
                return _pair;
            }
        }
    }
}
