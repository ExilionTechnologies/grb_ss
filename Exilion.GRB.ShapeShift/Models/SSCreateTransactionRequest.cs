namespace Exilion.GRB.ShapeShift.Models
{
    public class SSCreateTransactionRequest
    {
        public SSCreateTransactionRequest(Pair pair, string withdrawalAddress, string returnAddress)
        {
            this.pair = pair.ToString();
            this.withdrawal = withdrawalAddress;
            this.returnAddress = returnAddress;
            this.apiKey = apiKey;
        }

        public string pair { get; }
        public string withdrawal{ get; }
        public string returnAddress { get; }
        public string apiKey { get; set; }
    }
}