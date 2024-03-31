using Mediporta.Services;

namespace Mediporta.Tests.IntegrationTests.Services
{
    public class ITagServiceIntegrationTests
    {
        private readonly ITagService _service;

        public ITagServiceIntegrationTests(ITagService service)
        {
            _service = service;
        }
    }
}
