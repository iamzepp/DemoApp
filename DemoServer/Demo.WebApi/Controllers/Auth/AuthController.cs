using System;
using System.Linq;
using Dapper;
using Demo.Models.Models.Auth;
using Demo.Models.Validators;
using Microsoft.AspNetCore.Mvc;
using Demo.WebApi.Common.DbConnection;
using Demo.WebApi.Common.Enums;
using Demo.WebApi.Common.ResultPackage;

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
                      WHERE user_id = @userId";

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
        
        [HttpPost("RegisterUser")]
        public Result<UserRegistrationModel> GetUserInfoById(UserRegistrationModel user)
        {
            var validator = new UserRegistrationValidator();
            var result = validator.Validate(user);

            if (!result.IsValid)
            {
                return new Result<UserRegistrationModel>
                {
                    Message = result.Errors.ToString(),
                    ResultCode = (int) ResultCode.Error
                };
            }
            
            try
            {
                var sql = 
                    @"INSERT INTO public.users
                      (first_name, last_name, birth_date, email, password_hash, register_date)
	                  VALUES (@FIRSTNAME, ?, ?, ?, ?, ?);";

                using (_connection)
                {
                    var count = _connection.Execute(sql);

                    return new Result<UserRegistrationModel>
                    {
                        Message = "Success",
                        ResultCode = (int) ResultCode.Success
                    };
                }
               
            }
            catch (Exception ex)
            {
                return new Result<UserRegistrationModel>
                {
                    Message = ex.Message,
                    ResultCode = (int) ResultCode.Error
                };
            }
        }
        
    }
}