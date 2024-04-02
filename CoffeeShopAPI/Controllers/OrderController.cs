using CoffeeShopAPI.Data.dao;
using CoffeeShopAPI.Data.dto.Entities;
using CoffeeShopAPI.Data.dto.Models;
using CoffeeShopAPI.Data.dto.Models.Request;
using CoffeeShopAPI.Data.dto.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeShopAPI.Controllers;

[Route("[controller]")]
[ApiController]
public class OrderController : Controller
{
    private readonly OrderDao _orderDao;
    private readonly UserDao _userDao;
    private readonly OrderItemDao _orderItemDao;

    public OrderController(OrderDao orderDao, UserDao userDao, OrderItemDao orderItemDao)
    {
        _orderDao = orderDao;
        _userDao = userDao;
        _orderItemDao = orderItemDao;
    }

    [Route("addOrder")]
    [HttpPost]
    public IActionResult AddOrder([FromBody] OrderCreationModel orderModel)
    {
        ResponseModel result = _orderDao.AddOrder(orderModel);
        if(!result.Success)
        {
            return BadRequest(result);
        }
        return Ok(result);
    }
    
    [Route("addOrderItem")]
    [HttpPut]
    public IActionResult AddOrderItem(String orderId, [FromBody] OrderItemCreationModel orderItemModel)
    {
        ResponseModel result = _orderItemDao.Append(orderId, orderItemModel);
        if(!result.Success)
        {
            return BadRequest(result);
        }
        return Ok(result);
    }
    
    [Route("updateOrderItem")]
    [HttpPatch]
    public IActionResult UpdateOrderItem(String orderItemId, [FromBody] OrderItemUpdateModel orderItemUpdateModel)
    {
        ResponseModel result = _orderItemDao.Update(orderItemId, orderItemUpdateModel);
        if(!result.Success)
        {
            return BadRequest(result);
        }
        return Ok(result);
    }
    
    [Route("UpdateOrder")]
    [HttpPatch]
    public IActionResult UpdateOrder(String orderId, [FromBody] OrderUpdateModel orderUpdateModel)
    {
        
        ResponseModel result = _orderDao.UpdateOrder(orderId, orderUpdateModel);
        if(!result.Success)
        {
            return BadRequest(result);
        }
        return Ok(result);
    }
    
    [Route("deleteOrderItem")]
    [HttpDelete]
    public IActionResult DeleteOrderItem(String orderItemId)
    {
        ResponseModel result = _orderItemDao.Delete(orderItemId);
        if(!result.Success)
        {
            return BadRequest(result);
        }
        return Ok(result);
    }
    
    [Route("getAll")]
    [HttpGet]
    public IActionResult GetAll()
    {
        List<OrderModel> orderResponseModels = new();
        List<Order> orders = _orderDao.GetAll();
        foreach (var order in orders)
        {
            Console.Write(order.UserId);
            Console.Write(order.Items.Count);
            orderResponseModels.Add(new OrderModel(order));
        }
        return Ok(orderResponseModels);
    }
    
    [Route("getByStatus")]
    [HttpGet]
    public IActionResult GetByStatus(String status)
    {
        if (!_orderDao.validStatuses.Contains(status))
        {
            return BadRequest(new ResponseModel()
            {
                Success = false,
                Message = "Invalid status"
            });
        }
        List<OrderModel> orderResponseModels = new();
        List<Order> orders = _orderDao.GetByStatus(status);
        foreach (var order in orders)
        {
            orderResponseModels.Add(new OrderModel(order));
        }
        return Ok(orderResponseModels);
    }
    
    [Route("getByUserId")]
    [HttpGet]
    public IActionResult GetByUserId(String userId)
    {
        if (!_userDao.IsUserExistsById(userId))
        {
            return BadRequest(new ResponseModel()
            {
                Success = false,
                Message = "User not found"
            });
        }
        List<OrderModel> orderResponseModels = new();
        List<Order> orders = _orderDao.GetByUserId(userId);
        foreach (var order in orders)
        {
            orderResponseModels.Add(new OrderModel(order));
        }
        return Ok(orderResponseModels);
    }
    
    [Route("completeOrder")]
    [HttpPatch]
    public ResponseModel CompleteOrder(String orderId)
    {
        return _orderDao.UpdateOrderStatus(orderId, "completed");
    }
    
    [Route("cancelOrder")]
    [HttpPatch]
    public ResponseModel CancelOrder(String orderId)
    {
        return _orderDao.UpdateOrderStatus(orderId, "canceled");
    }
}