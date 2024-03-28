using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Mediporta.Database;
using Mediporta.Database.Entities;
using Newtonsoft.Json;

namespace Mediporta.Seeders
{
    public class TagSeeder
    {
        private readonly HttpClient _httpClient;
        private readonly MyDbContext _context;

        public TagSeeder(HttpClient httpClient, MyDbContext context)
        {
            _httpClient = httpClient;
            _context = context;
        }

        public async Task SeedTagsToDatabase()
        {
            List<Tag> range = new List<Tag>();

            var apiUrl = "https://api.stackexchange.com";
            _httpClient.BaseAddress = new Uri(apiUrl);

            for (int i = 1; i < 11; i++)
            {
                var response = await _httpClient.GetAsync($"/2.3/tags?page={i}&pagesize=2&order=desc&min=1000&sort=popular&site=stackoverflow");
                response.EnsureSuccessStatusCode();

                using (var decompressionStream = new GZipStream(await response.Content.ReadAsStreamAsync(), CompressionMode.Decompress))
                using (var streamReader = new StreamReader(decompressionStream))
                {
                    var json = await streamReader.ReadToEndAsync();
                    var responseApi = JsonConvert.DeserializeObject<ApiResponse>(json);

                    if (responseApi != null && responseApi.Items != null)
                    {
                        foreach (var tag in responseApi.Items)
                        {
                            range.Add(tag);
                        }
                    }
                }
            }
            _context.Tags.AddRange(range);
            await _context.SaveChangesAsync();
        }
    }
}
