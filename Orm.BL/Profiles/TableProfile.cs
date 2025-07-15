// Orm.BL/Services/Concretes/TableService.cs
using Microsoft.EntityFrameworkCore;
using Orm.BL.Dtos.TableDtos;
using Orm.BL.Profiles;
using Orm.BL.Services.Interfaces;
using Orm.Core.Entities;
using Orm.DAL.DataStorage.Contexts;
using Orm.DAL.Repositories.Concretes;
using Orm.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orm.BL.Services.Concretes;

public class TableService : ITableService
{
    private readonly IRepository<Table> _tableRepository;
    private readonly AppDbContex _context; // Directly use DbContext for queries like FirstOrDefaultAsync

    public TableService(AppDbContex context)
    {
        _context = context;
        _tableRepository = new Repository<Table>(_context);
    }

    public async Task AddTable(string tableNo)
    {
        if (string.IsNullOrWhiteSpace(tableNo))
        {
            throw new ArgumentException("Table number cannot be empty.", nameof(tableNo));
        }

        var existingTable = await _context.Tables.FirstOrDefaultAsync(t => t.No == tableNo);
        if (existingTable != null)
        {
            throw new InvalidOperationException($"Table with number '{tableNo}' already exists.");
        }

        // Use DateTime.MinValue to indicate not occupied initially
        var table = new Table { No = tableNo, IsOcupied = false, StartTime = DateTime.MinValue };
        await _tableRepository.AddAsync(table);
    }

    public async Task<List<TableReturnDto>> GetAllTables()
    {
        var tables = await _tableRepository.GetAllAsync();
        return tables.Select(TableProfile.TableToTableReturnDto).ToList();
    }

    public async Task<TableReturnDto?> GetTableById(int id)
    {
        var table = await _tableRepository.GetByIdAsync(id);
        return table != null ? TableProfile.TableToTableReturnDto(table) : null;
    }

    public async Task UpdateTableStatus(int id, bool isOccupied)
    {
        var table = await _tableRepository.GetByIdAsync(id);
        if (table == null)
        {
            throw new KeyNotFoundException($"Table with ID {id} not found.");
        }

        table.IsOcupied = isOccupied;
        // Set StartTime when occupied, reset to MinValue when vacated
        table.StartTime = isOccupied ? DateTime.Now : DateTime.MinValue;

        await _tableRepository.UpdateAsync(table);
    }

    public async Task<List<TableReturnDto>> GetOccupiedTables()
    {
        var tables = await _context.Tables.Where(t => t.IsOcupied).ToListAsync();
        return tables.Select(TableProfile.TableToTableReturnDto).ToList();
    }

    public async Task<List<TableReturnDto>> GetAvailableTables()
    {
        var tables = await _context.Tables.Where(t => !t.IsOcupied).ToListAsync();
        return tables.Select(TableProfile.TableToTableReturnDto).ToList();
    }
}