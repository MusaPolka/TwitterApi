using System.ComponentModel.DataAnnotations;

namespace TwitterAPI.Common
{
    public class CustomValidation
    {
        public sealed class CheckFormat : ValidationAttribute
        {
            private readonly string[] _formats;

            public CheckFormat(string[] formats)
            {
                _formats = formats;
            }
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                var file = value as IFormFile;

                string fileExt = Path.GetExtension(file.FileName);

                if (_formats.Contains(fileExt))
                {
                    return ValidationResult.Success;
                }
                else
                {
                    var validFormats = string.Join(" ", _formats);

                    return new ValidationResult($"You can only upload images with {validFormats} extensions");
                }
            }
        }
    }
}
