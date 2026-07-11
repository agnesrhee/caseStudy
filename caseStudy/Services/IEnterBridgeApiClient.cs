using caseStudy.Models;

namespace caseStudy.Services
{
    public interface IEnterBridgeApiClient
    {
        Task<PaginatedResponse<ProductDto>?> GetProductsAsync(int pageNumber, int pageSize);
        Task<PriceDto?> GetLatestPriceForProductAsync(int productId);
    }
}
