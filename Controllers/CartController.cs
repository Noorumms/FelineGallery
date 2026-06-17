using Feline_Gallery_v1.Data;
using Feline_Gallery_v1.Models;
using Feline_Gallery_v1.Models.Interfaces;
using Feline_Gallery_v1.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Feline_Gallery_v1.Controllers
{
    public class CartController : Controller
    {
        private readonly IArtworkRepository _artworkRepo;
        private const string CartSessionKey = "ShoppingCart";

        public CartController(IArtworkRepository artworkRepo)
        {
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

        private void SaveCart(Cart cart)
        {
            var cartJson = JsonSerializer.Serialize(cart);
            HttpContext.Session.SetString(CartSessionKey, cartJson);
        }

        public IActionResult Index()
        {
            var cart = GetCart();
            var viewModel = new CartVM
            {
                Cart = cart,
                ShippingCost = 0,
                Tax = 0
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int artworkId)
        {
            try
            {
                var artwork = await _artworkRepo.GetByIdAsync(artworkId);

                if (artwork == null)
                {
                    TempData["ErrorMessage"] = "Artwork not found.";
                    return RedirectToAction("Index", "Shop");
                }

                if (!artwork.IsAvailable)
                {
                    TempData["ErrorMessage"] = "This artwork is no longer available.";
                    return RedirectToAction("Index", "Shop");
                }

                var cart = GetCart();
                var existingItem = cart.Items.FirstOrDefault(x => x.ArtworkId == artworkId);

                if (existingItem != null)
                {
                    existingItem.Quantity++;
                    TempData["SuccessMessage"] = $"'{artwork.Title}' quantity updated in cart!";
                }
                else
                {
                    cart.Items.Add(new CartItem
                    {
                        ArtworkId = artwork.ArtworkId,
                        Title = artwork.Title,
                        ImageUrl = artwork.ImageUrl,
                        ArtistName = artwork.Artist?.Name ?? "Unknown Artist",
                        Price = artwork.Price,
                        Quantity = 1
                    });
                    TempData["SuccessMessage"] = $"'{artwork.Title}' added to cart successfully!";
                }

                SaveCart(cart);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Failed to add artwork to cart. Please try again.";
                return RedirectToAction("Index", "Shop");
            }
        }
        [HttpPost]
        public IActionResult RemoveFromCart(int artworkId)
        {
            var cart = GetCart();
            cart.Items.RemoveAll(x => x.ArtworkId == artworkId);
            SaveCart(cart);

            TempData["SuccessMessage"] = "Item removed from cart.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult UpdateQuantity(int artworkId, int quantity)
        {
            var cart = GetCart();
            var item = cart.Items.FirstOrDefault(x => x.ArtworkId == artworkId);

            if (item != null)
            {
                if (quantity <= 0)
                {
                    cart.Items.Remove(item);
                }
                else
                {
                    item.Quantity = quantity;
                }
                SaveCart(cart);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult ClearCart()
        {
            HttpContext.Session.Remove(CartSessionKey);
            TempData["SuccessMessage"] = "Cart cleared.";
            return RedirectToAction("Index");
        }

        public int GetCartCount()
        {
            var cart = GetCart();
            return cart.Items.Sum(x => x.Quantity);
        }
    }
}
