using Mediporta.Models;
using Mediporta.Seeders;
using Mediporta.Services;
using Microsoft.AspNetCore.Mvc;

namespace Mediporta.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagController
    {
        private readonly ITagService _service;
        private readonly ITagSeeder _seeder;
        private readonly string _apiUrl;

        public TagController(ITagService service, ITagSeeder seeder)
        {
            _service = service;
            _seeder = seeder;
            _apiUrl = _service.SetHttpClientBaseAddress();
        }
        [HttpGet]
        public async Task<object> Get()
        {
            var response = await _service.GetTags(_apiUrl);
            return response;

        }
        [HttpGet("count")]
        public async Task<ActionResult<List<PercentageTagsDto>>> CountTags()
        {
            var response = await _service.GetTags(_apiUrl);
            List<PercentageTagsDto> dto = _service.CountPercentTags(response);

            return dto;
        }
        [HttpGet("sort")]
        public async Task<object> GetSelectedTags([FromQuery] SelectedTagsDto dto)
        {
            var response = await _service.GetTags(dto, _apiUrl);
            return response;
        }
        [HttpPost("reload")]
        public void ReloadTags()
        {
            var tags = _service.ReloadTasks(_apiUrl);
            _seeder.SaveTagsToDatabase(tags);
        }
    }
}

