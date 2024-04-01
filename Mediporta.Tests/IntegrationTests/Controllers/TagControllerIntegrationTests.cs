using Mediporta.Controllers;
using Mediporta.Database;
using Mediporta.Database.Entities;
using Mediporta.Services;
using Mediporta.Tests.Fakes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace Mediporta.Tests.IntegrationTests.Controllers
{
    public class TagControllerIntegrationTests
    {
        private ITagService _service;
        private TagController _controller;

        [SetUp]
        public void Setup()
        {
            var configuration = new ConfigurationBuilder()
             .AddInMemoryCollection(new Dictionary<string, string> { { "ApiUrl", "https://api.stackexchange.com" } }
             .Select(kv => new KeyValuePair<string, string?>(kv.Key, kv.Value))
             .ToList())
             .Build();

            var httpClient = new HttpClient();

            var options = new DbContextOptionsBuilder<MyDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase").Options;

            var context = new FakeMyDbContext(options);

            _service = new TagService(httpClient, configuration, null, context);

            _controller = new TagController(_service, null);
        }

        [Test]
        public async Task GetTags_WhenCalled_ShouldReturnListOfTags()
        {
            var result = await _controller.GetTags();

            Assert.That(result, Is.InstanceOf<List<Tag>>());
        }
    }
}
