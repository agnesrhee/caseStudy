using caseStudy.Models;
using caseStudy.Services;
using Microsoft.AspNetCore.Mvc;

namespace caseStudy.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IEnterBridgeApiClient _apiClient;

        public ProductsController(IEnterBridgeApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 10)
        {
            pageNumber = Math.Max(pageNumber, 1);
            pageSize = Math.Clamp(pageSize, 1, 50);

            var products = await _apiClient.GetProductsAsync(pageNumber, pageSize);
            if (products is null)
            {
                return View(null);
            }

            var productListItems = await Task.WhenAll(products.Items.Select(async product => new ProductListItemViewModel
            {
                Product = product,
                LatestPrice = await _apiClient.GetLatestPriceForProductAsync(product.Id)
            }));

            var model = new PaginatedResponse<ProductListItemViewModel>
            {
                PageNumber = products.PageNumber,
                PageSize = products.PageSize,
                TotalCount = products.TotalCount,
                TotalPages = products.TotalPages,
                HasPreviousPage = products.HasPreviousPage,
                HasNextPage = products.HasNextPage,
                Items = productListItems.ToList()
            };

            return View(model);
        }
    }
}
