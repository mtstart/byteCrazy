using System.ComponentModel.DataAnnotations;

namespace byteCrazy.Models
{
    public class RegisterViewModel
    {
        [Required]
        [StringLength(7, ErrorMessage = "The student number must be 7 digits long.", MinimumLength = 7)]
        [RegularExpression(@"\d{7}", ErrorMessage = "The student number must be exactly 7 digits.")]
        [Display(Name = "Student Number")]
        public string StudentNumber { get; set; }
        public var email = StudentNumber + "@uon.edu.au";


        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Hometown")]
        public string Hometown { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "The password must be at least 6 characters long.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}