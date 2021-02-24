using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace HCAaudit.Service.Portal.AuditUI.ViewModel.ModelValidator
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class DateTimeValidatorForStartDate : ValidationAttribute
    {
        private const string DefaultErrorMessage = "{0} should be grater than Nov 1, 2017.";

        public DateTimeValidatorForStartDate()
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
                DateTimeFormatInfo dateFormat = new DateTimeFormatInfo();
                dateFormat.ShortDatePattern = "MM/dd/yyyy";
                dateFormat.DateSeparator = "/";
                DateTime compareDate = Convert.ToDateTime("11/1/2017", dateFormat);

                if (dtThis < compareDate)
                {
                    return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
                }
            }
            return ValidationResult.Success;
        }

    }
}
