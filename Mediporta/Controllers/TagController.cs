using Mediporta.Database;
using Mediporta.Database.Entities;
using Mediporta.Models;
using Mediporta.Seeders;
using Mediporta.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IO.Compression;

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
            var apiResponse = await _service.GetTags();
            return apiResponse;

        }
        [HttpGet("counter")]
        public double CountTags([FromQuery] TagCounterDto dto)
        {
            return 2.2;
        }
    }
}

