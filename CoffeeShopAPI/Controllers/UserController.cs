using CoffeeShopAPI.Data.dao;
using CoffeeShopAPI.Data.dto.Models;
using CoffeeShopAPI.Data.dto.Models.Request;
using CoffeeShopAPI.Data.dto.Models.Response;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeShopAPI.Controllers;

[Route("[controller]")]
[ApiController]
public class UserController : Controller
{
    private readonly UserDao _userDao;
    
    public UserController(UserDao userDao)
    {
        _userDao = userDao;
    }
    
    [Route("registerUser")]
    [HttpPost]
    public IActionResult RegisterUser([FromBody]UserRegistrationModel? userRegistrationModel)
    {
        if (userRegistrationModel == null)
        {
            return BadRequest(new ResponseModel
            {
                Message = "User registration model is null",
                Success = false
            });
        }

        UserAuthResponseModel res = _userDao.RegisterUser(userRegistrationModel);
        return Ok(res);
    }
    
    [Route("loginUser")]
    [HttpPost]
    public IActionResult LoginUser([FromBody]UserLoginModel? userLoginModel)
    {
        if (userLoginModel == null)
        {
            return BadRequest(new ResponseModel()
            {
                Message = "User login model is null",
                Success = false
            });
        }

        UserAuthResponseModel res = _userDao.LoginUser(userLoginModel);
        return Ok(res);
    }

    [Route("getAll")]
    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(_userDao.GetAll());
    }
    
    [Route("ChangePassword")]
    [HttpPatch]
    public IActionResult ChangePassword(String userId, [FromBody]ChangeUserPasswordModel changePasswordModel)
    {
        ResponseModel res = _userDao.ChangePassword(userId ,changePasswordModel);
        if (!res.Success)
        {
            return BadRequest(res);
        }
        return Ok(res);
    }
    
    
}