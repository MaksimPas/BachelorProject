using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace BachelorProject.CustomAttributes
{
    //these attributes work correctly if action view is returned along with passed model upon validation failure
    //then error messages is displayed by ValidationSummary()


    //validate if phone number contains 8 digits
    public class NorwegianPhoneNumber : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            //maybe "1" is not allowed in norwegian phone numbers? if true then [2-9]
            var regex = @"^(0047|\+47|47)?[1-9]\d{7}$";
            if (value != null)
            {
                if (!Regex.Match((string)value, regex, RegexOptions.None).Success)
                {
                    return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
                }
            }

            return ValidationResult.Success;
        }
    }

    //validate if property contains a positive number
    public class NotNegativeNumber : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                if (((int)value) <= 0)
                {
                    return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
                }
            }

            return ValidationResult.Success;
        }
    }

    //validate if property contains only ASCII letters
    public class Name : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                if (!Regex.IsMatch((string)value, @"^[a-zA-Z]+$"))
                {
                    return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
                }
            }

            return ValidationResult.Success;
        }
    }
}