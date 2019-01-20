using Logik;
using Logik.Languages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Models
{
 

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
        [Display(Name = "Code")]
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

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Display(Name = nameof(Resources.Picture), ResourceType = typeof(Resources))]
        public byte[] Picture { get; set; }

        [Display(Name = nameof(Resources.Description), ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceType = typeof(Resources),
            ErrorMessageResourceName = nameof(Resources.Required))]
        [StringLength(500, MinimumLength = 3, ErrorMessageResourceType = typeof(Resources),
             ErrorMessageResourceName = nameof(Resources.Length))]


        public string Description { get; set; }

        public List<Interests> Interests { get; set; }

        [Display(Name = nameof(Resources.Name), ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceType = typeof(Resources),
           ErrorMessageResourceName = nameof(Resources.Required))]
        [StringLength(60, MinimumLength = 3, ErrorMessageResourceType = typeof(Resources),
                   ErrorMessageResourceName = nameof(Resources.Length))]
        public string Name { get; set; }

        [Display(Name = nameof(Resources.BirthDate), ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceType = typeof(Resources),
            ErrorMessageResourceName = nameof(Resources.Required))]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Birthdate { get; set; }

        [Display(Name = nameof(Resources.City), ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceType = typeof(Resources),
     ErrorMessageResourceName = nameof(Resources.Required))]
        [StringLength(60, MinimumLength = 3, ErrorMessageResourceType = typeof(Resources),
             ErrorMessageResourceName = nameof(Resources.Length))]
        public string City { get; set; }

        [Display(Name = nameof(Resources.Gender), ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceType = typeof(Resources),
     ErrorMessageResourceName = nameof(Resources.Required))]
        public string Genders { get; set; }


        [EmailAddress(ErrorMessageResourceType = typeof(Resources),
                     ErrorMessageResourceName = nameof(Resources.EmailWrong))]
        [Display(Name = nameof(Resources.Email), ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceType = typeof(Resources),
             ErrorMessageResourceName = nameof(Resources.Required))]
        public string Email { get; set; }

        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{6,}$", ErrorMessageResourceType = typeof(Resources),
             ErrorMessageResourceName = nameof(Resources.PasswordChar))]
        [Display(Name = nameof(Resources.Password), ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceType = typeof(Resources),
ErrorMessageResourceName = nameof(Resources.Required))]
        [StringLength(60, MinimumLength = 6, ErrorMessageResourceType = typeof(Resources),
     ErrorMessageResourceName = nameof(Resources.PasswordLength))]
        [DataType(DataType.Password)]
        public string Password { get; set; }


        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessageResourceType = typeof(Resources),
     ErrorMessageResourceName = nameof(Resources.PasswordSame))]
        [Display(Name = nameof(Resources.ConfirmPassword), ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceType = typeof(Resources),
     ErrorMessageResourceName = nameof(Resources.Required))]
        [StringLength(60, MinimumLength = 6, ErrorMessageResourceType = typeof(Resources),
             ErrorMessageResourceName = nameof(Resources.PasswordLength))]
        public string ConfirmPassword { get; set; }
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
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
