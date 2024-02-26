using MediatR;
using myn_graphql_sample.Data.Requests.Commands;
using myn_graphql_sample.Entities;
using Microsoft.Extensions.Logging.Console;

namespace myn_graphql_sample.Data.Handlers.Commands
{
    public class AddUserCommandHandler : IRequestHandler<AddUserCommand, User>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<AddUserCommandHandler> _logger;

        public AddUserCommandHandler(AppDbContext context, ILogger<AddUserCommandHandler> logger)
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
                //_logger.LogInformation("AddUserCommandHandler: Exception" + ex.Message);
                _logger.LogError(ex, "Exception occurred while processing AddUserCommand");
                throw;
            }
           
        }

    }
}
