using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace APICatalogo.DTOs
{
    public class ProductDTOResponse
    {
        
        public int ProductId { get; set; }
        
        public string? Name { get; set; }
        
        public string? Description { get; set; }
        
        public decimal Price { get; set; }
        public float Stock { get; set; }
        
        public string? ImgUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        
        public int CategoryId { get; set; }
    }
}
