using APICatalogo.DTOs;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace APICatalogo.Models;

public class Category
{

    public Category()
    {
        products = new Collection<Product>();
    }

    [Key]
    public int CategoryId { get; set; }

    [Required]
    [StringLength(80)]
    public string? Name { get; set; }
    [Required]
    [StringLength(300)]
    public string? ImgUrl { get; set; }

    [JsonIgnore]
    public ICollection<Product>? products { get; set; }

    internal IEnumerable<CategoryDTO> Select()
    {
        throw new NotImplementedException();
    }
}

