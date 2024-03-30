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
using System.Collections.Generic;

namespace Mediporta.Tests.UnitTests.Services
{
    public class TagServiceUnitTests
    {
        [Test]
        public void CountPercentTags_WhenCalled_ShouldBeReturnTagPercentages()
        {
            var mockHttpClient = new Mock<HttpClient>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockValidator = new Mock<ITagValidator>();

            var options = new DbContextOptionsBuilder<MyDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase").Options;

            var mockContext = new Mock<FakeMyDbContext>(options);

            var tagService = new TagService(mockHttpClient.Object, mockConfiguration.Object, mockValidator.Object, mockContext.Object);

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

            var result = tagService.CountPercentTags(tags);

            result.Should().BeEquivalentTo(new List<PercentageTagsDto>
            {
                new PercentageTagsDto { Name = "javascript", Count = 6, PercentageTag = 60 },
                new PercentageTagsDto { Name = "java", Count = 4, PercentageTag = 40 }
            });
        }
    }
}
