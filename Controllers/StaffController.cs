

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

        [HttpGet("GetStaff/{staffId}")]
        public IEnumerable<Staff> GetStaff(int staffId)
        {
            string sql = "EXEC ApiOnAzureSchema.spGet_Staff";
            DynamicParameters sqlParameter = new DynamicParameters();

            if (staffId != 0)
            {
                sql += "@StaffId = @StaffIdParameter";
                sqlParameter.Add("@StaffIdParameter", staffId, DbType.Int32);
            }

            return _dataContext.LoadDataWithParameters<Staff>(sql, sqlParameter);
        }

        [HttpDelete("DeleteStaff/{staffId}")]
        public IActionResult DeleteStaff(int staffId)
        {
            string sql = @"EXEC AppOnAzureSchema.spStaff_Delete
                @StaffId = @StaffIdParameter  
            ";

            DynamicParameters sqlParameters = new DynamicParameters();
            sqlParameters.Add("@StaffIdParameter", staffId, DbType.Int32);

            if(_dataContext.ExecuteSqlWithParameters(sql, sqlParameters))
            {
                return Ok();
            }
            throw new Exception("Failed to delete Staff");
        }
    }
}