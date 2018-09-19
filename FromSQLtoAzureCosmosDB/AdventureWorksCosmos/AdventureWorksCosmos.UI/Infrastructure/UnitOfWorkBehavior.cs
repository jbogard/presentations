using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace AdventureWorksCosmos.UI.Infrastructure
{
    public class UnitOfWorkBehavior<TRequest, TResponse> 
        : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UnitOfWorkBehavior(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<TResponse> Handle(
            TRequest request, 
            CancellationToken token, 
            RequestHandlerDelegate<TResponse> next)
        {
            var response = await next();

            await _unitOfWork.Complete();

            return response;
        }
    }
}