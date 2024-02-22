using MediatR;
using myn_graphql_sample.Data.Requests.Commands;
using myn_graphql_sample.Data.Requests.Queries;
using myn_graphql_sample.Entities;
using myn_graphql_sample.Repositories;

namespace myn_graphql_sample.GraphQL.MutationTypes
{
    public class UserMutations
    {
        private readonly IMediator _mediator;
        public UserMutations(IMediator mediator)
        {
            _mediator = mediator;
        }

        // Adds a new user based on the provided information.
        public async Task<User> AddUserAsync([Service] IMediator mediator, User input)
        {
            return await mediator.Send(new AddUserCommand(input));
        }


        // Updates a user based on the provided information.
        public async Task<User> UpdateUserAsync(int id, string? firstName, string? lastName, string? email, string? address)
        {
           
            User user = new User();

            if (user == null)
            {
                return null; // or handle accordingly
            }

            // Update user properties
            if (firstName != null)
            {
                user.FirstName = firstName;
            }
            if (lastName != null)
            {
                user.LastName = lastName;
            }
            if (email != null)
            {
                user.Email = email;
            }
            if (address != null)
            {
                user.Address = address;
            }

            // Send command to update the user
            return await _mediator.Send(new UpdateUserCommand(user));
        }

        // Updates a user with the specified ID and optional new information.
        //public User UpdateUser([Service] IUserService userService,int id, string? firstName, string? lastName, string? email, string? address)
        //{
        //    User users = new User();
        //    if (id <= 0)
        //    {
        //        return users;
        //    }

        //    User user = userService.GetUserById(id);

        //    if (user == null)
        //    {
        //        return users;
        //    }

        //    if (firstName != null)
        //    {
        //        user.FirstName = firstName;
        //    }
        //    if (lastName != null)
        //    {
        //        user.LastName = lastName;
        //    }
        //    if (email != null)
        //    {
        //        user.Email = email;
        //    }
        //    if (address != null)
        //    {
        //        user.Address = address;
        //    }

        //    return userService.UpdateUser(user);
        //}


        // Deletes a user with the specified ID from the system.
        public bool DeleteUser([Service] IUserService userService,int id)
        {
            return userService.DeleteUser(id);
        }
    }
}
