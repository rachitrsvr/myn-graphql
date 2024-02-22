using MediatR;
using myn_graphql_sample.Data.Requests.Commands;
using myn_graphql_sample.Entities;

namespace myn_graphql_sample.Data.Handlers.Commands
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, bool>
    {
        private readonly AppDbContext _context;

        public DeleteUserCommandHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
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
    }
}
