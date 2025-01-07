using APICatalogo.Models;

namespace APICatalogo.DTOs.Mapping;

public static class CategoryDTOMappingExtensions
{
    public static Category ToCategory(this CategoryDTO categoryDTO)
    {

        if (categoryDTO is null)
           return null;

        return new Category
        {
        CategoryId = categoryDTO.CategoryId,
        Name = categoryDTO.Name,
        ImgUrl = categoryDTO.ImgUrl,
        };
    }

    public static CategoryDTO ToCategoryDTO(this Category category)
    {
        if (category is null)
           return null;

        return new CategoryDTO
        {
            CategoryId = category.CategoryId,
            Name = category.Name,
            ImgUrl = category.ImgUrl,
        };
    }

    public static IEnumerable<CategoryDTO> ToCategoryDTOList(this IEnumerable<Category> categories)
    {
        if(categories is null)
        return Enumerable.Empty<CategoryDTO>();

        return categories.Select(category => new CategoryDTO
        {
            CategoryId = category.CategoryId,
            Name = category.Name,
            ImgUrl= category.ImgUrl,
        }).ToList();
    }
}
