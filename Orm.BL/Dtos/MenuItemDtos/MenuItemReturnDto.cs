namespace Orm.BL.Dtos.MenuItemDtos;

public class MenuItemReturnDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Price { get; set; }
    public Category Category { get; set; }
}