using MediatR;
using myn_graphql_sample.Entities;

namespace myn_graphql_sample.Data.Requests.Commands
{
    //public record AddUserCommand(User input) : IRequest<User>;
    public class AddUserCommand : IRequest<User>
    {
        public User Input { get; }

        public AddUserCommand(User input)
        {
            Input = input;
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
            await _context.SaveChangesAsync(cancellationToken);

            return request.Input;
        }
    }

    public class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly AppDbContext _context;

        public TransactionBehavior(AppDbContext context)
        {
            _context = context;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync(cancellationToken))
            {
                try
                {
                    var response = await next();
                    await _context.SaveChangesAsync(cancellationToken);
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

        public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

}
