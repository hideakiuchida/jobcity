using CsvHelper;
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
            try
            {
                string uri = $"https://stooq.com/q/l/?s=aapl.us&f=sd2t2ohlcv&h&e=csv";
                using (var msg = new HttpRequestMessage(HttpMethod.Get, new Uri(uri)))
                {
                    msg.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/csv"));
                    using (var resp = await _client.SendAsync(msg))
                    {
                        resp.EnsureSuccessStatusCode();

                        using (var s = await resp.Content.ReadAsStreamAsync())
                        using (var sr = new StreamReader(s))
                        using (var futureoptionsreader = new CsvReader(sr, CultureInfo.CurrentCulture))
                        {
                            var x = futureoptionsreader;
                        }
                    }
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                return "An exception occurs while communicating with Chatbot";
            }

        }
    }
}
