namespace Orm.BL.Dtos.MenuItemDtos;

public class MenuItemCreateDto
{
    public string Name { get; set; }
    public int Price { get; set; }
    public Category Category { get; set; }
}