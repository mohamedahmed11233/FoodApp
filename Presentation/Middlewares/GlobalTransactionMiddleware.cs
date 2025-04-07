

using Infrastructure.Context;
using Microsoft.EntityFrameworkCore.Storage;

namespace Hotel_Reservation_System.Middleware
{
    public class GlobalTransactionMiddleware : IMiddleware
    {
        private readonly FoodAppDbContext _dbContext;

        public GlobalTransactionMiddleware(FoodAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            IDbContextTransaction Transaction = null!;
            try
            {
                Transaction = _dbContext.Database.BeginTransaction();

                await next(context);
                Transaction.Commit();

            }
            catch (Exception)
            {
                
                Transaction?.Rollback();
                throw;
            }


        }
    }
}
