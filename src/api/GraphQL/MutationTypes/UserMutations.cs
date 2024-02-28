using MediatR;
using Myn.GraphQL.Api.Data.Requests.Commands;
using Myn.GraphQL.Api.Entities;

namespace Myn.GraphQL.Api.GraphQL.MutationTypes
{
    public class UserMutations
    {
        // Adds a new user based on the provided information.
        public async Task<User> AddUserAsync([Service] IMediator mediator, User input)
        {
            return await mediator.Send(new AddUserCommand(input));
        }

        // Updates a user based on the provided information.
        public async Task<User> UpdateUserAsync([Service] IMediator mediator, int id, string? firstName, string? lastName, string? email, string? address)
        {
           
            User input = new User();
            // Update user properties
            if (id != null)
            {
                input.Id = id;
            }
            if (firstName != null)
            {
                input.FirstName = firstName;
            }
            if (lastName != null)
            {
                input.LastName = lastName;
            }
            if (email != null)
            {
                input.Email = email;
            }
            if (address != null)
            {
                input.Address = address;
            }

            // Send command to update the user
            return await mediator.Send(new UpdateUserCommand(input));
        }

        public async Task<bool> DeleteUserAsync(int id, [Service] IMediator mediator)
        {
            // Logic to prepare DeleteUserCommand
            var deleteUserCommand = new DeleteUserCommand(id);

            // Use mediator to send the command
            return await mediator.Send(deleteUserCommand);
        }
    }
}
