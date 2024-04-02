using CoffeeShopAPI.Data.dto.Entities;
using CoffeeShopAPI.Data.dto.Models.Request;
using CoffeeShopAPI.Data.dto.Models.Response;
using Microsoft.EntityFrameworkCore;

namespace CoffeeShopAPI.Data.dao;

public class OrderItemDao
{
    private readonly DataContext _dataContext;

    public OrderItemDao(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public ResponseModel Add(Guid orderId , OrderItemCreationModel orderItemModel)
    {
        if (String.IsNullOrEmpty(orderItemModel.MenuItemId))
        {
            return new ResponseModel()
            {
                Message = "MenuItemId is empty",
                Success = false
            };
        }

        MenuItem menuItem = _dataContext.MenuItems
            .FirstOrDefault(m => m.Id.ToString() == orderItemModel.MenuItemId);
        if (menuItem == null)
        {
            return new ResponseModel()
            {
                Message = "MenuItem not found",
                Success = false
            };
        }

        if (orderItemModel.Quantity <= 0)
        {
            return new ResponseModel()
            {
                Message = "Quantity must be greater than 0",
                Success = false
            };
        }

        Guid orderItemId = Guid.NewGuid();
        if (orderItemModel.AdditivesIds != null && orderItemModel.AdditivesIds.Count > 0)
        {
            foreach (var additiveId in orderItemModel.AdditivesIds)
            {
                Additive additive = _dataContext.Additives
                    .FirstOrDefault(a => a.Id.ToString() == additiveId);
                if (additive == null)
                {
                    return new ResponseModel()
                    {
                        Message = $"Additive with id: '{additiveId}' not found",
                        Success = false
                    };
                }

                if (_dataContext.MenuItemAdditives
                        .FirstOrDefault(a =>
                            a.MenuItemId == menuItem.Id && a.AdditiveId.ToString() == additiveId) == null)
                {
                    return new ResponseModel()
                    {
                        Message = $"Additive with id: '{additiveId}' is not available for this menu item",
                        Success = false
                    };
                }

                _dataContext.OrderItemAdditives.Add(new OrderItemAdditive()
                {
                    AdditiveId = Guid.Parse(additiveId),
                    OrderItemId = orderItemId
                });
            }
        }
        OrderItem orderItem = new OrderItem()
        {
            Id = orderItemId,
            ItemId = menuItem.Id,
            Quantity = orderItemModel.Quantity,
            OrderId = orderId
        };
        _dataContext.OrderItems.Add(orderItem);
        _dataContext.SaveChanges();
        
        return new ResponseModel()
        {
            Message = "OrderItem added",
            Success = true
        };
    }
    
     public ResponseModel Append(String orderId , OrderItemCreationModel orderItemModel)
    {
        Order order = _dataContext.Orders.FirstOrDefault(o => o.Id.ToString() == orderId);
        if (order == null)
        {
            return new ResponseModel()
            {
                Message = "Order not found",
                Success = false
            };
        }
        if (String.IsNullOrEmpty(orderItemModel.MenuItemId))
        {
            return new ResponseModel()
            {
                Message = "MenuItemId is empty",
                Success = false
            };
        }

        MenuItem menuItem = _dataContext.MenuItems
            .FirstOrDefault(m => m.Id.ToString() == orderItemModel.MenuItemId);
        if (menuItem == null)
        {
            return new ResponseModel()
            {
                Message = "MenuItem not found",
                Success = false
            };
        }

        if (orderItemModel.Quantity <= 0)
        {
            return new ResponseModel()
            {
                Message = "Quantity must be greater than 0",
                Success = false
            };
        }

        Guid orderItemId = Guid.NewGuid();
        if (orderItemModel.AdditivesIds != null && orderItemModel.AdditivesIds.Count > 0)
        {
            foreach (var additiveId in orderItemModel.AdditivesIds)
            {
                Additive additive = _dataContext.Additives
                    .FirstOrDefault(a => a.Id.ToString() == additiveId);
                if (additive == null)
                {
                    return new ResponseModel()
                    {
                        Message = $"Additive with id: '{additiveId}' not found",
                        Success = false
                    };
                }

                if (_dataContext.MenuItemAdditives
                        .FirstOrDefault(a =>
                            a.MenuItemId == menuItem.Id && a.AdditiveId.ToString() == additiveId) == null)
                {
                    return new ResponseModel()
                    {
                        Message = $"Additive with id: '{additiveId}' is not available for this menu item",
                        Success = false
                    };
                }

                _dataContext.OrderItemAdditives.Add(new OrderItemAdditive()
                {
                    AdditiveId = Guid.Parse(additiveId),
                    OrderItemId = orderItemId
                });
            }
        }
        OrderItem orderItem = new OrderItem()
        {
            Id = orderItemId,
            ItemId = menuItem.Id,
            Quantity = orderItemModel.Quantity,
            OrderId = order.Id
        };
        _dataContext.OrderItems.Add(orderItem);
        _dataContext.SaveChanges();
        
        return new ResponseModel()
        {
            Message = "OrderItem added",
            Success = true
        };
    }
    
    public ResponseModel Update(String orderItemId, OrderItemUpdateModel orderItemUpdateModel)
    {
        if(orderItemUpdateModel.AdditivesIds == null && orderItemUpdateModel.Quantity == null)
        {
            return new ResponseModel()
            {
                Message = "No data to update",
                Success = false
            };
        }
        OrderItem orderItem = _dataContext.OrderItems
            .FirstOrDefault(oi => oi.Id.ToString() == orderItemId);
        if (orderItem == null)
        {
            return new ResponseModel()
            {
                Message = "OrderItem not found",
                Success = false
            };
        }

        if (orderItemUpdateModel.Quantity != null)
        {
            if (orderItemUpdateModel.Quantity <= 0)
            {
                return new ResponseModel()
                {
                    Message = "Quantity must be greater than 0",
                    Success = false
                };
            }

            orderItem.Quantity = orderItemUpdateModel.Quantity.Value;
        }
        
        if (orderItemUpdateModel.AdditivesIds != null)
        {
            _dataContext.OrderItemAdditives.RemoveRange(_dataContext.OrderItemAdditives
                .Where(oia => oia.OrderItemId.ToString() == orderItemId));
            foreach (var additiveId in orderItemUpdateModel.AdditivesIds)
            {
                Additive additive = _dataContext.Additives
                    .FirstOrDefault(a => a.Id.ToString() == additiveId);
                if (additive == null)
                {
                    return new ResponseModel()
                    {
                        Message = $"Additive with id: '{additiveId}' not found",
                        Success = false
                    };
                }

                MenuItem menuItem = _dataContext.MenuItems.FirstOrDefault(mi => mi.Id == orderItem.ItemId);
                if(menuItem == null)
                {
                    return new ResponseModel()
                    {
                        Message = "MenuItem not found",
                        Success = false
                    };
                }
                if (_dataContext.MenuItemAdditives
                        .FirstOrDefault(a =>
                            a.MenuItemId == menuItem.Id && a.AdditiveId.ToString() == additiveId) == null)
                {
                    return new ResponseModel()
                    {
                        Message = $"Additive with id: '{additiveId}' is not available for this menu item",
                        Success = false
                    };
                }

                _dataContext.OrderItemAdditives.Add(new OrderItemAdditive()
                {
                    AdditiveId = Guid.Parse(additiveId),
                    OrderItemId = orderItem.Id
                });
            }
        }

        _dataContext.SaveChanges();
        return new ResponseModel()
        {
            Message = "OrderItem updated",
            Success = true
        };
    }
    
    public ResponseModel Delete(String orderItemId)
    {
        OrderItem orderItem = _dataContext.OrderItems
            .FirstOrDefault(oi => oi.Id.ToString() == orderItemId);
        if (orderItem == null)
        {
            return new ResponseModel()
            {
                Message = "OrderItem not found",
                Success = false
            };
        }

        _dataContext.OrderItemAdditives.RemoveRange(_dataContext.OrderItemAdditives
            .Where(oia => oia.OrderItemId.ToString() == orderItemId));
        _dataContext.OrderItems.Remove(orderItem);
        if (_dataContext.Orders.Include(o => o.Items).First(o => o.Id == orderItem.OrderId).Items.Count == 0)
        {
            _dataContext.Orders.Remove(_dataContext.Orders.First(o => o.Id == orderItem.OrderId));
        }
        _dataContext.SaveChanges();
        return new ResponseModel()
        {
            Message = "OrderItem deleted",
            Success = true
        };
    }
}