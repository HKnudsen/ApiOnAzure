

using ApiOnAzure.Data;
using ApiOnAzure.Models;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace ApiOnAzure.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StaffController: ControllerBase
    {
        private readonly DataContext _dataContext;

        public StaffController(IConfiguration config)
        {
            _dataContext = new DataContext(config);
        }

        [HttpPut("UpsertStaff")]
        public IActionResult UpsertStaff(Staff staff)
        {
            string sql = @"EXEC ApiOnAzureSchema.spUpsert_Staff 
                @FirstName = @FirstNameParameter,
                @LastName = @LastNameParameter,
                @AccountNumber = @AccountNumberParameter
            ";

            DynamicParameters sqlParameter = new DynamicParameters();
            sqlParameter.Add("@FirstNameParameter", staff.FirstName, DbType.String);
            sqlParameter.Add("@LastNameParameter", staff.LastName, DbType.String);
            sqlParameter.Add("@AccountNumberParameter", staff.AccountNumber, DbType.Int32);

            if (_dataContext.ExecuteSqlWithParameters(sql, sqlParameter))
            {
                return Ok();
            }
            throw new Exception("Failed to Upsert Staff");
        }
        
    }
}