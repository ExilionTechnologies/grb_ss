using System;
using System.Collections.Generic;
using System.Text;

namespace Exilion.GRB.ShapeShift.Models
{
    public class SSCancelTransactionResponse
    {
        public string success { get; }
        public bool Success { get { return success.ToLower().Contains("cancelled"); } }
    }
}
