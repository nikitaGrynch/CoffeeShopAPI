using CoffeeShopAPI.Data.dao;
using CoffeeShopAPI.Data.dto.Entities;
using CoffeeShopAPI.Data.dto.Models;
using CoffeeShopAPI.Data.dto.Models.Request;
using CoffeeShopAPI.Data.dto.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeShopAPI.Controllers;

[Route("[controller]")]
[ApiController]
public class CategoryController : Controller
{
    private readonly CategoryDao _categoryDao;

    public CategoryController(CategoryDao categoryDao)
    {
        _categoryDao = categoryDao;
    }

    [Route("addCategory")]
    [HttpPost]
    public IActionResult AddCategory([FromBody] CategoryModel categoryModel)
    {
        ResponseModel result =  _categoryDao.AddFromModel(categoryModel);
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
        List<Category> categories = _categoryDao.GetAll();
        return Ok(categories);
    }

    [Route("getById")]
    [HttpGet]
    public IActionResult GetById(String id)
    {
        Category? category = _categoryDao.GetById(id);
        if (category == null)
        {
            return NotFound(new ResponseModel()
            {
                Message = "Category not found",
                Success = false
            });
        }

        return Ok(category);
    }
    
    [Route("updateCategory")]
    [HttpPatch]
    public IActionResult UpdateCategory(String id, [FromBody] CategoryUpdateModel categoryUpdateModel)
    {
        ResponseModel result = _categoryDao.UpdateCategory(id, categoryUpdateModel);
        if (!result.Success)
        {
            return BadRequest(result);
        }
        return Ok(result);
    }
}