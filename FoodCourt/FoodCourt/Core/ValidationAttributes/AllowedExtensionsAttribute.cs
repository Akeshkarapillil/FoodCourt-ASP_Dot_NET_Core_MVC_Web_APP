﻿namespace FoodCourt.Core.ValidationAttributes
{
    public class AllowedExtensionsAttribute : ValidationAttribute
    {
        private readonly string[] _extensions;

        public AllowedExtensionsAttribute(string[] extensions)
        {
            _extensions = extensions;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var files = value as IEnumerable<IFormFile>;
            if (files != null)
            {
                foreach (var file in files)
                {
                    var extension = Path.GetExtension(file.FileName);
                    if (!_extensions.Contains(extension.ToLower()))
                    {
                        return new ValidationResult(GetErrorMessage());
                    }
                }
            }

            return ValidationResult.Success;
        }

        private string GetErrorMessage()
        {
            return $"This file extension is not allowed. Allowed extensions are: {string.Join(", ", _extensions)}.";
        }
    }
}
