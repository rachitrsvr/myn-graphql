using MediatR;
using myn_graphql_sample.Data.Requests.Commands;
using myn_graphql_sample.Entities;

namespace myn_graphql_sample.Data.Handlers.Commands
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, bool>
    {
        private readonly AppDbContext _context;
        private readonly ILogger _logger;

        public DeleteUserCommandHandler(AppDbContext context, ILogger<AddUserCommand> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Retrieve the user from the database based on the provided ID
                var userToDelete = await _context.Users.FindAsync(request.Id);

                if (userToDelete == null)
                {
                    // User not found, return false indicating deletion failure
                    return false;
                }

                // Remove the user from the database
                _context.Users.Remove(userToDelete);
                await _context.SaveChangesAsync(cancellationToken);

                // Return true indicating successful deletion
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogInformation("DeleteUserCommandHandler: Exception" + ex.Message);

                return true;
            }
           
        }
    }
}
