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