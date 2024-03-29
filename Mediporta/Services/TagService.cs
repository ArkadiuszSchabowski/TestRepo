using Azure;
using Mediporta.Database;
using Mediporta.Database.Entities;
using Mediporta.Exceptions;
using Mediporta.Models;
using Mediporta.Validators;
using Newtonsoft.Json;
using System.IO.Compression;

namespace Mediporta.Services
{
    public interface ITagService
    {
        string SetHttpClientBaseAddress();
        List<PercentageTagsDto> CountPercentTags(List<Tag> tags);
        Task<List<Tag>> GetTags();
        Task<List<Tag>> GetTags(SelectedTagsDto dto);
        Task<ApiResponse?> DecompressionResponse(HttpResponseMessage response);
        void ReloadTasks();
    }
    public class TagService : ITagService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ITagValidator _validator;
        private readonly MyDbContext _context;

        public TagService(HttpClient httpClient, IConfiguration configuration, ITagValidator validator, MyDbContext context)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _validator = validator;
            _context = context;
        }
        public void ReloadTasks()
        {
            var apiTags = GetTags();
            

        }

        public List<PercentageTagsDto> CountPercentTags(List<Tag> tags)
        {

            var sum = tags.Sum(x => x.Count);

            var tagPercentages = tags.Select(x => new PercentageTagsDto
            {
                Name = x.Name,
                Count = x.Count,
                PercentageTag = ((double)x.Count / (double)sum) * 100
            }).ToList();

            return tagPercentages;
        }

        public async Task<List<Tag>> GetTags(SelectedTagsDto dto)
        {
            string apiUrl = SetHttpClientBaseAddress();

            _validator.ValidationSelectedTagsDto(dto);

            var response = await _httpClient.GetAsync($"{apiUrl}/2.3/tags?order={dto.Order}&sort={dto.SortBy}&site=stackoverflow&page={dto.PageNumber}&pagesize={dto.PageSize}");

            if (!response.IsSuccessStatusCode)
            {
                throw new APIUnavailableException("Wystąpił problem z zewnętrznym serwerem");
            }

            var apiResponse = await DecompressionResponse(response);

            return apiResponse.Items;
        }
        public async Task<List<Tag>> GetTags()
        {
            string apiUrl = SetHttpClientBaseAddress();

            var response = await _httpClient.GetAsync($"{apiUrl}/2.3/tags?page=1&pagesize=100&order=desc&min=1000&sort=popular&site=stackoverflow");

            if (!response.IsSuccessStatusCode)
            {
                throw new APIUnavailableException("Wystąpił problem z zewnętrznym serwerem");
            }

            var apiResponse = await DecompressionResponse(response);

            return apiResponse.Items;
        }


        public string SetHttpClientBaseAddress()
        {
            var apiUrl = _configuration.GetConnectionString("ApiUrl");

            if (apiUrl != null)
            {
                _httpClient.BaseAddress = new Uri(apiUrl);
            }
            else
            {
                throw new InvalidOperationException("Niepodano adresu serwera zewnętrznego");
            }
            return apiUrl;
        }
        public async Task<ApiResponse?> DecompressionResponse(HttpResponseMessage response)
        {
            using (var decompressionStream = new GZipStream(await response.Content.ReadAsStreamAsync(), CompressionMode.Decompress))

            using (var streamReader = new StreamReader(decompressionStream))
            {
                var json = await streamReader.ReadToEndAsync();

                var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(json);

                if (apiResponse == null)
                {
                    throw new NotFoundException("Lista nie zawiera żadnych tagów");
                }
                return apiResponse;
            }
        }
    }
}
