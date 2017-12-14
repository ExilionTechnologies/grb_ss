using System;

namespace Exilion.GRB.ShapeShift.Models
{
    public class SSTransaction
    {
        public string inputTXID { get; set; }//: [Transaction ID of the input coin going into shapeshift],
        public string inputAddress { get; set; }//: [Address that the input coin was paid to for this shift],
        public string inputCurrency { get; set; }//: [Currency type of the input coin],
        public string inputAmount { get; set; }//: [Amount of input coin that was paid in on this shift],
        public string outputTXID { get; set; }//: [Transaction ID of the output coin going out to user],
        public string outputAddress { get; set; }//: [Address that the output coin was sent to for this shift],
        public string outputCurrency { get; set; }//: [Currency type of the output coin],
        public string outputAmount { get; set; }//: [Amount of output coin that was paid out on this shift],
        public string shiftRate { get; set; }//: [The effective rate the user got on this shift.],
        public string status { get; set; }//: [status of the shift]
        public TXStatus Status { get { return (TXStatus)Enum.Parse(typeof(TXStatus), status); } }
    }
}