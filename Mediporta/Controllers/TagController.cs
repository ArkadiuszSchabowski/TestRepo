using Mediporta.Database.Entities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IO.Compression;

namespace Mediporta.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagController
    {
        private readonly HttpClient _httpClient;

        public TagController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        [HttpGet]
        public async Task<object> Get()
        {
            var apiUrl = "https://api.stackexchange.com";

            _httpClient.BaseAddress = new Uri(apiUrl);

            var response = await _httpClient.GetAsync("/2.3/tags?page=1&pagesize=100&order=desc&min=1000&sort=popular&site=stackoverflow");

            response.EnsureSuccessStatusCode();

            using (var decompressionStream = new GZipStream(await response.Content.ReadAsStreamAsync(), CompressionMode.Decompress))

            using (var streamReader = new StreamReader(decompressionStream))
            {
                var json = await streamReader.ReadToEndAsync();

                var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(json);

                return apiResponse;
            }
        }
    }
}

