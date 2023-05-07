using FoodCourt.Core.Types;

namespace FoodCourt.Core.ValidationAttributes
{
    public class ContentTypeAttribute: ValidationAttribute
    {
        private readonly string[] imageType = { "image/jpg", "image/jpeg", "image/pjpeg", "image/gif", "image/x-png", "image/png" };
        private readonly FileType _type;

        public ContentTypeAttribute(FileType type)
        {
            _type = type;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var files = value as IEnumerable<IFormFile>;
            if (files != null)
            {
                foreach (var file in files)
                {
                    Console.WriteLine(file.ContentType.ToLower());
                    if (!imageType.Contains(file.ContentType.ToLower()))
                    {
                        return new ValidationResult(GetErrorMessage());
                    }
                }
            }

            return ValidationResult.Success;
        }

        private string GetErrorMessage()
        {
            return $"This file type is not allowed. Allowed file type is: {_type}.";
        }
    }
}
