using Dapper;
using Npgsql;
using Shared.Dtos;
using System.Data;

namespace Services.Discount.Services
{
    public class DiscountService : IDiscountService
    {
        private readonly IDbConnection _dbConnection;

        public DiscountService(IConfiguration configuration)
        {
            _dbConnection = new NpgsqlConnection(configuration.GetConnectionString("PostgreSql"));
        }

        public async Task<Response<NoContent>> Delete(int id)
        {
            var status = await _dbConnection.ExecuteAsync("delete from discount where id=@Id", new { Id = id });
            return DiscountNotFound(status);
        }

        public async Task<Response<List<Models.Discount>>> GetAll()
        {
            var discounts = await _dbConnection.QueryAsync<Models.Discount>("Select * from discount");
            return Response<List<Models.Discount>>.Success(discounts.ToList(), 200);
        }

        public async Task<Response<Models.Discount>> GetByCodeAndUserId(string code, string userId)
        {
            var discounts = await _dbConnection.QueryAsync<Models.Discount>("Select * from discount where code=@Code", new { Code = code });
            //var discounts = await _dbConnection.QueryAsync<Models.Discount>("Select * from discount where userid=@UserId and code=@Code", new { UserId = userId, Code = code });
            var hasDiscount = discounts.FirstOrDefault();
            return DiscountNotFound(hasDiscount);
        }

        public async Task<Response<Models.Discount>> GetById(int id)
        {
            var discount = (await _dbConnection.QueryAsync<Models.Discount>("Select * from discount where id=@Id", new { Id = id })).SingleOrDefault();
            return DiscountNotFound(discount);
        }

        public async Task<Response<NoContent>> Save(Models.Discount discount)
        {
            var insertStatus = await _dbConnection.ExecuteAsync("Insert into discount (userid,rate,code) values (@UserId,@Rate,@Code)", discount);
            return DiscountNotFound(insertStatus);
        }

        public async Task<Response<NoContent>> Update(Models.Discount discount)
        {
            var saveStatus = await _dbConnection.ExecuteAsync("Update discount set userid=@UserId, code=@Code, rate=@Rate where id=@Id", discount);
            return DiscountNotFound(saveStatus);
        }

        private Response<NoContent> DiscountNotFound(int? status)
        {
            return status > 0
              ? Response<NoContent>.Success(204)
              : Response<NoContent>.Fail("Discount not found.", 404);
        }

        private Response<Models.Discount> DiscountNotFound(Models.Discount discount)
        {
            return discount != null
              ? Response<Models.Discount>.Success(discount, 200)
              : Response<Models.Discount>.Fail("Discount not found.", 404);
        }
    }
}
