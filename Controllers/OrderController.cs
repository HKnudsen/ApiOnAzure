using ApiOnAzure.Data;
using ApiOnAzure.Models;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace ApiOnAzure.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersController: ControllerBase
    {
        private readonly DataContext _dataContext;

        public OrdersController(IConfiguration _config)
        {
            _dataContext = new DataContext(_config);
        }

        [HttpPut("UpsertOrder")]
        public IActionResult UpsertOrder(Order order)
        {
            string sql = @"EXEC ApiOnAzureSchema.spUpsert_Order
                @UserId = @UserIdParameter,
                @StaffId = @StaffIdParameter,
                @Treatment = @TreatmentParameter,
                @Price = @PriceParameter
            ";

            DynamicParameters sqlParameters = new DynamicParameters();
            sqlParameters.Add("@UserIdParameter", order.UserId, DbType.Int32);
            sqlParameters.Add("@StaffIdParameter", order.StaffId, DbType.Int32);
            sqlParameters.Add("@TreatmentParameter", order.Treatment, DbType.String);
            sqlParameters.Add("@PriceParemeter", order.Price, DbType.Decimal);

            if (_dataContext.ExecuteSqlWithParameters(sql, sqlParameters))
            {
                return Ok();
            }
            throw new Exception("Failed to Upsert Order");
        }
    }

}