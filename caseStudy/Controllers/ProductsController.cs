using caseStudy.Models;
using caseStudy.Services;
using caseStudy.ViewModels;
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

        public async Task<IActionResult> PriceHistory(int id)
        {
            var product = await _apiClient.GetProductAsync(id);
            if (product is null)
            {
                return NotFound();
            }

            var prices = await _apiClient.GetPriceHistoryForProductAsync(id, 1, 1000);
            var rawPrices = prices?.Items ?? [];
            var model = new ProductPriceHistoryViewModel
            {
                Product = product,
                RawPriceCount = rawPrices.Count,
                PriceChanges = GetPriceChanges(rawPrices)
            };

            return View(model);
        }

        private static IReadOnlyList<PriceChangeViewModel> GetPriceChanges(IReadOnlyList<PriceDto> prices)
        {
            var changes = new List<PriceChangeViewModel>();
            PriceDto? previousPrice = null;

            foreach (var price in prices.OrderBy(price => price.DateTime))
            {
                if (previousPrice is null ||
                    price.Amount != previousPrice.Amount ||
                    price.Quantity != previousPrice.Quantity ||
                    price.UnitOfMeasure != previousPrice.UnitOfMeasure)
                {
                    changes.Add(new PriceChangeViewModel
                    {
                        DateChanged = price.DateTime,
                        Amount = price.Amount,
                        Quantity = price.Quantity,
                        UnitOfMeasure = price.UnitOfMeasure
                    });
                }

                previousPrice = price;
            }

            return changes
                .OrderByDescending(change => change.DateChanged)
                .ToList();
        }
    }
}
