using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using BachelorProject.CustomAttributes;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace BachelorProject.Models
{
    public class IndexViewModel
    {
        public bool HasPassword { get; set; }
        public IList<UserLoginInfo> Logins { get; set; }
        public string PhoneNumber { get; set; }
        public bool TwoFactor { get; set; }
        public bool BrowserRemembered { get; set; }
    }

    public class ManageLoginsViewModel
    {
        public IList<UserLoginInfo> CurrentLogins { get; set; }
        public IList<AuthenticationDescription> OtherLogins { get; set; }
    }

    public class FactorViewModel
    {
        public string Purpose { get; set; }
    }

    public class SetPasswordViewModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "{0}et må inneholde minst {2} tegn.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Nytt passord")]
        public string NewPassword { get; set; }

        
        [DataType(DataType.Password)]
        [Display(Name = "Bekreft passordet")]
        [Compare("NewPassword", ErrorMessage = "Passord og brekreft passord må være like.")]
        public string ConfirmPassword { get; set; }
    }

    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Nåværende passord")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0}et må inneholde minst {2} tegn.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Nytt passord")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Bekreft passordet")]
        [Compare("NewPassword", ErrorMessage = "Passord og brekreft passord må være like.")]
        public string ConfirmPassword { get; set; }
    }

    public class AddPhoneNumberViewModel
    {
        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string Number { get; set; }
    }

    public class VerifyPhoneNumberViewModel
    {
        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
    }

    public class ConfigureTwoFactorViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
    }

    public class EditProfileInfoViewModel
    {
        public static EditProfileInfoViewModel Create(string firstName, string lastName, string phone, string email)
        {
            return new EditProfileInfoViewModel
            {
                FirstName = firstName,
                LastName = lastName,
                Phone = phone,
                Email = email
            };
        }

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

        [EmailAddress]
        public string Email { get; set; }
    }
}