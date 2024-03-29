using Mediporta.Models;
using Mediporta.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Numerics;


namespace Mediporta.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagController
    {
        private readonly ITagService _service;

        public TagController(ITagService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<object> Get()
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
    }
}

