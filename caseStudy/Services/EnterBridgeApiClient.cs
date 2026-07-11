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

        public async Task<PriceDto?> GetLatestPriceForProductAsync(int productId)
        {
            var response = await _httpClient.GetFromJsonAsync<PaginatedResponse<PriceDto>>(
                $"/api/prices?productId={productId}&PageNumber=1&PageSize=1&sortBy=DateTime&sortDirection=desc");

            return response?.Items.FirstOrDefault();
        }
    }
}
