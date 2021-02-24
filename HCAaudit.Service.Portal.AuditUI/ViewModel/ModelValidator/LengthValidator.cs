using System;
using System.ComponentModel.DataAnnotations;

namespace HCAaudit.Service.Portal.AuditUI.ViewModel.ModelValidator
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class LengthValidator : ValidationAttribute
    {
        private const string DefaultErrorMessage = "{0} should be of 10 digits.";

        public LengthValidator()
            : base(DefaultErrorMessage)
        {

        }
        public override string FormatErrorMessage(string name)
        {
            return string.Format(ErrorMessageString, name);
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string thisValue = Convert.ToString(value);
            if (string.IsNullOrEmpty(thisValue) && validationContext.DisplayName.ToLower().Equals("npi"))
            {
                return ValidationResult.Success;
            }
            thisValue = thisValue.Trim().Replace(" ", "").Replace("-", "").Replace("(", "").Replace(")", "").Replace(".", "");

            if (thisValue.Length != 10)
            {
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            }
            return ValidationResult.Success;
        }
    }
}
