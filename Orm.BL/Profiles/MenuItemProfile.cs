using Orm.BL.Dtos.MenuItemDtos;
using Orm.Core.Entities;

namespace Orm.BL.Profiles;

public class MenuItemProfile
{
    public static MenuItem MenuItemCreateToMenuItem(MenuItemCreateDto menuItemCreateDto)
    {
        return new MenuItem
        {
            Category = menuItemCreateDto.Category,
            Price = menuItemCreateDto.Price,
            Name = menuItemCreateDto.Name,
        };
    }

    public static MenuItemCreateDto MenuItemToMenuItemCreate(MenuItem menuItem)
    {
        // This mapping is unusual; typically you map entity to ReturnDto or entity to UpdateDto.
        // Mapping an entity back to a CreateDto might not be the intended use case.
        // If this is for an "Update" scenario, consider a MenuItemUpdateDto.
        return new MenuItemCreateDto
        {
            Category = menuItem.Category,
            Price = menuItem.Price,
            Name = menuItem.Name,
        };
    }

    public static MenuItemReturnDto MenuItemToMenuItemReturnDto(MenuItem menuItem)
    {
        return new MenuItemReturnDto
        {
            Id = menuItem.Id,
            Name = menuItem.Name,
            Price = menuItem.Price,
            Category = menuItem.Category
        };
    }
}