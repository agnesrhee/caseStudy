using System.Net.Http.Json;
using caseStudy.Models;

namespace caseStudy.Services
{
    public class EnterBridgeApiClient : IEnterBridgeApiClient
    {
        private readonly HttpClient _httpClient;

        public EnterBridgeApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<PaginatedResponse<ProductDto>?> GetProductsAsync(int pageNumber, int pageSize)
        {
            return await _httpClient.GetFromJsonAsync<PaginatedResponse<ProductDto>>($"/api/products?pageNumber={pageNumber}&pageSize={pageSize}");
        }

        public async Task<PaginatedResponse<ProductDto>?> GetProductsByCategoryAsync(string category, int pageNumber, int pageSize)
        {
            var encodedCategory = Uri.EscapeDataString(category);
            return await _httpClient.GetFromJsonAsync<PaginatedResponse<ProductDto>>(
                $"/api/products?category={encodedCategory}&PageNumber={pageNumber}&PageSize={pageSize}&sortBy=Name&sortDirection=asc");
        }

        public async Task<ProductDto?> GetProductAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<ProductDto>($"/api/products/{id}");
        }

        public async Task<IReadOnlyList<string>> GetCategoriesAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<string>>("/api/products/categories") ?? [];
        }

        public async Task<PriceDto?> GetLatestPriceForProductAsync(int productId)
        {
            var response = await _httpClient.GetFromJsonAsync<PaginatedResponse<PriceDto>>(
                $"/api/prices?productId={productId}&PageNumber=1&PageSize=1&sortBy=DateTime&sortDirection=desc");

            return response?.Items.FirstOrDefault();
        }

        public async Task<PaginatedResponse<PriceDto>?> GetPriceHistoryForProductAsync(int productId, int pageNumber, int pageSize)
        {
            return await _httpClient.GetFromJsonAsync<PaginatedResponse<PriceDto>>(
                $"/api/prices?productId={productId}&PageNumber={pageNumber}&PageSize={pageSize}&sortBy=DateTime&sortDirection=desc");
        }
    }
}
