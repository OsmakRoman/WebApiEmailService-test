using System.ComponentModel.DataAnnotations;

namespace WebApiEmailService.Models
{
    /// <summary>
    /// Class for incoming JSON data in [FromBody] binding (POST request).
    /// </summary>
    public class PostMessage
    {
        /// <summary>
        /// Field Subject from incoming JSON.
        /// </summary>
        [Required]
        public string Subject { get; set; }
        /// <summary>
        /// Field Body from incoming JSON.
        /// </summary>
        [Required]
        public string Body { get; set; }
        /// <summary>
        /// Field Resipients from incoming JSON (array of strings).
        /// </summary>
        [Required]
        [EmailArrayValidation]
        public string[] Resipients { get; set; }

    }

    /// <summary>
    /// Custom ValidationAttribute class for validating array of strings.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class EmailArrayValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string[] array = value as string[];

            if (array != null)
            {
                if (array.Length == 0)
                    return new ValidationResult("At least one element is required.");

                EmailAddressAttribute emailAttribute = new EmailAddressAttribute();

                foreach (string str in array)
                {
                    if (!emailAttribute.IsValid(str))
                    {
                        return new ValidationResult("At least one element is not valid email address.");
                    }
                }

                return ValidationResult.Success;
            }

            return base.IsValid(value, validationContext);
        }
    }
}

