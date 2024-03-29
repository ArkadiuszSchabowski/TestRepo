using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Mediporta.Database;
using Mediporta.Database.Entities;
using Mediporta.Exceptions;
using Mediporta.Services;
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
        private readonly IConfiguration _configuration;
        private readonly ITagService _service;

        public TagSeeder(HttpClient httpClient, MyDbContext context, IConfiguration configuration, ITagService service)
        {
            _httpClient = httpClient;
            _context = context;
            _configuration = configuration;
            _service = service;
        }
        public void SeedTagsToDatabase()
        {
            var tags = GetTags();
            SaveTagsToDatabase(tags);
        }

        public List<Tag> GetTags()
        {
            string apiUrl = _service.SetHttpClientBaseAddress();

            List<Tag> listTag = new List<Tag>();

            for (int i = 1; i < 11; i++)
            {
                var response = _httpClient.GetAsync($"{apiUrl}/2.3/tags?page={i}&pagesize=100&order=desc&min=1000&sort=popular&site=stackoverflow").Result;

                Task.Delay(1000).Wait();

                if (!response.IsSuccessStatusCode)
                {
                    throw new APIUnavailableException("Wystąpił problem z zewnętrznym serwerem");
                }

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
            return listTag;
        }
        public void SaveTagsToDatabase(List<Tag> listTag)
        {
            _context.Tags.AddRange(listTag);
            _context.SaveChanges();
        }
    }
}

