using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace HCAaudit.Service.Portal.AuditUI.ViewModel.ModelValidator
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class CompareValidator : ValidationAttribute
    {
        private const string DefaultErrorMessage = "Must provide {0} or {1}.";

        public string OtherProperty { get; private set; }

        public CompareValidator(string otherProperty)
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
            var otherProperty = validationContext.ObjectInstance.GetType().GetProperty(OtherProperty);
            var otherPropertyValue = otherProperty.GetValue(validationContext.ObjectInstance, null);

            string thisValue = Convert.ToString(value);
            string otherValue = Convert.ToString(otherPropertyValue);

            if (string.IsNullOrEmpty(thisValue) && string.IsNullOrEmpty(otherValue))
            {
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            }
            return ValidationResult.Success;
        }
    }
}
