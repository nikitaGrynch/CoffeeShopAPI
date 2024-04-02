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
        
        if(registrationModel.Password.Length < 6)
        {
            return new UserAuthResponseModel
            {
                Message = "Password must be at least 6 characters",
                Success = false
            };
        }
        if (!Regex.IsMatch(registrationModel.Password, @"\d"))
        {
            return new UserAuthResponseModel
            {
                Message = "Password must contain at least one digit",
                Success = false
            };
        }
        if (!Regex.IsMatch(registrationModel.Password, @"[a-z]"))
        {
            return new UserAuthResponseModel
            {
                Message = "Password must contain at least one lower case letter",
                Success = false
            };
        }
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

    public ResponseModel ChangePassword(String userId, ChangeUserPasswordModel changePassModel)
    {
        User user = _dataContext.Users.FirstOrDefault(u => u.Id.ToString() == userId);
        if (user == null)
        {
            return new ResponseModel
            {
                Message = "User does not exist",
                Success = false
            };
        }

        if (String.IsNullOrEmpty(changePassModel.OldPassword))
        {
            return new ResponseModel()
            {
                Message = "Old password is required",
                Success = false
            };
        }
        if (String.IsNullOrEmpty(changePassModel.NewPassword))
        {
            return new ResponseModel()
            {
                Message = "New password is required",
                Success = false
            };
        }
        if(changePassModel.NewPassword.Length < 6)
        {
            return new ResponseModel()
            {
                Message = "Password must be at least 6 characters",
                Success = false
            };
        }
        if (!Regex.IsMatch(changePassModel.NewPassword, @"\d"))
        {
            return new ResponseModel()
            {
                Message = "Password must contain at least one digit",
                Success = false
            };
        }
        if (!Regex.IsMatch(changePassModel.NewPassword, @"[a-z]"))
        {
            return new UserAuthResponseModel
            {
                Message = "Password must contain at least one lower case letter",
                Success = false
            };
        }
        if (!Regex.IsMatch(changePassModel.NewPassword, @"[A-Z]"))
        {
            return new UserAuthResponseModel
            {
                Message = "Password must contain at least one upper case letter",
                Success = false
            };
        }
        if (String.IsNullOrEmpty(changePassModel.ConfirmPassword))
        {
            return new ResponseModel()
            {
                Message = "Confirm password is required",
                Success = false
            };
        }
        if (!_hashService.Verify(changePassModel.OldPassword, user.PasswordHash))
        {
            return new ResponseModel
            {
                Message = "Old password is not valid",
                Success = false
            };
        }
        if(_hashService.Verify(changePassModel.NewPassword, user.PasswordHash))
        {
            return new ResponseModel
            {
                Message = "New password must be different from the old password",
                Success = false
            };
        }
        if (changePassModel.NewPassword != changePassModel.ConfirmPassword)
        {
            return new ResponseModel
            {
                Message = "New password and confirm password do not match",
                Success = false
            };
        }
        user.PasswordHash = _hashService.Hash(changePassModel.NewPassword);
        _dataContext.SaveChanges();
        return new ResponseModel
        {
            Message = "Password changed successfully",
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