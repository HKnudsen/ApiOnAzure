using ApiOnAzure.Data;
using ApiOnAzure.Dtos;
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
            sqlParameters.Add("@PriceParameter", order.Price, DbType.Decimal);

            if (_dataContext.ExecuteSqlWithParameters(sql, sqlParameters))
            {
                return Ok();
            }
            throw new Exception("Failed to Upsert Order");
        }

        [HttpGet("GetOrders/{orderId}")]
        public IEnumerable<Order> GetOrders(int orderId)
        {
            string sql = "EXEC ApiOnAzureSchema.spGet_Orders";
            DynamicParameters sqlParameters = new DynamicParameters();

            if (orderId != 0)
            {
                sql += " @OrderId = @OrderIdParameter";
                sqlParameters.Add("@OrderIdParameter", orderId, DbType.Int32);
            }

            return _dataContext.LoadDataWithParameters<Order>(sql, sqlParameters);
        }

        [HttpGet("GetOrdersForUserId/{userId}")]
        public IEnumerable<OrderForUserDto> GetOrdersForUser(int userId)
        {
            string sql = "EXEC ApiOnAzureSchema.spGet_Orders_For_User @UserId = @UserIdParameter";
            DynamicParameters sqlParameters = new DynamicParameters();
            sqlParameters.Add("@UserIdParameter", userId, DbType.Int32);

            return _dataContext.LoadDataWithParameters<OrderForUserDto>(sql, sqlParameters);
        }

        [HttpGet("GetOrdersForStaffId/{staffId}")]
        public IEnumerable<OrderForStaffDto> GetOrdersForStaff(int staffId)
        {
            string sql = "EXEC ApiOnAzureSchema.spGet_Orders_For_Staff @StaffId = @StaffIdParameter";
            DynamicParameters sqlParameters = new DynamicParameters();
            sqlParameters.Add("@StaffIdParameter", staffId, DbType.Int32);

            return _dataContext.LoadDataWithParameters<OrderForStaffDto>(sql, sqlParameters);
        }

        [HttpDelete("DeleteOrder/{orderId}")]
        public IActionResult DeleteOrder(int orderId)
        {
            string sql = "EXEC ApiOnAzureSchema.spDelete_Order @OrderId = @OrderIdParameter";
            DynamicParameters sqlParameters = new DynamicParameters();
            sqlParameters.Add("@OrderIdParameter", orderId, DbType.Int32);

            if (_dataContext.ExecuteSqlWithParameters(sql, sqlParameters))
            {
                return Ok();
            }
            throw new Exception("Failed to Delete Order");
        }
    }

}