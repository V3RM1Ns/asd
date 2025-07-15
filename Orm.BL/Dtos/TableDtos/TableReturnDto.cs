// Orm.BL/Dtos/TableDtos/TableReturnDto.cs
using System;

namespace Orm.BL.Dtos.TableDtos;

public class TableReturnDto
{
    public int Id { get; set; }
    public string No { get; set; }
    public bool IsOccupied { get; set; }
    public DateTime? StartTime { get; set; } // Made nullable to match entity's default and potential null values
}