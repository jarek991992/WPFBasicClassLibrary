using BasicClassLibrary.BaseClasses;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BasicClassLibrary.Sample
{
    public class ValidationViewModelSampleDelegateRule : NotifyDataErrorInfoDelegateRule<ValidationViewModelSampleDelegateRule>
    {
        private string sampleStringValue;
        public string SampleStringValue
        {
            get => sampleStringValue;
            set => SetProperty(ref sampleStringValue, value);
        }

        private static List<string> propertyNames;
        /// <summary>
        /// Properties names that need to be validated
        /// </summary>
        protected override List<string> PropertyNames { get => propertyNames; set => propertyNames = value; }

        static ValidationViewModelSampleDelegateRule()
        {
            propertyNames = new List<string>() { nameof(SampleStringValue) };
            Rules.Add(new DelegateRule<ValidationViewModelSampleDelegateRule>(nameof(SampleStringValue), "SampleStringValue cannot be empty", x => !string.IsNullOrWhiteSpace(x.SampleStringValue)));
        }
    }

    public class ValidationViewModelSampleDataAnnotations : NotifyDataErrorInfoDataAnnotations
    {
        private static List<string> propertyNames;
        /// <summary>
        /// Properties names that need to be validated
        /// </summary>
        protected override List<string> PropertyNames { get => propertyNames; set => propertyNames = value; }

        private string sampleStringValue;
        [Required(ErrorMessage = "The property {0} cannot be empty")]
        [CustomValidation(typeof(ValidationViewModelSampleDataAnnotationsValidation), nameof(ValidationViewModelSampleDataAnnotationsValidation.SampleStringValueIsNotEmpty))] // same as above
        public string SampleStringValue
        {
            get => sampleStringValue;
            set => SetProperty(ref sampleStringValue, value);
        }

        static ValidationViewModelSampleDataAnnotations()
        {
            propertyNames = new List<string>() { nameof(SampleStringValue) };
        }
    }

    public static class ValidationViewModelSampleDataAnnotationsValidation
    {
        public static ValidationResult SampleStringValueIsNotEmpty(object obj, ValidationContext context)
        {
            var temp = (ValidationViewModelSampleDataAnnotations)context.ObjectInstance;
            if (string.IsNullOrEmpty(temp.SampleStringValue))
            {
                return new ValidationResult("SampleStringValue cannot be empty", new List<string>() { "SampleStringValue" });
            }
            return ValidationResult.Success;
        }

        public static ValidationResult SampleStringValueIsNotEmpty2(string value)
        {
            return string.IsNullOrEmpty(value) ?
                new ValidationResult("SampleStringValue cannot be empty", new List<string>() { "SampleStringValue" }) :
                ValidationResult.Success;
        }
    }
}
