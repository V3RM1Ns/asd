// Orm.BL/Services/Concretes/OrderService.cs
using Microsoft.EntityFrameworkCore;
using Orm.BL.Dtos.OrderDtos;
using Orm.BL.Dtos.OrderItemDtos;
using Orm.BL.Profiles;
using Orm.BL.Services.Interfaces;
using Orm.Core.Entities;
using Orm.DAL.DataStorage.Contexts;
using Orm.DAL.Repositories.Concretes;
using Orm.DAL.Repositories.Interfaces;

namespace Orm.BL.Services.Concretes;

public class OrderService : IOrderService
{
    private readonly AppDbContex _context;
    private readonly IRepository<Order> _orderRepository;
    private readonly IRepository<MenuItem> _menuItemRepository;
    private readonly IRepository<Table> _tableRepository; // Added for Table management

    public OrderService(AppDbContex context)
    {
        _context = context;
        _orderRepository = new Repository<Order>(_context);
        _menuItemRepository = new Repository<MenuItem>(_context);
        _tableRepository = new Repository<Table>(_context); // Initialize table repository
    }

    public async Task AddOrder(OrderCreateDto orderCreateDto)
    {
        var order = OrderProfile.OrderCreateToOrder(orderCreateDto);

        foreach (var orderItem in order.OrderItems)
        {
            var menuItem = await _menuItemRepository.GetByIdAsync(orderItem.MenuItemId);
            if (menuItem == null)
            {
                throw new Exception($"MenuItem with ID {orderItem.MenuItemId} not found.");
            }

            orderItem.TotalAmount = menuItem.Price * orderItem.Quantity;
        }

        order.Count = order.OrderItems.Count;

        // Handle table occupancy when an order is added
        if (order.TableId.HasValue)
        {
            var table = await _tableRepository.GetByIdAsync(order.TableId.Value);
            if (table == null)
            {
                throw new Exception($"Table with ID {order.TableId.Value} not found.");
            }
            if (table.IsOcupied)
            {
                throw new InvalidOperationException($"Table {table.No} is already occupied.");
            }
            table.IsOcupied = true;
            table.StartTime = DateTime.Now; // Set start time when occupied
            await _tableRepository.UpdateAsync(table);
        }

        await _orderRepository.AddAsync(order);
    }

    public async Task UpdateOrder(Order order)
    {
        // This method assumes the Order entity passed in is already tracked or detached and re-attached if needed.
        // If order items can change (added/removed), you might need more complex logic to manage them.
        foreach (var orderItem in order.OrderItems)
        {
            var menuItem = await _menuItemRepository.GetByIdAsync(orderItem.MenuItemId);
            if (menuItem == null)
            {
                throw new Exception($"MenuItem with ID {orderItem.MenuItemId} not found.");
            }
            orderItem.TotalAmount = menuItem.Price * orderItem.Quantity;
        }
        order.Count = order.OrderItems.Count;
        await _orderRepository.UpdateAsync(order);
    }

    public async Task DeleteOrder(Order order)
    {
        // Get the order with its associated table to vacate it
        var orderToDelete = await _context.Orders
                                    .Include(o => o.Table) // Crucial to load Table for vacating logic
                                    .FirstOrDefaultAsync(o => o.Id == order.Id);

        if (orderToDelete == null)
        {
            // Already deleted or not found
            return;
        }

        if (orderToDelete.TableId.HasValue)
        {
            var table = orderToDelete.Table;
            if (table != null) // Double check if table was loaded
            {
                table.IsOcupied = false;
                table.StartTime = null; // Vacate the table and reset start time
                await _tableRepository.UpdateAsync(table); // Update the table status
            }
        }
        await _orderRepository.DeleteAsync(orderToDelete.Id); // Delete the order
    }

    public async Task<List<OrderReturnDto>> GetByPriceRange(decimal min, decimal max)
    {
        var orders = await _context.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.MenuItem)
            .Include(o => o.Table) // Include Table for TableNo mapping
            .ToListAsync();

        var filteredOrders = orders.Where(o => o.OrderItems.Sum(oi => (decimal)oi.TotalAmount) >= min &&
                                               o.OrderItems.Sum(oi => (decimal)oi.TotalAmount) <= max)
            .ToList();

        return filteredOrders.Select(OrderProfile.OrderToOrderReturnDto).ToList();
    }

    public async Task<List<OrderReturnDto>> GetByDateRange(DateTime min, DateTime max)
    {
        var orders = await _context.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.MenuItem)
            .Include(o => o.Table) // Include Table for TableNo mapping
            .Where(x => x.OrderDate >= min && x.OrderDate <= max)
            .ToListAsync();
        return orders.Select(OrderProfile.OrderToOrderReturnDto).ToList();
    }

    public async Task<OrderReturnDto?> GetById(int id) // Changed return type to nullable DTO
    {
        var order = await _context.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.MenuItem)
            .Include(o => o.Table) // Include Table for TableNo mapping
            .FirstOrDefaultAsync(x => x.Id == id);
        return order != null ? OrderProfile.OrderToOrderReturnDto(order) : null;
    }

    public async Task<List<OrderReturnDto>> GetAllOrdersAsync()
    {
        var orders = await _context.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.MenuItem)
            .Include(o => o.Table) // Include Table for TableNo mapping
            .ToListAsync();
        return orders.Select(OrderProfile.OrderToOrderReturnDto).ToList();
    }
}