using MediatR;
using myn_graphql_sample.Data.Requests.Commands;
using myn_graphql_sample.Entities;

namespace myn_graphql_sample.Data.Handlers.Commands
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, User>
    {
        private readonly AppDbContext _context;

        public UpdateUserCommandHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
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
    }
}
