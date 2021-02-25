using System;
using System.ComponentModel.DataAnnotations;

namespace HCAaudit.Service.Portal.AuditUI.ViewModel.ModelValidator
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class DateLessThanCurrentDate : ValidationAttribute
    {
        private const string DefaultErrorMessage = "{0} should be less than Current Date.";

        public DateLessThanCurrentDate()
            : base(DefaultErrorMessage)
        {
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(ErrorMessageString, name);
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                DateTime dtThis = Convert.ToDateTime(value);

                if (DateTime.Now < dtThis)
                {
                    return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
                }
            }
            return ValidationResult.Success;
        }

    }
}
