using Mediporta.Database.Entities;
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
        private string _apiUrl;

        public TagController(ITagService service, ITagSeeder seeder)
        {
            _service = service;
            _seeder = seeder;
        }
        [HttpGet]
        public async Task<List<Tag>> GetTags()
        {
            var response = await _service.GetTags();
            return response;

        }
        [HttpGet("count")]
        public async Task<ActionResult<List<PercentageTagsDto>>> CountTags()
        {
            var response = await _service.GetTags();
            List<PercentageTagsDto> dto = _service.CountPercentTags(response);

            return dto;
        }
        [HttpGet("sort")]
        public async Task<object> GetSelectedTags([FromQuery] SelectedTagsDto dto)
        {
            var response = await _service.GetTags(dto);
            return response;
        }
        [HttpPost("reload")]
        public async Task ReloadTags()
        {
            var tags = await _service.ReloadTasks();
            _seeder.SaveTagsToDatabase(tags);
        }
    }
}

