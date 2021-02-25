using System;
using System.ComponentModel.DataAnnotations;

namespace HCAaudit.Service.Portal.AuditUI.ViewModel.ModelValidator
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class DateTimeNotLessThan : ValidationAttribute
    {
        private const string DefaultErrorMessage = "{0} should be greater than {1}.";

        public string OtherProperty { get; private set; }

        public DateTimeNotLessThan(string otherProperty)
            : base(DefaultErrorMessage)
        {
            if (string.IsNullOrEmpty(otherProperty))
            {
                throw new ArgumentNullException("otherProperty");
            }

            OtherProperty = otherProperty;
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(ErrorMessageString, name, OtherProperty);
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var otherProperty = validationContext.ObjectInstance.GetType().GetProperty(OtherProperty);
                var otherPropertyValue = otherProperty.GetValue(validationContext.ObjectInstance, null);

                DateTime dtThis = Convert.ToDateTime(value);
                DateTime dtOther = Convert.ToDateTime(otherPropertyValue);

                if (dtThis <= dtOther)
                {
                    return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
                }
            }
            return ValidationResult.Success;
        }

    }
}
