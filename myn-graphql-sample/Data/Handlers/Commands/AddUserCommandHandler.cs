using System.Threading;
using System.Threading.Tasks;
using MediatR;
using myn_graphql_sample.Data.Requests.Commands;
using myn_graphql_sample.Entities;
using myn_graphql_sample.Data.Context;

namespace myn_graphql_sample.Data.Handlers.Commands
{
    public class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly AppDbContext _context;

        public TransactionBehavior(AppDbContext context)
        {
            _context = context;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var response = await next();
                    await _context.SaveChangesAsync();
                    transaction.Commit();
                    return response;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }

    public class AddUserCommandHandler : IRequestHandler<AddUserCommand, User>
    {
        private readonly AppDbContext _context;

        public AddUserCommandHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            _context.Users.Add(request.Input);
            return request.Input;
        }
    }
}
