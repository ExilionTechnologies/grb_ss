using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Diagnostics;

namespace Exilion.GRB.ShapeShift.Test
{
    [TestClass]
    public class ShapeShiftTest
    {
        private string publicKey = "8184995feccccc6ba53fc5820ff9f558c3c19d58f47f91fa0d331468cc0af75826b0bb0257b9b7055e3910cd1ff147feae855fa69b6ef3ddf697e2df0e7db3eb";
        private string privateKey = "e0ad6d1d38c1a80f88956db0f36d24272522cf00cdd4bb1a776aa8e14c0129e697529431af8e4f209fe1b168387f282a4f5c389515f25068bcbca421f306f40a";

        [TestMethod]
        public void GetRates_returns_rates()
        {
            ShapeShiftProxy sut = new ShapeShiftProxy(new List<Pair>() { new Pair("btc_ltc"), new Pair("btc_eth") },publicKey,privateKey);
            var rates = sut.GetRatesAsync().Result;
            Assert.IsTrue(rates.Count == 2);
            Assert.IsTrue(rates.Any(r => r.pair == "btc_ltc" && r.rate != 0));
        }
        [TestMethod]
        public void GetRates_fails()
        {
            ShapeShiftProxy sut = new ShapeShiftProxy(new List<Pair>() { new Pair("btc_ltc"), new Pair("eth_eth") },publicKey, privateKey);
            string strErr = "";
            try
            {
                var rates = sut.GetRatesAsync().Result;
            }
            catch (Exception ex)
            {
                strErr = ex. InnerException.Message;
            }
            Assert.IsTrue(strErr.ToLower().Contains("unknown"));
        }
        [TestMethod]
        public void GetMarketInfo()
        {
            ShapeShiftProxy sut = new ShapeShiftProxy(new List<Pair>() { new Pair("btc_ltc"), new Pair("eth_eth") },publicKey, privateKey);
            var info = sut.GetMarketInfo().Result;
            Assert.IsTrue(info != null && info.Length > 0);
            var bchPairs = info.Where(i => i.Pair.Counter.ToLower() == "zec").ToList();
            Assert.IsTrue(bchPairs.Count() != 0);
        }
        [TestMethod]
        public void CreateTransaction_IncorrectWithdrawalAddress()
        {
            ShapeShiftProxy sut = new ShapeShiftProxy(new List<Pair>() { }, publicKey, privateKey);
            try
            {
                var rs = sut.CreateTransactionAsync(new Models.SSCreateTransactionRequest(new Pair("LTC_BTC"), "bad withdrawal address", "bad return address")).Result;
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.InnerException.Message.ToLower().Contains("withdrawal"));
            }
        }
        [TestMethod]
        public void CreateTransaction()
        {
            ShapeShiftProxy sut = new ShapeShiftProxy(new List<Pair>() { }, publicKey, privateKey);
            try
            {
                var rs = sut.CreateTransactionAsync(new Models.SSCreateTransactionRequest(new Pair("LTC_BTC"), "14G1fA92R4hK5xWD5KUBNvNekNPFzwHs6E", "LRYi8j6Hi8SJyjWEK4W6zAXfP5ZUmC9mPh")).Result;
                // we will not send any LTC
                // get status
                var transactions = sut.GetTransactionsAsync().Result;
                var tx = transactions.First(t => t.inputAddress == rs.deposit);
                Assert.IsTrue(tx != null);
                // cancel transaction
                var cancelResult = sut.CancelTransactionAsync( new Models.SSCancelTransactionRequest() { address = tx.inputAddress }).Result;
                Assert.IsTrue(cancelResult.Success);

            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.InnerException.Message.ToLower().Contains("withdrawal"));
            }
        }
        [TestMethod]
        public void GetTransactions()
        {
            ShapeShiftProxy sut = new ShapeShiftProxy(new List<Pair>() { }, publicKey, privateKey);
            while (true)
            {
                var transactions = sut.GetTransactionsAsync().Result;
                if (transactions.Length != 0)
                {
                    foreach(var t in transactions)
                        Debug.WriteLine(string.Format($"{t.inputCurrency}->{t.outputCurrency} {t.outputAmount} {t.status}"));
                }
                Thread.Sleep(15000);
            }
        }
    }
}
