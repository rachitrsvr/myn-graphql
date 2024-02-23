using MediatR;
using myn_graphql_sample.Data.Requests.Commands;
using myn_graphql_sample.Entities;

namespace myn_graphql_sample.Data.Handlers.Commands
{
    public class AddUserCommandHandler : IRequestHandler<AddUserCommand, User>
    {
        private readonly AppDbContext _context;
        private readonly ILogger _logger;

        public AddUserCommandHandler(AppDbContext context, ILogger<AddUserCommand> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<User> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _context.Users.Add(request.input);
                _context.SaveChanges();

                return request.input;
            }
            catch (Exception ex)
            {
                _logger.LogError("AddUserCommandHandler: Exception- " + ex.Message,
                DateTime.UtcNow.ToLongTimeString());
                throw;
            }
           
        }

    }
}
