using caseStudy.Data;
using caseStudy.Models;
using caseStudy.Services;
using caseStudy.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace caseStudy.Controllers
{
    public class SalesController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IEnterBridgeApiClient _apiClient;

        public SalesController(ApplicationDbContext dbContext, IEnterBridgeApiClient apiClient)
        {
            _dbContext = dbContext;
            _apiClient = apiClient;
        }

        public async Task<IActionResult> Index()
        {
            var sales = await _dbContext.Sales
                .OrderByDescending(sale => sale.SoldAt)
                .ToListAsync();

            return View(sales);
        }

        public async Task<IActionResult> Recurring()
        {
            var sales = await _dbContext.Sales
                .Where(sale => sale.IsRecurring)
                .OrderBy(sale => sale.ProductName)
                .ThenBy(sale => sale.Username)
                .ToListAsync();

            return View(sales);
        }

        public async Task<IActionResult> Create(int? buyAgainSaleId = null)
        {
            var model = new SalesCreateViewModel
            {
                Categories = await _apiClient.GetCategoriesAsync()
            };

            if (buyAgainSaleId is not null)
            {
                var sale = await _dbContext.Sales.FindAsync(buyAgainSaleId);

                if (sale is not null)
                {
                    model.Username = sale.Username;
                    model.Items =
                    [
                        new SalesLineItemInputModel
                        {
                            Category = sale.ProductCategory,
                            ProductId = sale.ProductId,
                            Quantity = sale.Quantity
                        }
                    ];
                }
            }

            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var sale = await _dbContext.Sales.FindAsync(id);
            if (sale is null)
            {
                return NotFound();
            }

            var model = new SalesEditViewModel
            {
                Id = sale.Id,
                Username = sale.Username,
                Categories = await _apiClient.GetCategoriesAsync(),
                Category = sale.ProductCategory,
                ProductId = sale.ProductId,
                Quantity = sale.Quantity
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SalesEditViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                model.Categories = await _apiClient.GetCategoriesAsync();
                return View(model);
            }

            var sale = await _dbContext.Sales.FindAsync(id);
            if (sale is null)
            {
                return NotFound();
            }

            var product = await _apiClient.GetProductAsync(model.ProductId);
            var latestPrice = await _apiClient.GetLatestPriceForProductAsync(model.ProductId);

            if (product is null)
            {
                ModelState.AddModelError(string.Empty, $"Product {model.ProductId} could not be found.");
            }

            if (product is not null && latestPrice is null)
            {
                ModelState.AddModelError(string.Empty, $"{product.Name} does not have a price yet.");
            }

            if (!ModelState.IsValid)
            {
                model.Categories = await _apiClient.GetCategoriesAsync();
                return View(model);
            }

            sale.Username = model.Username;
            sale.ProductId = product!.Id;
            sale.ProductName = product.Name;
            sale.ProductSku = product.Sku;
            sale.ProductCategory = product.Category;
            sale.Price = latestPrice!.Amount;
            sale.Quantity = model.Quantity;

            await _dbContext.SaveChangesAsync();
            TempData["SuccessMessage"] = "Sale updated successfully.";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SalesCreateViewModel model)
        {
            model.Items = model.Items
                .Where(item => item.ProductId > 0 || !string.IsNullOrWhiteSpace(item.Category))
                .ToList();

            if (model.Items.Count == 0)
            {
                ModelState.AddModelError(string.Empty, "Add at least one product.");
            }

            var duplicateProductIds = model.Items
                .Where(item => item.ProductId > 0)
                .GroupBy(item => item.ProductId)
                .Where(group => group.Count() > 1)
                .Select(group => group.Key)
                .ToList();

            if (duplicateProductIds.Count > 0)
            {
                ModelState.AddModelError(string.Empty, "That product is already selected. Remove the duplicate or adjust the quantity.");
            }

            if (!ModelState.IsValid)
            {
                model.Categories = await _apiClient.GetCategoriesAsync();
                return View(model);
            }

            foreach (var item in model.Items)
            {
                var product = await _apiClient.GetProductAsync(item.ProductId);
                var latestPrice = await _apiClient.GetLatestPriceForProductAsync(item.ProductId);

                if (product is null)
                {
                    ModelState.AddModelError(string.Empty, $"Product {item.ProductId} could not be found.");
                    continue;
                }

                if (latestPrice is null)
                {
                    ModelState.AddModelError(string.Empty, $"{product.Name} does not have a price yet.");
                    continue;
                }

                _dbContext.Sales.Add(new Sale
                {
                    Username = model.Username,
                    ProductId = product.Id,
                    ProductName = product.Name,
                    ProductSku = product.Sku,
                    ProductCategory = product.Category,
                    Price = latestPrice.Amount,
                    Quantity = item.Quantity,
                    SoldAt = DateTime.UtcNow
                });
            }

            if (!ModelState.IsValid)
            {
                model.Categories = await _apiClient.GetCategoriesAsync();
                return View(model);
            }

            await _dbContext.SaveChangesAsync();
            TempData["SuccessMessage"] = "Sale logged successfully.";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> ProductsByCategory(string category)
        {
            if (string.IsNullOrWhiteSpace(category))
            {
                return Json(Array.Empty<object>());
            }

            var products = await _apiClient.GetProductsByCategoryAsync(category, 1, 1000);

            return Json(products?.Items.Select(product => new
            {
                id = product.Id,
                name = product.Name,
                sku = product.Sku
            }) ?? []);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleRecurring(int id, string returnAction = nameof(Index))
        {
            var sale = await _dbContext.Sales.FindAsync(id);
            if (sale is null)
            {
                return NotFound();
            }

            sale.IsRecurring = !sale.IsRecurring;
            await _dbContext.SaveChangesAsync();

            TempData["SuccessMessage"] = sale.IsRecurring
                ? "Sale marked as recurring."
                : "Sale removed from recurring sales.";

            return returnAction == nameof(Recurring)
                ? RedirectToAction(nameof(Recurring))
                : RedirectToAction(nameof(Index));
        }
    }
}
