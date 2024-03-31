using Mediporta.Exceptions;
using Mediporta.Models;
using Mediporta.Validators;
using NUnit.Framework;

namespace Mediporta.Tests.UnitTests.Validators
{
    public class TagRequestValidatorUnitTests
    {
        [SetUp]
        public void Setup()
        {
            _validator = new TagRequestValidator();
        }
        private TagRequestValidator _validator;

        [Test]
        public void ValidationSelectedTagsDto_WhenGetCorrectParametrs_ShouldNotThrowBadRequestException()
        {
            var dto = new SelectedTagsDto()
            {
                Order = "asc",
                SortBy = "name",
                PageNumber = 1,
                PageSize = 10
            };
            Assert.DoesNotThrow(() => _validator.ValidationSelectedTagsDto(dto));
        }
        [Test]
        public void ValidationSelectedTagsDto_WhenGetIncorrectParametrPageSize_ShouldThrowBadRequestException()
        {
            var dto = new SelectedTagsDto()
            {
                Order = "asc",
                SortBy = "name",
                PageNumber = 1,
                PageSize = 0
            };

            Assert.Throws<BadRequestException>(() => _validator.ValidationSelectedTagsDto(dto));
        }
        [Test]
        public void ValidationSelectedTagsDto_WhenGetIncorrectParametrOrder_ShouldThrowBadRequestException()
        {
            var dto = new SelectedTagsDto()
            {
                Order = "inncorect_parametr",
                SortBy = "name",
                PageNumber = 1,
                PageSize = 10
            };
            Assert.Throws<BadRequestException>(() => _validator.ValidationSelectedTagsDto(dto));
        }
        [Test]
        public void ValidationSelectedTagsDto_WhenGetIncorrectParametrSortby_ShouldThrowBadRequestException()
        {
            var dto = new SelectedTagsDto()
            {
                Order = "asc",
                SortBy = "inncorect_parametr",
                PageNumber = 1,
                PageSize = 10
            };
            Assert.Throws<BadRequestException>(() => _validator.ValidationSelectedTagsDto(dto));
        }
    }
}
