using MediatR;
using myn_graphql_sample.Data.Requests.Commands;
using myn_graphql_sample.Entities;

namespace myn_graphql_sample.Data.Handlers.Commands
{
    public class AddUserCommandHandler : IRequestHandler<AddUserCommand, User>
    {
        private readonly AppDbContext _context;

        public AddUserCommandHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            _context.Users.Add(request.User);
            await _context.SaveChangesAsync(cancellationToken);

            return request.User;
        }

    }
}
