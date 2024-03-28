using Mediporta.Database;
using Mediporta.Database.Entities;
using Mediporta.Seeders;
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
        private readonly MyDbContext _context;
        private readonly ITagSeeder _seeder;

        public TagController(HttpClient httpClient, MyDbContext context, ITagSeeder seeder)
        {
            _httpClient = httpClient;
            _context = context;
            _seeder = seeder;
        }
        [HttpGet]
        public async Task<object> Get()
        {
            if (!_context.Tags.Any())
            {
                _seeder.SeedTagsToDatabase();
            }
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

