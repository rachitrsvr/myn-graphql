using MediatR;
using myn_graphql_sample.Data.Requests.Commands;
using myn_graphql_sample.Entities;
using Microsoft.Extensions.Logging.Console;

namespace myn_graphql_sample.Data.Handlers.Commands
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, User>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<UpdateUserCommand> _logger;

        public UpdateUserCommandHandler(AppDbContext context, ILogger<UpdateUserCommand> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<User?> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Retrieve the existing user from the database
                User existingUser = await _context.Users.FindAsync(request.input.Id);

                if (existingUser == null)
                {
                    // Handle the case where the user is not found
                    return null;
                }

                // Update the properties of the existing user
                existingUser.FirstName = request.input.FirstName;
                existingUser.LastName = request.input.LastName;
                existingUser.Email = request.input.Email;
                existingUser.Address = request.input.Address;

                // Save changes to the database
                await _context.SaveChangesAsync(cancellationToken);

                return existingUser;
            }
            catch (Exception ex)
            {
                //_logger.LogInformation("UpdateUserCommandHandler: Exception" + ex.Message);
                _logger.LogError(ex, "Exception occurred while processing UpdateUserCommand");
                return null;
            }
           
        }
    }
}
