using Mediporta.Database;
using Mediporta.Exceptions;
using Mediporta.Models;

namespace Mediporta.Validators
{
    public interface ITagValidator
    {
        void ValidationSelectedTagsDto(SelectedTagsDto dto);
    }
    public class TagRequestValidator : ITagValidator
    {

        public void ValidationSelectedTagsDto(SelectedTagsDto dto)
        {
            if (dto.PageSize < 1 || dto.PageSize > 100)
            {
                throw new BadRequestException("Rozmiar strony powinien mieścić się w zakresie 1-100");
            }

            if (!dto.SortBy.Equals("Name", StringComparison.OrdinalIgnoreCase) && !dto.SortBy.Equals("Popular", StringComparison.OrdinalIgnoreCase))
            {
                throw new BadRequestException("Możliwości sortowania to odpowiednio: Name lub Popular");
            }

            if (!dto.Order.Equals("asc", StringComparison.OrdinalIgnoreCase) && !dto.Order.Equals("desc", StringComparison.OrdinalIgnoreCase))
            {
                throw new BadRequestException("Możliwości sortowania rosnące: asc lub malejące: desc");
            }
        }
    }
}
