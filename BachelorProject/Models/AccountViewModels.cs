using BachelorProject.CustomAttributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BachelorProject.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "E-post")]
        public string Email { get; set; }

        [Required]
        [Name]
        [Display(Name = "Fornavn")]
        public string FirstName { get; set; }

        [Required]
        [Name]
        [Display(Name = "Etternavn")]
        public string LastName { get; set; }

        [Required]
        [NorwegianPhoneNumber(ErrorMessage = "Telefon har ugyldig format")]
        [Display(Name = "Telefon")]
        public string Phone { get; set; }


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
        [Display(Name = "Kode")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Husk denne nettleseren?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "E-post")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "E-post")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Passord")]
        public string Password { get; set; }

        [Display(Name = "Husk meg?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Fornavn")]
        [Name(ErrorMessage = "Fornavnet kan inneholde kun bokstaver")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Etternavn")]
        [Name(ErrorMessage = "Etternavnet kan inneholde kun bokstaver")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "E-post har ugyldig format")]
        [Display(Name = "E-post")]
        public string Email { get; set; }

        [Required]
        [NorwegianPhoneNumber(ErrorMessage = "Telefon har ugyldig format")]
        [Display(Name = "Telefon")]
        public string Phone { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0}et må inneholde minst {2} tegn.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Passord")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Bekreft passordet")]
        [Compare("Password", ErrorMessage = "Passord og brekreft passord må være like.")]
        public string ConfirmPassword { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "E-post")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0}et må inneholde minst {2} tegn.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Passord")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Bekreft passordet")]
        [Compare("Password", ErrorMessage = "Passord og brekreft passord må være like.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "E-post")]
        public string Email { get; set; }
    }
}
