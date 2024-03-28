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
    public interface ITagSeeder
    {
       void SeedTagsToDatabase();
    }
    public class TagSeeder : ITagSeeder
    {
        private readonly HttpClient _httpClient;
        private readonly MyDbContext _context;

        public TagSeeder(HttpClient httpClient, MyDbContext context)
        {
            _httpClient = httpClient;
            _context = context;
        }

        public void SeedTagsToDatabase()
        {
            List<Tag> listTag = new List<Tag>();

            var apiUrl = "https://api.stackexchange.com";
            _httpClient.BaseAddress = new Uri(apiUrl);

            for (int i = 1; i < 11; i++)
            {
                var response = _httpClient.GetAsync($"/2.3/tags?page={i}&pagesize=100&order=desc&min=1000&sort=popular&site=stackoverflow").Result;
                Task.Delay(1000).Wait();

                response.EnsureSuccessStatusCode();

                using (var decompressionStream = new GZipStream(response.Content.ReadAsStreamAsync().Result, CompressionMode.Decompress))
                using (var streamReader = new StreamReader(decompressionStream))
                {
                    var json = streamReader.ReadToEndAsync().Result;
                    var responseApi = JsonConvert.DeserializeObject<ApiResponse>(json);

                    if (responseApi != null && responseApi.Items != null)
                    {
                        foreach (var tag in responseApi.Items)
                        {
                            listTag.Add(tag);
                        }
                    }
                }
            }
            _context.Tags.AddRange(listTag);
            _context.SaveChanges();
        }
    }
}

