using CoffeeShopAPI.Data.dao;
using CoffeeShopAPI.Data.dto.Entities;
using CoffeeShopAPI.Data.dto.Models;
using CoffeeShopAPI.Data.dto.Models.Request;
using CoffeeShopAPI.Data.dto.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeShopAPI.Controllers;

[Route("[controller]")]
[ApiController]
public class AdditiveController : Controller
{
    private readonly AdditiveDao _additiveDao;

    public AdditiveController(AdditiveDao additiveDao)
    {
        _additiveDao = additiveDao;
    }

    [Route("addAdditive")]
    [HttpPost]
    public IActionResult AddAdditive([FromBody] AdditiveModel additiveModel)
    {
        ResponseModel result = _additiveDao.AddFromModel(additiveModel);
        if (!result.Success)
        {
            return BadRequest(result);
        }
        return Ok(result);
    }
    
    [Route("updateAdditive")]
    [HttpPatch]
    public IActionResult UpdateAdditive(String id, [FromBody] AdditiveUpdateModel additiveUpdateModel)
    {
        ResponseModel result = _additiveDao.UpdateAdditive(id, additiveUpdateModel);
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
        List<Additive> additives = _additiveDao.GetAll();
        return Ok(additives);
    }
    
    [Route("getById")]
    [HttpGet]
    public IActionResult GetById(String id)
    {
        Additive? additive = _additiveDao.GetById(id);
        if(additive == null)
        {
            return NotFound(new ResponseModel()
            {
                Message = "Additive not found",
                Success = false
            });
        }

        return Ok(additive);
    }
}