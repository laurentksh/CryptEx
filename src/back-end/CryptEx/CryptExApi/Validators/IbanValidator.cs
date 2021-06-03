using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CryptExApi.Validators
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class IbanValidator : ValidationAttribute
    {
        private const string Regex_ = @"/\b[A-Z]{2}[0-9]{2}(?:[ ]?[0-9]{4}){4}(?!(?:[ ]?[0-9]){3})(?:[ ]?[0-9]{1,2})?\b/gm";

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null) //Ignore null values
                return ValidationResult.Success;

            var iban = value as string;

            var result = Regex.IsMatch(iban, Regex_);

            return result ? ValidationResult.Success : new ValidationResult("Invalid IBAN provided.");
        }
    }
}
