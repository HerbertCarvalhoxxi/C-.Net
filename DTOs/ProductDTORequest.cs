using System.ComponentModel.DataAnnotations;

namespace APICatalogo.DTOs
{
    public class ProductDTORequest : IValidatableObject
    {
        [Range(1, 9999, ErrorMessage = "Estoque deve estar entre 1 e 9999")]
        public float ? Stock {  get; set; }
        public DateTime CreatedAt { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (CreatedAt.Date <= DateTime.Now.Date)
            {
                yield return new ValidationResult("A data deve ser maior que a do cadastro", new[] {nameof(this.CreatedAt)});
            }
        }
    }
}
