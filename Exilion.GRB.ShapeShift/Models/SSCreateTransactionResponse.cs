namespace Exilion.GRB.ShapeShift.Models
{
    public class SSCreateTransactionResponse
    {
        public string deposit { get; set; }//: [Deposit Address(or memo field if input coin is BTS / BITUSD)],
        public string depositType { get; set; }//: [Deposit Type(input coin symbol)],
        public string withdrawal { get; set; }//: [Withdrawal Address], //-- will match address submitted in post
        public string withdrawalType { get; set; }//: [Withdrawal Type(output coin symbol)],
        public string apiPubKey { get; set; }//: [public API attached to this shift, if one was given]
    }
}