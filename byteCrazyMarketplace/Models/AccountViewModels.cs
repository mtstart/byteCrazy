using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace byteCrazy.Models
{
    //  Model returned by AccountController actions
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Hometown")]
        public string Hometown { get; set; }
        [Required]
        [Display(Name = "Student Number")]
        public string StudentNumber { get; set; }
    }
    public class UserDetails
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string AdditionalInfo1 { get; set; }
        public string AdditionalInfo2 { get; set; }
        

        public virtual ApplicationUser User { get; set; }
    }
    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }    
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Rember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Hometown")]
        public string Hometown { get; set; }

        [Required]
        [StringLength(9, ErrorMessage = "The phone number must be 9 digits long.", MinimumLength = 9)]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Phone number must be numeric")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
       
        [Display(Name = "User ID")]
        public string UserId { get; set; }

        [Required]
        [Display(Name = "Student Number")]
        public string StudentNumber { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm New Password")]
        [Compare("Password", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; } 
    }

    public class ForgotPasswordViewModel    
    {
        [Required]
        [Display(Name = "Student Number")]
        public string StudentNumber { get; set; }

        [Required]
        [StringLength(9, ErrorMessage = "The phone number must be 9 digits long.", MinimumLength = 9)]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Phone number must be numeric")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
    }
    public class UserCenterViewModel
    {
        public List<Product> PublishedProductsOnSale { get; set; }  // Products on sale
        public List<Product> PublishedProductsSold { get; set; }    // Sold
        public List<Product> PurchasedProducts { get; set; }        // Purchased products
        public List<Product> SavedProducts { get; set; }            // saved
    }
    public class Product
    {
        [Key]
        [StringLength(50)]
        public string ProductID { get; set; }

        [Required]
        [StringLength(50)]
        public string Title { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [StringLength(50)]
        public string CategoryID { get; set; }

        [StringLength(50)]
        public string Location { get; set; }

        [Column(TypeName = "money")]
        public decimal? Price { get; set; }

        [StringLength(500)]
        public string ImgUrl { get; set; }

        [Required]
        [StringLength(50)]
        public string SellerID { get; set; }

        [StringLength(50)]
        public string BuyerID { get; set; }

        [StringLength(50)]
        public string Status { get; set; }

        public DateTime PostedDate { get; set; }

        public DateTime? PurchaseDate { get; set; }
    
    }
    public class Order
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int ProductId { get; set; }

        public virtual Product Product { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
    public class SavedProduct
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(50)]
        public string UserID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string ProductID { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [ForeignKey("UserID")]
        public virtual ApplicationUser User { get; set; }

        [ForeignKey("ProductID")]
        public virtual Product Product { get; set; }
    }

}
