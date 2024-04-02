using CoffeeShopAPI.Data.dto.Entities;
using CoffeeShopAPI.Data.dto.Models;
using CoffeeShopAPI.Data.dto.Models.Request;
using CoffeeShopAPI.Data.dto.Models.Response;

namespace CoffeeShopAPI.Data.dao;

public class CategoryDao
{
    private readonly DataContext _dataContext;

    public CategoryDao(DataContext dataContext)
    {
        _dataContext = dataContext;
    }
    
    public ResponseModel AddFromModel(CategoryModel categoryModel)
    {
        if (String.IsNullOrEmpty(categoryModel.Name))
        {
            return new ResponseModel()
            {
                Message = "Name is required",
                Success = false
            };
        }
        if (_dataContext.Categories.Any(c => categoryModel.Name == c.Name))
        {
            return new ResponseModel()
            {
                Message = "Category with this name already exists",
                Success = false
            };
        }
        _dataContext.Categories.Add(new Category()
        {
            Id = Guid.NewGuid(),
            Name = categoryModel.Name,
            Description = categoryModel.Description
        });
        _dataContext.SaveChanges();
        return new ResponseModel()
        {
            Message = "Category added successfully",
            Success = true
        };
    }

    public ResponseModel UpdateCategory(String categoryId, CategoryUpdateModel categoryUpdateModel)
    {
        if(String.IsNullOrEmpty(categoryUpdateModel.Name) && String.IsNullOrEmpty(categoryUpdateModel.Description))
        {
            return new ResponseModel()
            {
                Message = "No data to update",
                Success = false
            };
        }
        Category? category = _dataContext.Categories.FirstOrDefault(c => c.Id.ToString() == categoryId);
        if (category == null)
        {
            return new ResponseModel()
            {
                Message = "Category not found",
                Success = false
            };
        }
        if (!String.IsNullOrEmpty(categoryUpdateModel.Name))
        {
            if(_dataContext.Categories.Any(c => c.Name == categoryUpdateModel.Name && c.Id != category.Id))
            {
                return new ResponseModel()
                {
                    Message = "Category with this name already exists",
                    Success = false
                };
            }
            category.Name = categoryUpdateModel.Name;
        }
        if (!String.IsNullOrEmpty(categoryUpdateModel.Description))
        {
            category.Description = categoryUpdateModel.Description;
        }
        _dataContext.SaveChanges();
        return new ResponseModel()
        {
            Message = "Category updated successfully",
            Success = true
        };
    }
    
    public List<Category> GetAll()
    {
        return _dataContext.Categories.ToList();
    }
    
    public Category? GetById(String id)
    {
        return _dataContext.Categories.FirstOrDefault(c => c.Id.ToString() == id);
    }
}