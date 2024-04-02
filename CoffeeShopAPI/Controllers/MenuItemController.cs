using CoffeeShopAPI.Data.dao;
using CoffeeShopAPI.Data.dto.Entities;
using CoffeeShopAPI.Data.dto.Models;
using CoffeeShopAPI.Data.dto.Models.Request;
using CoffeeShopAPI.Data.dto.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeShopAPI.Controllers;

[Route("[controller]")]
[ApiController]
public class MenuItemController : Controller
{
    private readonly MenuItemDao _menuItemDao;

    public MenuItemController(MenuItemDao menuItemDao)
    {
        _menuItemDao = menuItemDao;
    }

    [Route("addItem")]
    [HttpPost]
    public IActionResult AddMenuItem([FromBody] MenuItemCreationModel menuItemCreationModel)
    {
        ResponseModel result = _menuItemDao.AddFromModel(menuItemCreationModel);
        if (!result.Success)
        {
            return BadRequest(result);
        }
        return Ok(result);
    }
    
    [Route("updateItem")]
    [HttpPatch]
    public IActionResult UpdateMenuItem(String id, [FromBody] MenuItemUpdateModel menuItemUpdateModel)
    {
        ResponseModel result = _menuItemDao.UpdateMenuItem(id, menuItemUpdateModel);
        if (!result.Success)
        {
            return BadRequest(result);
        }
        return Ok(result);
    }
    
    [Route("getAll")]
    [HttpGet]
    public IActionResult GetAll()
    {
        List<MenuItemModel> menuItemResponseModels = new();
        List<MenuItem> menuItems = _menuItemDao.GetAll();
        foreach (var menuItem in menuItems)
        {
            menuItemResponseModels.Add(new MenuItemModel(menuItem));
        }

        return Ok(menuItemResponseModels);
    }
    
    [Route("getById")]
    [HttpGet]
    public IActionResult GetById(String id)
    {
        MenuItem? menuItem = _menuItemDao.GetById(id);
        if(menuItem == null)
        {
            return NotFound(new ResponseModel()
            {
                Message = "MenuItem not found",
                Success = false
            });
        }
        return Ok(new MenuItemModel(menuItem));
    }

    [Route("getByCategoryId")]
    [HttpGet]
    public IActionResult GetByCategoryId(String categoryId)
    {
        List<MenuItemModel> menuItemResponseModels = new();
        List<MenuItem> menuItems = _menuItemDao.GetByCategoryId(categoryId);
        foreach (var menuItem in menuItems)
        {
            menuItemResponseModels.Add(new MenuItemModel(menuItem));
        }

        return Ok(menuItemResponseModels);
    }
}