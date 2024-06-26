using Microsoft.AspNetCore.Mvc;
using ApiOnAzure.Data;
using ApiOnAzure.Models;
using Dapper;
using System.Data;

namespace ApiOnAzure.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController: ControllerBase
{
    private readonly DataContext _dataContext;

    public UserController(IConfiguration config)
    {
        _dataContext = new DataContext(config);
    }

    [HttpGet("TestConnection")]
    public DateTime TestConnection()
    {
        return _dataContext.LoadDataSingle<DateTime>("SELECT GETDATE()");
    }

    [HttpPut("UserUpsert")]
    public IActionResult UserUpsert(User user)
    {
        string sql = @"
            EXEC ApiOnAzureSchema.spUser_Upsert 
            @Firstname = @FirstnameParam,
            @Lastname = @LastnameParam,
            @Email = @EmailParam,
            @Gender = @GenderParam,
            @UserId = @UserIdParam
        ";

        DynamicParameters sqlParameters = new DynamicParameters();
        sqlParameters.Add("@FirstNameParam", user.FirstName, DbType.String);
        sqlParameters.Add("@LastNameParam", user.LastName, DbType.String);
        sqlParameters.Add("@EmailParam", user.Email, DbType.String);
        sqlParameters.Add("@GenderParam", user.Gender, DbType.String);
        sqlParameters.Add("@UserIdParam", user.UserId, DbType.Int32);

        if (_dataContext.ExecuteSqlWithParameters(sql, sqlParameters))
        {
            return Ok();
        }
        throw new Exception("Failed to Upsert User");
    }

    [HttpGet("GetUsers/{userId}")]
    public IEnumerable<User> GetUsers(int userId)
    {
        string sql = "EXEC ApiOnAzureSchema.spGet_Users";
        DynamicParameters sqlParameters = new DynamicParameters();

        if (userId != 0)
        {
            sql += " @UserId = @UserIdParameter";
            sqlParameters.Add("@UserIdParameter", userId, DbType.Int32);
        }
        return _dataContext.LoadDataWithParameters<User>(sql, sqlParameters);
    }

    [HttpDelete("DeleteUser/{userId}")]
    public IActionResult DeleteUser(int userId)
    {
        string sql = "EXEC ApiOnAzureSchema.DeleteUser @UserId = @UserIdParameter";
        DynamicParameters sqlParameters = new DynamicParameters();
        sqlParameters.Add("UserIdParameter", userId, DbType.Int32);

        if(_dataContext.ExecuteSqlWithParameters(sql, sqlParameters))
        {
            return Ok();
        }
        throw new Exception("Failed to Delete User");
    }
}
