//namespace ContosoUniversity.Services
//{
//    using System;
//    using System.Threading.Tasks;
//    using Microsoft.AspNetCore.Mvc.Filters;
//    using Models;

//    public class MvcTransactionFilter : IAsyncActionFilter
//    {
//        private readonly SchoolContext _dbContext;

//        public MvcTransactionFilter(SchoolContext dbContext)
//        {
//            _dbContext = dbContext;
//        }

//        public async Task OnActionExecutionAsync(
//            ActionExecutingContext context,
//            ActionExecutionDelegate next)
//        {
//            try
//            {
//                await _dbContext.BeginTransaction();

//                await next();

//                await _dbContext.CloseTransaction();
//            }
//            catch (Exception e)
//            {
//                await _dbContext.CloseTransaction(e);

//                throw;
//            }
//        }
//    }
//}