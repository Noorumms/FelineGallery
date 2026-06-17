using Feline_Gallery_v1.Data;
using Feline_Gallery_v1.Models;
using Feline_Gallery_v1.Models.Interfaces;
using Feline_Gallery_v1.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Feline_Gallery_v1.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IArtworkRepository _artworkRepo;
        private const string CartSessionKey = "ShoppingCart";

        public CheckoutController(IOrderRepository orderRepo, IArtworkRepository artworkRepo)
        {
            _orderRepo = orderRepo;
            _artworkRepo = artworkRepo;
        }

        private Cart GetCart()
        {
            var cartJson = HttpContext.Session.GetString(CartSessionKey);
            if (string.IsNullOrEmpty(cartJson))
            {
                return new Cart();
            }
            return JsonSerializer.Deserialize<Cart>(cartJson) ?? new Cart();
        }

        public IActionResult Index()
        {
            var cart = GetCart();

            if (cart.Items == null || cart.Items.Count == 0)
            {
                TempData["ErrorMessage"] = "Your cart is empty.";
                return RedirectToAction("Index", "Shop");
            }

            var viewModel = new CheckoutVM
            {
                Cart = cart,
                ShippingCost = 0,
                Tax = 0,
                Country = "United States"
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PlaceOrder(CheckoutVM model)
        {
            Console.WriteLine("=== PlaceOrder called ===");

            var cart = GetCart();

            if (cart.Items == null || cart.Items.Count == 0)
            {
                TempData["ErrorMessage"] = "Your cart is empty.";
                return RedirectToAction("Index", "Shop");
            }

            model.Cart = cart;

            // Debug: Log validation errors
            if (!ModelState.IsValid)
            {
                var errors = new List<string>();
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                        Console.WriteLine($"Validation Error: {error.ErrorMessage}");
                    }
                }

                TempData["ErrorMessage"] = $"Please fill all required fields. Errors: {string.Join(", ", errors)}";
                return View("Index", model);
            }

            Console.WriteLine($"Creating order for: {model.CustomerName}");

            // Verify artworks still available
            foreach (var item in cart.Items)
            {
                var artwork = await _artworkRepo.GetByIdAsync(item.ArtworkId);
                if (artwork == null || !artwork.IsAvailable)
                {
                    TempData["ErrorMessage"] = $"Sorry, '{item.Title}' is no longer available.";
                    return View("Index", model);
                }
            }

            var order = new Order
            {
                UserId = User.Identity?.IsAuthenticated == true ? User.Identity.Name : null,
                CustomerName = model.CustomerName,
                CustomerEmail = model.CustomerEmail,
                CustomerPhone = model.CustomerPhone,
                ShippingAddress = $"{model.ShippingAddress}, {model.City}, {model.PostalCode}, {model.Country}",
                TotalAmount = model.GrandTotal,
                OrderDate = DateTime.Now,
                Status = "Pending"
            };

            var orderItems = cart.Items.Select(item => new OrderItem
            {
                ArtworkId = item.ArtworkId,
                Price = item.Price,
                Quantity = item.Quantity
            }).ToList();

            try
            {
                Console.WriteLine("Saving order to database...");

                var orderId = await _orderRepo.CreateOrderWithItemsAsync(order, orderItems);

                Console.WriteLine($"Order created with ID: {orderId}");

                // Mark artworks as sold
                foreach (var item in cart.Items)
                {
                    var artwork = await _artworkRepo.GetByIdAsync(item.ArtworkId);
                    if (artwork != null)
                    {
                        artwork.IsAvailable = false;
                        await _artworkRepo.UpdateAsync(artwork);
                    }
                }

                // Clear cart
                HttpContext.Session.Remove(CartSessionKey);

                Console.WriteLine("Order completed successfully");

                TempData["SuccessMessage"] = "Your order has been placed successfully!";
                return RedirectToAction("Confirmation", new { id = orderId });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error placing order: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");

                TempData["ErrorMessage"] = $"Failed to place order: {ex.Message}";
                return View("Index", model);
            }
        }

        public async Task<IActionResult> Confirmation(int id)
        {
            var order = await _orderRepo.GetWithItemsAsync(id);

            if (order == null)
            {
                TempData["ErrorMessage"] = "Order not found.";
                return RedirectToAction("Index", "Home");
            }

            return View(order);
        }
    }
}
