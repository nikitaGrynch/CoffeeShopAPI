using System.Text.RegularExpressions;
using CoffeeShopAPI.Data.dto.Entities;
using CoffeeShopAPI.Data.dto.Models;
using CoffeeShopAPI.Data.dto.Models.Request;
using CoffeeShopAPI.Data.dto.Models.Response;
using CoffeeShopAPI.Services.Hash;

namespace CoffeeShopAPI.Data.dao;

public class UserDao
{
    private readonly DataContext _dataContext;
    private readonly IHashService _hashService;

    public UserDao(DataContext dataContext, IHashService hashService)
    {
        _dataContext = dataContext;
        _hashService = hashService;
    }

    public UserAuthResponseModel RegisterUser(UserRegistrationModel registrationModel)
    {
        // user validation
        if (String.IsNullOrEmpty(registrationModel.PhoneNumber))
        {
            return new UserAuthResponseModel()
            {
                Message = "Phone number is required",
                Success = false
            };
        }
        if(!Regex.IsMatch(registrationModel.PhoneNumber, @"^\+380\d{9}$"))
        {
            return new UserAuthResponseModel
            {
                Message = "Phone number is not valid",
                Success = false
            };
        }
        if (_dataContext.Users.Any(u => u.PhoneNumber == registrationModel.PhoneNumber))
        {
            return new UserAuthResponseModel
            {
                Message = "User with this phone number already exists",
                Success = false
            };
        }

        if (String.IsNullOrEmpty(registrationModel.Name))
        {
            return new UserAuthResponseModel()
            {
                Message = "Name is required",
                Success = false
            };
        }
        if (!Regex.IsMatch(registrationModel.Name, @"^[а-яА-ЯёЁіІїЇєЄa-zA-Z\s]+$"))
        {
            return new UserAuthResponseModel
            {
                Message = "Name is not valid",
                Success = false
            };
        }
        if (String.IsNullOrEmpty(registrationModel.Password))
        {
            return new UserAuthResponseModel()
            {
                Message = "Password is required",
                Success = false
            };
        }
        
        // password must be at least 6 characters
        if(registrationModel.Password.Length < 6)
        {
            return new UserAuthResponseModel
            {
                Message = "Password must be at least 6 characters",
                Success = false
            };
        }
        // password must contain al least one digit
        if (!Regex.IsMatch(registrationModel.Password, @"\d"))
        {
            return new UserAuthResponseModel
            {
                Message = "Password must contain at least one digit",
                Success = false
            };
        }
        // password must contain at least one lower case letter
        if (!Regex.IsMatch(registrationModel.Password, @"[a-z]"))
        {
            return new UserAuthResponseModel
            {
                Message = "Password must contain at least one lower case letter",
                Success = false
            };
        }
        // password must contain at least one upper case letter
        if (!Regex.IsMatch(registrationModel.Password, @"[A-Z]"))
        {
            return new UserAuthResponseModel
            {
                Message = "Password must contain at least one upper case letter",
                Success = false
            };
        }

        if (String.IsNullOrEmpty(registrationModel.PasswordConfirm))
        {
            return new UserAuthResponseModel()
            {
                Message = "Password confirm is required",
                Success = false
            };
        }
        if (registrationModel.Password != registrationModel.PasswordConfirm)
        {
            return new UserAuthResponseModel
            {
                Message = "Password and confirm password do not match",
                Success = false
            };
        }
        
        // create user
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = registrationModel.Name,
            PhoneNumber = registrationModel.PhoneNumber,
            PasswordHash = _hashService.Hash(registrationModel.Password),
            RegisterDt = DateTime.Now
        };
        
        _dataContext.Users.Add(user);
        _dataContext.SaveChanges();
        return new UserAuthResponseModel
        {
            User = user,
            Message = "User registered successfully",
            Success = true
        };
    }

    public UserAuthResponseModel LoginUser(UserLoginModel loginModel)
    {
        var user = _dataContext.Users.FirstOrDefault(u => u.PhoneNumber == loginModel.PhoneNumber);
        if (user == null)
        {
            return new UserAuthResponseModel
            {
                Message = "User with this phone number does not exist",
                Success = false
            };
        }
        if (!_hashService.Verify(loginModel.Password, user.PasswordHash))
        {
            return new UserAuthResponseModel
            {
                Message = "Password is not valid",
                Success = false
            };
        }
        return new UserAuthResponseModel
        {
            User = user,
            Message = "User logged in successfully",
            Success = true
        };
    }
    
    public List<User?> GetAll()
    {
        return _dataContext.Users.ToList();
    }

    public bool IsUserExistsById(string userId)
    {
        return _dataContext.Users.Any(u => u.Id.ToString() == userId);
    }
}