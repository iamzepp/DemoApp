using System;
using System.Linq;
using Dapper;
using Demo.Models.Models.Auth;
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
                    @"SELECT  
                            first_name AS FirstName,
                            last_name AS LastName,
                            emal AS Email,
                            birth_date AS BirthDate,
                            register_date AS RegisterDate
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
    }
}