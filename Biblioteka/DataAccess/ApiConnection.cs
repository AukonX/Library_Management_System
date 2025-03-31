using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Biblioteka.DataAccess
{
    internal class ApiConnection
    {
        private const string ApiKey = "";
        private const string BaseUrl = "";

        private readonly HttpClient _httpClient;

        public ApiConnection()
        {
            _httpClient = new HttpClient();
        }

        public async Task<BookData?> GetVolumeInfoDataAsync(string isbn)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"{BaseUrl}?q=isbn:{isbn}&key={ApiKey}");
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();

                BookData? bookData = JsonConvert.DeserializeObject<BookData>(responseBody);

                return bookData;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

    }

    public class BookData
    {
        public BookInfo[]? items { get; set; }
    }

    public class BookInfo
    {
        public VolumeInfoData VolumeInfo { get; set; }
    }

    public class VolumeInfoData
    {
        public string? Title { get; set; }
        public string? publishedDate { get; set; }
    }
}
