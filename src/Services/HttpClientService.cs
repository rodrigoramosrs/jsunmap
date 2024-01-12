using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace jsunmap.Services
{
    internal static class HttpClientService
    {
        internal static async Task<string> DownloadString(Uri uri)
        {
            string result = string.Empty;
            using (var client = InternalBuildHttpClient())
            {
                client.DefaultRequestHeaders.Host = uri.Host;
                HttpResponseMessage response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    Stream stream = await response.Content.ReadAsStreamAsync();
                    if (response.Content.Headers.ContentEncoding.Contains("gzip"))
                    {
                        stream = new GZipStream(stream, CompressionMode.Decompress);
                    }
                    else if (response.Content.Headers.ContentEncoding.Contains("br"))
                    {
                        stream = new BrotliStream(stream, CompressionMode.Decompress);
                    }

                    using (StreamReader reader = new StreamReader(stream))
                    {
                        result = await reader.ReadToEndAsync();
                    }
                }
            }
            return result;
        }

        private static HttpClient InternalBuildHttpClient()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");
            client.DefaultRequestHeaders.Accept.ParseAdd("text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
            client.DefaultRequestHeaders.AcceptEncoding.ParseAdd("gzip, deflate, br");
            client.DefaultRequestHeaders.AcceptLanguage.ParseAdd("en-US,en;q=0.9");
            client.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue { NoCache = true };
            client.DefaultRequestHeaders.Connection.ParseAdd("keep-alive");
            client.DefaultRequestHeaders.Referrer = new Uri("https://www.google.com/");
            return client;
        }
    }
}
