using Orm.Core.Entities.Common;

namespace Orm.Core.Entities;

public class Table:BaseEntity
{
    public string No { get; set; }
    public List<Order> Orders { get; set; }
    public bool IsOcupied { get; set; } = false;
    public DateTime StartTime { get; set; }
}