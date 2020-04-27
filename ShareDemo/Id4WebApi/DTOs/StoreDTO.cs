using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Id4WebApi
{
    public class StoreDTO : IValidatableObject
    {
        [Required]
        public string Id { get; set; }
        [Required(ErrorMessage = "{0}必填")]
        [MaxLength(50, ErrorMessage = "{0}长度超长")]
        public string Name { get; set; }
        [MaxLength(200)]
        public string Address { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Name.ToLower().Trim() == "admin")
                yield return new ValidationResult("Name不能为admin");
            if (!string.IsNullOrWhiteSpace(Address) && Address.Length < 5)
                yield return new ValidationResult("Address不能太短");
        }
    }
}
