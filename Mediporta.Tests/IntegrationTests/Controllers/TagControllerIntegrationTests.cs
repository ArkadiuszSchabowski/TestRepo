using Mediporta.Controllers;
using Mediporta.Database;
using Mediporta.Database.Entities;
using Mediporta.Exceptions;
using Mediporta.Services;
using Mediporta.Tests.Fakes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System;

namespace Mediporta.Tests.IntegrationTests.Controllers
{
    public class TagControllerIntegrationTests
    {
        private ITagService _service;
        private TagController _controller;

        [Test]
        [Category("TestExplorerMode")]
        public async Task GetTags_WhenCalled_ShouldReturnListOfTags()
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
            var result = await _controller.GetTags();

            Assert.That(result, Is.InstanceOf<List<Tag>>());
        }
        [Test]
        [Category("TestExplorerMode")]
        public async Task GetTags_WhenCalledWithWrongConfig_ThrowsUrlException()
        {
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string> { { "ApiUrl", "https://api.stackxchange.com" } })
                .Build();

            var options = new DbContextOptionsBuilder<MyDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            var context = new FakeMyDbContext(options);

            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;

            var httpClient = new HttpClient(httpClientHandler);

            _service = new TagService(httpClient, configuration, null, context);

            _controller = new TagController(_service, null);

            Assert.ThrowsAsync<ApiUnavailableException>(async () => await _controller.GetTags());
        }
    }
}
