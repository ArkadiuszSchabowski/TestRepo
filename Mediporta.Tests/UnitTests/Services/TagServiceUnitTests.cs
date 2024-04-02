using FluentAssertions;
using Mediporta.Database;
using Mediporta.Database.Entities;
using Mediporta.Models;
using Mediporta.Services;
using Mediporta.Tests.Fakes;
using Mediporta.Validators;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;

namespace Mediporta.Tests.UnitTests.Services
{
    [TestFixture]
    public class TagServiceUnitTests
    {
        private ITagService _service;
        [SetUp]
        public void Setup()
        {
            var mockHttpClient = new Mock<HttpClient>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockValidator = new Mock<ITagValidator>();

            var options = new DbContextOptionsBuilder<MyDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase").Options;

            var mockContext = new Mock<FakeMyDbContext>(options);

            _service = new TagService(mockHttpClient.Object, mockConfiguration.Object, mockValidator.Object, mockContext.Object);
        }
        [Test]
        public void CountPercentTags_WhenCalled_ShouldBeReturnCorrectListOfTagPercentages()
        {
            var tags = new List<Tag>()
            {
                new Tag
                {
                    Id = 1,
                    Collectives = null,
                    Count = 6,
                    HasSynonyms = false,
                    IsModeratorOnly = false,
                    IsRequired = false,
                    LastActivityDate = null,
                    Name = "javascript",
                    Synonyms = null,
                    UserId = null
                },
                new Tag
                {
                    Id = 2,
                    Collectives = null,
                    Count = 4,
                    HasSynonyms = false,
                    IsModeratorOnly = false,
                    IsRequired = false,
                    LastActivityDate = null,
                    Name = "java",
                    Synonyms = null,
                    UserId = null
                }
            };

            var result = _service.CountPercentTags(tags);

            result.Should().BeEquivalentTo(new List<PercentageTagsDto>{
                new PercentageTagsDto { Name = "javascript", Count = 6, PercentageTag = 60 },
                new PercentageTagsDto { Name = "java", Count = 4, PercentageTag = 40 }});
        }
        [Test]
        [Category("TestExplorerMode")]
        public void SetHttpClientBaseAddress_WhenCalled_ReturnStringApiUrl()
        {
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                { { "ApiUrl", "https://api.stackexchange.com"}}
                .Select(kv => new KeyValuePair<string, string?>(kv.Key, kv.Value))
                .ToList()).Build();

            var httpClient = new HttpClient();

            var service = new TagService(httpClient, configuration, null, null);

            var result = service.SetHttpClientBaseAddress();

            Assert.That(result, Is.EqualTo("https://api.stackexchange.com"));
        }
    }
}
