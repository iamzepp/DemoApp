using System;
using System.Linq;
using Dapper;
using Demo.Models.Models.Auth;
using Demo.WebApi.Common.DbConnection;
using Demo.WebApi.Common.Enums;
using Demo.WebApi.Common.ResultPackage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo.WebApi.Controllers.Administration
{
    [ApiController]
    [Route("[controller]")]
    public class AdministrationController : ControllerBase
    {
        private readonly IMainDbConnection _connection;

        public AdministrationController(IMainDbConnection connection)
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
    }
}