using caseStudy.Models;

namespace caseStudy.Services
{
    public interface IEnterBridgeApiClient
    {
        Task<PaginatedResponse<ProductDto>?> GetProductsAsync(int pageNumber, int pageSize);
        Task<PaginatedResponse<ProductDto>?> GetProductsByCategoryAsync(string category, int pageNumber, int pageSize);
        Task<ProductDto?> GetProductAsync(int id);
        Task<IReadOnlyList<string>> GetCategoriesAsync();
        Task<PriceDto?> GetLatestPriceForProductAsync(int productId);
        Task<PaginatedResponse<PriceDto>?> GetPriceHistoryForProductAsync(int productId, int pageNumber, int pageSize);
    }
}
