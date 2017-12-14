using System;
using System.Collections.Generic;
using System.Text;

namespace Exilion.GRB.ShapeShift
{
    public class Pair
    {
        public string Base{ get; private set; }
        public string Counter { get; private set; }

        public Pair(string pair)
        {
            var arr = pair.Split('_');
            Base = arr[0];
            Counter = arr[1];
        }
        public override string ToString()
        {
            return string.Format("{0}_{1}", Base, Counter);
        }
    }
}
