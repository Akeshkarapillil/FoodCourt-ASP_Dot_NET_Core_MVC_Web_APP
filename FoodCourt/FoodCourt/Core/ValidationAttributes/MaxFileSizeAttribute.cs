namespace FoodCourt.Core.ValidationAttributes
{
    public class MaxFileSizeAttribute: ValidationAttribute
    {
        private readonly int _maxFileSize;

        public MaxFileSizeAttribute(int maxFileSize)
        {
            _maxFileSize = maxFileSize;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var files = value as IEnumerable<IFormFile>;

            if (files != null)
            {
                foreach (var file in files)
                {
                    if (file.Length > _maxFileSize)
                    {
                        return new ValidationResult(GetErrorMessage());
                    }
                }
            }

            return ValidationResult.Success;
        }

        private string GetErrorMessage()
        {
            return $"The file size cannot exceed {_maxFileSize / 1024 / 1024} MB.";
        }
    }
}
