using Mediporta.Database;
using Mediporta.Database.Entities;
using Mediporta.Exceptions;
using Mediporta.Services;
using Newtonsoft.Json;
using System.IO.Compression;

namespace Mediporta.Seeders
{
    public interface ITagSeeder
    {
        Task SeedTagsToDatabase();
        void SaveTagsToDatabase(List<Tag> listTag);
    }

    public class TagSeeder : ITagSeeder
    {
        private readonly HttpClient _httpClient;
        private readonly MyDbContext _context;
        private readonly ITagService _service;

        public TagSeeder(HttpClient httpClient, MyDbContext context, ITagService service)
        {
            _httpClient = httpClient;
            _context = context;
            _service = service;
        }
        public async Task SeedTagsToDatabase()
        {
            var tags = await GetTags();
            SaveTagsToDatabase(tags);
        }

        public async Task<List<Tag>> GetTags()
        {
            string apiUrl = _service.SetHttpClientBaseAddress();

            List<Tag> listTag = new List<Tag>();

            for (int i = 1; i < 11; i++)
            {
                var response = _httpClient.GetAsync($"{apiUrl}/2.3/tags?page={i}&pagesize=100&order=desc&min=1000&sort=popular&site=stackoverflow").Result;

                Task.Delay(1000).Wait();

                if (!response.IsSuccessStatusCode)
                {
                    throw new ApiUnavailableException("Wystąpił problem z zewnętrznym serwerem");
                }

                var apiResponse = await _service.DecompressionResponse(response);

                if (apiResponse != null && apiResponse.Items != null)
                {
                    foreach (var tag in apiResponse.Items)
                    {
                        listTag.Add(tag);
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

