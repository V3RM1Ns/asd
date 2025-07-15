// Orm.BL/Services/Interfaces/ITableService.cs
using Orm.BL.Dtos.TableDtos;
using System.Collections.Generic;
using System.Threading.Tasks;
using System; // Added for DateTime

namespace Orm.BL.Services.Interfaces;

public interface ITableService
{
    Task AddTable(string tableNo);
    Task<List<TableReturnDto>> GetAllTables();
    Task<TableReturnDto?> GetTableById(int id); // Changed return type to nullable DTO
    Task UpdateTableStatus(int id, bool isOccupied);
    Task<List<TableReturnDto>> GetOccupiedTables();
    Task<List<TableReturnDto>> GetAvailableTables();
}