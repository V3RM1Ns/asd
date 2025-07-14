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

    public OrderService(AppDbContex context)
    {
        _context = context;
        _orderRepository = new Repository<Order>(_context);
        _menuItemRepository = new Repository<MenuItem>(_context);
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

        await _orderRepository.AddAsync(order);
    }

    public async Task UpdateOrder(Order order)
    {
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
        await _orderRepository.DeleteAsync(order.Id);
    }

    public async Task<List<OrderReturnDto>> GetByPriceRange(decimal min, decimal max)
    {
        var orders = await _context.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.MenuItem)
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
            .Where(x => x.OrderDate >= min && x.OrderDate <= max)
            .ToListAsync();
        return orders.Select(OrderProfile.OrderToOrderReturnDto).ToList();
    }

    public async Task<OrderReturnDto> GetById(int id)
    {
        var order = await _context.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.MenuItem)
            .FirstOrDefaultAsync(x => x.Id == id);
        return order != null ? OrderProfile.OrderToOrderReturnDto(order) : null;
    }

    public async Task<List<OrderReturnDto>> GetAllOrdersAsync()
    {
        var orders = await _context.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.MenuItem)
            .ToListAsync();
        return orders.Select(OrderProfile.OrderToOrderReturnDto).ToList();
    }
}