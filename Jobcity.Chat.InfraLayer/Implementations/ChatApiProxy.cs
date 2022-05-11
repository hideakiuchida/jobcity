using CsvHelper;
using Jobcity.Chat.Domain;
using Jobcity.Chat.InfraLayer.Contracts;
using Jobcity.Chat.InfraLayer.Dtos.Responses;
using RestSharp;
using System;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Jobcity.Chat.InfraLayer.Implementations
{
    public class ChatApiProxy : IChatApiProxy
    {
        private readonly HttpClient _client;
        public ChatApiProxy()
        {
            _client = new HttpClient();
        }
        public async Task<string> DownloadCsv(string stockCode)
        {
            string message = string.Empty;
            string uri = $"https://stooq.com/q/l/?s=aapl.us&f=sd2t2ohlcv&h&e=csv";
            using (var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, new Uri(uri)))
            {
                httpRequestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/csv"));
                using (var resp = await _client.SendAsync(httpRequestMessage))
                {
                    resp.EnsureSuccessStatusCode();

                    using (var s = await resp.Content.ReadAsStreamAsync())
                    using (var sr = new StreamReader(s))
                    using (var csvReader = new CsvReader(sr, CultureInfo.CurrentCulture))
                    {
                        var stockCodeCsv = csvReader.GetRecords<StockCodeCsv>();
                        foreach (var item in stockCodeCsv)
                        {
                            if (item.Symbol.ToLower().Equals(Constants.Commands.AaplUsCode.ToLower()))
                            {
                                message = $"{item.Symbol} quote is ${item.High} per share.";
                            }
                        }
                    }
                }
            }
            return message;
        }
    }
}
