using System.Runtime.InteropServices.JavaScript;
using CoffeeShopAPI.Data.dto.Entities;
using CoffeeShopAPI.Data.dto.Models;
using CoffeeShopAPI.Data.dto.Models.Request;
using CoffeeShopAPI.Data.dto.Models.Response;

namespace CoffeeShopAPI.Data.dao;

public class AdditiveDao
{
    private readonly DataContext _dataContext;

    public AdditiveDao(DataContext dataContext)
    {
        _dataContext = dataContext;
    }
    
    public ResponseModel AddFromModel(AdditiveModel additiveModel)
    {
        if (_dataContext.Additives.Any(a => additiveModel.Name == a.Name))
        {
            return new ResponseModel()
            {
                Message = "Additive with this name already exists",
                Success = false
            };
        }
        if (String.IsNullOrEmpty(additiveModel.Name))
        {
            return new ResponseModel()
            {
                Message = "Name is required",
                Success = false
            };
        }

        if (additiveModel.Price < 0)
        {
            return new ResponseModel()
            {
                Message = "Price cannot be less than 0",
                Success = false
            };
        }

        _dataContext.Additives.Add(new Additive()
        {
            Id = Guid.NewGuid(),
            Name = additiveModel.Name,
            Price = additiveModel.Price
        });
        _dataContext.SaveChanges();
        return new ResponseModel()
        {
            Message = "Additive added successfully",
            Success = true
        };
    }

    public ResponseModel UpdateAdditive(String additiveId, AdditiveUpdateModel additiveUpdateModel)
    {
        if (String.IsNullOrEmpty(additiveUpdateModel.Name) &&
            String.IsNullOrEmpty(additiveUpdateModel.Description) &&
            additiveUpdateModel.Price == null)
        {
            return new ResponseModel()
            {
                Message = "No data to update",
                Success = false
            };
        }
        Additive? additive = _dataContext.Additives.FirstOrDefault(a => a.Id.ToString() == additiveId);
        if (additive == null)
        {
            return new ResponseModel()
            {
                Message = "Additive not found",
                Success = false
            };
        }
        if (!String.IsNullOrEmpty(additiveUpdateModel.Name))
        {
            if (_dataContext.Additives.Any(a => additiveUpdateModel.Name == a.Name && a.Id != additive.Id))
            {
                return new ResponseModel()
                {
                    Message = "Additive with this name already exists",
                    Success = false
                };
            }
            additive.Name = additiveUpdateModel.Name;
        }

        if (additiveUpdateModel.Price != null)
        {
            if (additiveUpdateModel.Price < 0)
            {
                return new ResponseModel()
                {
                    Message = "Price must be greater or equal to 0",
                    Success = false
                };
            }

            additive.Price = additiveUpdateModel.Price.Value;
        }

        if (!String.IsNullOrEmpty(additiveUpdateModel.Description))
        {
            additive.Description = additiveUpdateModel.Description;
        }
        _dataContext.SaveChanges();
        return new ResponseModel()
        {
            Message = "Additive updated successfully",
            Success = true
        };
    }
    
    public List<Additive> GetAll()
    {
        return _dataContext.Additives.ToList();
    }

    public Additive? GetById(String id)
    {
        return _dataContext.Additives.FirstOrDefault(a => a.Id.ToString() == id);
    }
}