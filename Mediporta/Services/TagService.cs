using Mediporta.Database;
using Mediporta.Database.Entities;
using Mediporta.Exceptions;
using Mediporta.Seeders;
using Newtonsoft.Json;
using System.IO.Compression;
using System.Net.Http;

namespace Mediporta.Services
{
    public interface ITagService
    {
        Task<List<Tag>> GetTags();
    }
    public class TagService : ITagService
    {
        private readonly HttpClient _httpClient;

        public TagService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<List<Tag>> GetTags()
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

                if (apiResponse == null)
                {
                    throw new NotFoundException("Lista nie zawiera żadnych tagów");
                }

                return apiResponse.Items;
            }
        }
    }
}
