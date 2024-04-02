using CoffeeShopAPI.Data.dto.Entities;
using CoffeeShopAPI.Data.dto.Models.Request;
using CoffeeShopAPI.Data.dto.Models.Response;
using Microsoft.EntityFrameworkCore;

namespace CoffeeShopAPI.Data.dao;

public class OrderDao
{
    private readonly DataContext _dataContext;
    private readonly OrderItemDao _orderItemDao;
    public readonly String[] validStatuses = {"in_process", "completed", "canceled"};

    public OrderDao(DataContext dataContext, OrderItemDao orderItemDao)
    {
        _dataContext = dataContext;
        _orderItemDao = orderItemDao;
    }
    
    public List<Order> GetAll()
    {
        return _dataContext.Orders
            .Include(o => o.Items)
            .ThenInclude(oi => oi.Item)
            .ThenInclude(mi => mi.AvailableAdditives)
            .ThenInclude(mia => mia.Additive)
            .Include(o => o.Items)
            .ThenInclude(oi => oi.Additives)
            .Include(oi => oi.Items)
            .ThenInclude(mi => mi.Item.Category)
            .ToList();
    }

    public List<Order> GetByStatus(string status)
    {
        return _dataContext.Orders
            .Where(o => o.Status == status)
            .Include(o => o.Items)
            .ThenInclude(oi => oi.Item)
            .ThenInclude(mi => mi.AvailableAdditives)
            .ThenInclude(mia => mia.Additive)
            .Include(o => o.Items)
            .ThenInclude(oi => oi.Additives)
            .Include(oi => oi.Items)
            .ThenInclude(mi => mi.Item.Category)
            .ToList();
    }
    
    public List<Order> GetByUserId(string userId)
    {
        return _dataContext.Orders
            .Where(o => o.UserId != null && o.UserId.ToString() == userId)
            .Include(o => o.Items)
            .ThenInclude(oi => oi.Item)
            .ThenInclude(mi => mi.AvailableAdditives)
            .ThenInclude(mia => mia.Additive)
            .Include(o => o.Items)
            .ThenInclude(oi => oi.Additives)
            .Include(oi => oi.Items)
            .ThenInclude(mi => mi.Item.Category)
            .ToList();
    }
    
    public ResponseModel AddOrder(OrderCreationModel orderModel)
    {
        if(orderModel.Items.Count == 0)
        {
            return new ResponseModel()
            {
                Message = "Order is empty",
                Success = false
            };
        }
        Guid orderId = Guid.NewGuid();
        Order order = new Order()
        {
            Id = orderId,
            Comment = orderModel.Comment,
            Items = new List<OrderItem>(),
            Moment = DateTime.Now,
            Status = "in_process",
            UserId = null
        };
        if (!String.IsNullOrEmpty(orderModel.UserId))
        {
            if (!_dataContext.Users.Any(u => u.Id.ToString() == orderModel.UserId))
            {
                return new ResponseModel()
                {
                    Message = "User not found",
                    Success = false
                };
            }
            order.UserId = Guid.Parse(orderModel.UserId);
        }

        foreach (var orderItemModel in orderModel.Items)
        {
            ResponseModel res = _orderItemDao.Add(orderId ,orderItemModel);
            if (!res.Success)
            {
                return res;
            }
        }

        _dataContext.Orders.Add(order);
        _dataContext.SaveChanges();
        return new ResponseModel()
        {
            Message = "Order added",
            Success = true
        };
    }

    public ResponseModel UpdateOrder(String orderId, OrderUpdateModel orderUpdateModel)
    {
        if(orderUpdateModel.UserId == null && orderUpdateModel.Comment == null)
        {
            return new ResponseModel()
            {
                Message = "No data to update",
                Success = false
            };
        }
        Order order = _dataContext.Orders
            .FirstOrDefault(o => o.Id.ToString() == orderId);
        if (order == null)
        {
            return new ResponseModel()
            {
                Message = "Order not found",
                Success = false
            };
        }
        if(order.Status != "in_process")
        {
            return new ResponseModel()
            {
                Message = "Order status is not 'in_process'",
                Success = false
            };
        }
        if (orderUpdateModel.UserId != null)
        {
            if (!_dataContext.Users.Any(u => u.Id.ToString() == orderUpdateModel.UserId))
            {
                return new ResponseModel()
                {
                    Message = "User not found",
                    Success = false
                };
            }
            order.UserId = Guid.Parse(orderUpdateModel.UserId);
        }
        if (!String.IsNullOrEmpty(orderUpdateModel.Comment))
        {
            order.Comment = orderUpdateModel.Comment;
        }
        _dataContext.SaveChanges();
        return new ResponseModel()
        {
            Message = "Order updated successfully",
            Success = true
        };
    }
    
    public ResponseModel UpdateOrderStatus(String orderId, String status)
    {
        Order order = _dataContext.Orders
            .FirstOrDefault(o => o.Id.ToString() == orderId);
        if (order == null)
        {
            return new ResponseModel()
            {
                Message = "Order not found",
                Success = false
            };
        }
        if(!validStatuses.Contains(status))
        {
            return new ResponseModel()
            {
                Message = "Invalid status",
                Success = false
            };
        }
        order.Status = status;
        _dataContext.SaveChanges();
        return new ResponseModel()
        {
            Message = $"Order status updated to '{status}' for order with id: '{orderId}'",
            Success = true
        };
    }
}