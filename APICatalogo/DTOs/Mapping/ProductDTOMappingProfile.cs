using APICatalogo.Models;
using AutoMapper;

namespace APICatalogo.DTOs.Mapping
{
    public class ProductDTOMappingProfile : Profile
    {
        public ProductDTOMappingProfile() 
        { 
            CreateMap<Product, ProductDTO>().ReverseMap();
            CreateMap<Category, CategoryDTO>().ReverseMap();
            CreateMap<Product, ProductDTORequest>().ReverseMap();
            CreateMap<Product, ProductDTOResponse>().ReverseMap();
        }
    }
}
