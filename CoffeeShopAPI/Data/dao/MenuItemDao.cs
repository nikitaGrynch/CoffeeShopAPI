using CoffeeShopAPI.Data.dto.Entities;
using CoffeeShopAPI.Data.dto.Models;
using CoffeeShopAPI.Data.dto.Models.Request;
using CoffeeShopAPI.Data.dto.Models.Response;
using Microsoft.EntityFrameworkCore;

namespace CoffeeShopAPI.Data.dao;

public class MenuItemDao
{
    private readonly DataContext _dataContext;

    public MenuItemDao(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public ResponseModel AddFromModel(MenuItemCreationModel menuItemCreationModel)
    {
        if (_dataContext.MenuItems.Any(mi => menuItemCreationModel.Name == mi.Name))
        {
            return new ResponseModel()
            {
                Message = "Menu item with this name already exists",
                Success = false
            };
        }

        if (String.IsNullOrEmpty(menuItemCreationModel.Name))
        {
            return new ResponseModel()
            {
                Message = "Name is required",
                Success = false
            };
        }

        if (String.IsNullOrEmpty(menuItemCreationModel.CategoryId))
        {
            return new ResponseModel()
            {
                Message = "Category is required",
                Success = false
            };
        }

        if (menuItemCreationModel.Price <= 0)
        {
            return new ResponseModel()
            {
                Message = "Price cannot be less than or equal to 0",
                Success = false
            };
        }

        if (menuItemCreationModel.AvailableAdditivesIds.Count > 0)
        {
            foreach (var additiveId in menuItemCreationModel.AvailableAdditivesIds)
            {
                if (!_dataContext.Additives.Any(a => a.Id.ToString() == additiveId))
                {
                    return new ResponseModel()
                    {
                        Message = "Additive with id " + additiveId + " does not exist",
                        Success = false
                    };
                }
            }
        }

        if (!_dataContext.Categories.Any(c => c.Id.ToString() == menuItemCreationModel.CategoryId))
        {
            return new ResponseModel()
            {
                Message = "Category does not exist",
                Success = false
            };
        }

        Guid menuItemId = Guid.NewGuid();
        Category category = _dataContext.Categories.First(c => c.Id.ToString() == menuItemCreationModel.CategoryId);
        foreach (var additive in _dataContext.Additives.ToList())
        {
            foreach (var additiveId in menuItemCreationModel.AvailableAdditivesIds)
            {
                if (additive.Id.ToString() == additiveId)
                {
                    _dataContext.MenuItemAdditives.Add(new MenuItemAdditive()
                    {
                        MenuItemId = menuItemId,
                        AdditiveId = additive.Id
                    });
                }
            }
        }

        _dataContext.MenuItems.Add(new MenuItem()
        {
            Id = menuItemId,
            Name = menuItemCreationModel.Name,
            Description = menuItemCreationModel.Description,
            Price = menuItemCreationModel.Price,
            CategoryId = category.Id
        });
        _dataContext.SaveChanges();
        return new ResponseModel()
        {
            Message = "Menu item added successfully",
            Success = true
        };
    }

    public ResponseModel UpdateMenuItem(String menuItemId, MenuItemUpdateModel menuItemUpdateModel)
    {
        if (String.IsNullOrEmpty(menuItemUpdateModel.Name) &&
            String.IsNullOrEmpty(menuItemUpdateModel.Description) &&
            String.IsNullOrEmpty(menuItemUpdateModel.CategoryId) &&
            menuItemUpdateModel.Price == null &&
            menuItemUpdateModel.AvailableAdditivesIds == null)
        {
            return new ResponseModel()
            {
                Message = "No data to update",
                Success = false
            };
        }

        MenuItem menuItem = _dataContext.MenuItems.FirstOrDefault(mi => mi.Id.ToString() == menuItemId);
        if (menuItem == null)
        {
            return new ResponseModel()
            {
                Message = "Menu item not found",
                Success = false
            };
        }

        if (!String.IsNullOrEmpty(menuItemUpdateModel.Name))
        {
            if (_dataContext.MenuItems.Any(mi => mi.Name == menuItemUpdateModel.Name && mi.Id != menuItem.Id))
            {
                return new ResponseModel()
                {
                    Message = "Menu item with this name already exists",
                    Success = false
                };
            }

            menuItem.Name = menuItemUpdateModel.Name;
        }
        if(!String.IsNullOrEmpty(menuItemUpdateModel.Description))
        {
            menuItem.Description = menuItemUpdateModel.Description;
        }
        if(menuItemUpdateModel.Price != null)
        {
            if(menuItemUpdateModel.Price <= 0)
            {
                return new ResponseModel()
                {
                    Message = "Price cannot be less than or equal to 0",
                    Success = false
                };
            }
            menuItem.Price = menuItemUpdateModel.Price.Value;
        }
        if(!String.IsNullOrEmpty(menuItemUpdateModel.CategoryId))
        {
            if(!_dataContext.Categories.Any(c => c.Id.ToString() == menuItemUpdateModel.CategoryId))
            {
                return new ResponseModel()
                {
                    Message = "Category does not exist",
                    Success = false
                };
            }
            menuItem.CategoryId = Guid.Parse(menuItemUpdateModel.CategoryId);
        }

        if (menuItemUpdateModel.AvailableAdditivesIds != null)
        {
            _dataContext.MenuItemAdditives.RemoveRange(_dataContext.MenuItemAdditives.Where(ma => ma.MenuItemId == menuItem.Id));
            foreach (var additiveId in menuItemUpdateModel.AvailableAdditivesIds)
            {
                if (!_dataContext.Additives.Any(a => a.Id.ToString() == additiveId))
                {
                    return new ResponseModel()
                    {
                        Message = "Additive with id " + additiveId + " does not exist",
                        Success = false
                    };
                }
                _dataContext.MenuItemAdditives.Add(new MenuItemAdditive()
                {
                    MenuItemId = menuItem.Id,
                    AdditiveId = Guid.Parse(additiveId)
                });
            }
        }

        _dataContext.SaveChanges();
        return new ResponseModel()
        {
            Message = "Menu item updated successfully",
            Success = true
        };
    }

    public List<MenuItem> GetAll()
    {
        return _dataContext.MenuItems
            .Include(mi => mi.Category)
            .Include(mi => mi.AvailableAdditives).ThenInclude(menuItemAdditive => menuItemAdditive.Additive)
            .ToList();
    }
    
    public MenuItem? GetById(String id)
    {
        return _dataContext.MenuItems
            .Include(mi => mi.Category)
            .Include(mi => mi.AvailableAdditives).ThenInclude(menuItemAdditive => menuItemAdditive.Additive)
            .FirstOrDefault(m => m.Id.ToString() == id);
    }
    
    public List<MenuItem> GetByCategoryId(String categoryId)
    {
        return _dataContext.MenuItems
            .Include(mi => mi.Category)
            .Include(mi => mi.AvailableAdditives).ThenInclude(menuItemAdditive => menuItemAdditive.Additive)
            .Where(m => m.Category.Id.ToString() == categoryId)
            .ToList();
    }
}