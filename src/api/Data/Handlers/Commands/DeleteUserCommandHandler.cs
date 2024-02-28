using MediatR;
using Myn.GraphQL.Api.Data.Requests.Commands;

namespace Myn.GraphQL.Api.Data.Handlers.Commands
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, bool>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<DeleteUserCommandHandler> _logger;

        public DeleteUserCommandHandler(AppDbContext context, ILogger<DeleteUserCommandHandler> logger)
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
                //_logger.LogInformation("DeleteUserCommandHandler: Exception" + ex.Message);
                _logger.LogError(ex, "Exception occurred while processing DeleteUserCommand");
                return true;
            }
           
        }
    }
}
