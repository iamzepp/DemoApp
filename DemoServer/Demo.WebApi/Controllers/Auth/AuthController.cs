using System;
using System.Linq;
using Dapper;
using Demo.Models.Models.Auth;
using Demo.Models.Validators;
using Microsoft.AspNetCore.Mvc;
using Demo.WebApi.Common.DbConnection;
using Demo.WebApi.Common.Enums;
using Demo.WebApi.Common.ResultPackage;
using Demo.WebApi.Common.Security;

namespace Demo.WebApi.Controllers.Auth
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMainDbConnection _connection;

        public AuthController(IMainDbConnection connection)
        {
            _connection = connection;
        }

        [HttpGet("GetUserInfoById")]
        public Result<UserModel> GetUserInfoById(long userId)
        {
            try
            {
                var sql = 
                    $@"SELECT  
                            first_name AS {nameof(UserModel.FirstName)},
                            last_name AS {nameof(UserModel.LastName)},
                            email AS {nameof(UserModel.Email)},
                            birth_date AS {nameof(UserModel.BirthDate)},
                            register_date AS {nameof(UserModel.RegisterDate)}
                      FROM public.users 
                      WHERE user_id = @userId;";

                using (_connection)
                {
                    var user = _connection.Query<UserModel>(sql, 
                        new
                        {
                            userId 
                            
                        }).FirstOrDefault();

                    return new Result<UserModel>
                    {
                        Data = user,
                        Message = "Success",
                        ResultCode = (int) ResultCode.Success
                    };
                }
               
            }
            catch (Exception ex)
            {
                return new Result<UserModel>
                {
                    Message = ex.Message,
                    ResultCode = (int) ResultCode.Error
                };
            }
        }
        
        [HttpPost("SignUp")]
        public Result<bool> SignUp(UserRegistrationModel user)
        {
            var validator = new UserRegistrationValidator();
            var result = validator.Validate(user);

            if (!result.IsValid)
            {
                return new Result<bool>
                {
                    Data = false,
                    Message = result.Errors.ToString(),
                    ResultCode = (int) ResultCode.Error
                };
            }
            
            try
            {
                using (_connection)
                {
                    _connection.Open();
                    
                    var sql = 
                        @"SELECT COUNT(*)
                          FROM public.users 
                          WHERE email = @Email;";
                    
                    var dublicateCount = _connection.ExecuteScalar<long>(sql, new { user.Email });

                    if (dublicateCount != 0)
                    {
                        return new Result<bool>
                        {
                            Data = false,
                            Message = "This email already exist.",
                            ResultCode = (int) ResultCode.Success
                        };
                    }
                    
                    sql = 
                        @"INSERT INTO public.users
                       (first_name, last_name, birth_date, email, password_hash, register_date)
	                   VALUES (@FirstName, @LastName, @BirthDate, @Email, @passwordHash, NOW());";
                    
                    var passwordHash = PasswordHasher.Hash(user.Password);
                    
                    _connection.Execute(sql,
                        new
                        {
                            user.FirstName,
                            user.LastName,
                            user.BirthDate,
                            user.Email,
                            passwordHash
                        });

                    return new Result<bool>
                    {
                        Data = true,
                        Message = "Success",
                        ResultCode = (int) ResultCode.Success
                    };
                }
               
            }
            catch (Exception ex)
            {
                return new Result<bool>
                {
                    Data = false,
                    Message = ex.Message,
                    ResultCode = (int) ResultCode.Error
                };
            }
        }
        
        [HttpPost("SignIn")]
        public Result<bool> SignIn(SignInModel model)
        {
            try
            {
                var sql = 
                    @"SELECT 
                             password_hash
                      FROM public.users 
                      WHERE email = @Email;";

                using (_connection)
                {
                    var result = _connection.Query<string>(sql, new { model.Email }).ToList();

                    if (!result.Any())
                    {
                        return new Result<bool>
                        {
                            Data = false,
                            Message = "User don't sign up.",
                            ResultCode = (int) ResultCode.Error
                        };
                    }
                    
                    var isCorrectPassword = PasswordHasher.Verify(model.Password, result.First());
                    
                    if (!isCorrectPassword)
                    {
                        return new Result<bool>
                        {
                            Data = false,
                            Message = "Don't correct password.",
                            ResultCode = (int) ResultCode.Error
                        };
                    }
                    
                    
                    return new Result<bool>
                    {
                        Data = true,
                        Message = "Success",
                        ResultCode = (int) ResultCode.Success
                    };
                }
               
            }
            catch (Exception ex)
            {
                return new Result<bool>
                {
                    Data = false,
                    Message = ex.Message,
                    ResultCode = (int) ResultCode.Error
                };
            }
        }
    }
}