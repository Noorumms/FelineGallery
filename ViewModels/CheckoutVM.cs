using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Feline_Gallery_v1.Models;

namespace Feline_Gallery_v1.ViewModels
{
    public class CheckoutVM
    {
        [ValidateNever]
        public Cart Cart { get; set; }

        [Required(ErrorMessage = "Full name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters")]
        [Display(Name = "Full Name")]
        public string CustomerName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        [Display(Name = "Email Address")]
        public string CustomerEmail { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [Display(Name = "Phone Number")]
        public string CustomerPhone { get; set; }

        [Required(ErrorMessage = "Shipping address is required")]
        [StringLength(500, MinimumLength = 5, ErrorMessage = "Address must be between 5 and 500 characters")]
        [Display(Name = "Street Address")]
        public string ShippingAddress { get; set; }

        [Required(ErrorMessage = "City is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "City must be between 2 and 100 characters")]
        [Display(Name = "City")]
        public string City { get; set; }

        [Required(ErrorMessage = "Postal code is required")]
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }

        [Required(ErrorMessage = "Country is required")]
        [Display(Name = "Country")]
        public string Country { get; set; }

        [Display(Name = "Payment Method")]
        public string PaymentMethod { get; set; } = "CashOnDelivery";

        public decimal ShippingCost { get; set; } = 0;
        public decimal Tax { get; set; } = 0;

        public decimal Subtotal => Cart?.Items?.Sum(x => x.Price * x.Quantity) ?? 0;
        public decimal GrandTotal => Subtotal + ShippingCost + Tax;
    }
}