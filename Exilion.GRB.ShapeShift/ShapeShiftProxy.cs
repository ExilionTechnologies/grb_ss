using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Exilion.GRB.ShapeShift.Models;
using Newtonsoft.Json;

namespace Exilion.GRB.ShapeShift
{
    public class ShapeShiftProxy
    {
        private HttpClient _httpClient = new HttpClient();
        private const string _baseURL = "https://shapeshift.io";
        private List<Pair> _supportedPairs;
        private string _publicKey;
        private string _privateKey;
        public ShapeShiftProxy(List<Pair> supportedPairs, string publicKey, string privateKey)
        {
            _privateKey = privateKey;
            _publicKey = publicKey;

            _supportedPairs = supportedPairs;
            _httpClient.BaseAddress = new Uri(_baseURL);
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

        }

        public async Task<List<SSRate>> GetRatesAsync()
        {
            var rates = new List<SSRate>();
            foreach (var pair in _supportedPairs)
                rates.Add(await GetRateAsync(pair));
            return rates;
        }

        public async Task<SSCreateTransactionResponse> CreateTransactionAsync(SSCreateTransactionRequest request)
        {
            request.apiKey = _publicKey;
            string uri = "shift";
            return await PostAsync<SSCreateTransactionRequest, SSCreateTransactionResponse>(uri, request);
        }
        public async Task<SSCancelTransactionResponse> CancelTransactionAsync(SSCancelTransactionRequest request)
        {
            return await PostAsync<SSCancelTransactionRequest,SSCancelTransactionResponse>("cancelpending", request);
        }
        public async Task<SSTransaction[]> GetTransactionsAsync()
        {
            return await QueryAsync<SSTransaction[]>(string.Format($"txbyapikey/{_privateKey}"));

        }
        public async Task<SSTransaction> GetTransactionAsync(string address)
        {
            return await QueryAsync<SSTransaction>(string.Format($"txbyapikey/{address}/{_privateKey}"));

        }
        public async Task<SSMarketInfo[]> GetMarketInfo()
        {

            string uri = string.Format("marketinfo");
            return await QueryAsync<SSMarketInfo[]>(uri);
        }

        private async Task<SSRate> GetRateAsync(Pair pair)
        {

            string uri = string.Format("rate/{0}", pair.ToString());
            return await QueryAsync<SSRate>(uri);
        }

        private async Task<T> QueryAsync<T>(string uri) 
        {
            T model = default(T);
            HttpResponseMessage response = await _httpClient.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var str = await response.Content.ReadAsStringAsync();
                if (str.Contains("error"))
                {
                    var err = JsonConvert.DeserializeObject<SSError>(str);
                    throw new Exception(err.error);
                }
                model = JsonConvert.DeserializeObject<T>(str);
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }
            return model;
        }

        private async Task<TOut> PostAsync<TIn,TOut>(string uri, TIn form)
        {
            string strForm = JsonConvert.SerializeObject(form);
            var content = new StringContent(strForm, Encoding.UTF8, "application/json");

            var model = default(TOut);

            HttpResponseMessage response = await _httpClient.PostAsync(uri, content);

            if (response.IsSuccessStatusCode)
            {
                var str = await response.Content.ReadAsStringAsync();
                if (str.Contains("error"))
                {
                    var err = JsonConvert.DeserializeObject<SSError>(str);
                    throw new Exception(err.error);
                }
                model = JsonConvert.DeserializeObject<TOut>(str);
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }
            return model;
        }
    }
}
