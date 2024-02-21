using HotChocolate;
using myn_graphql_sample.Entities;
using myn_graphql_sample.Repositories;

namespace myn_graphql_sample.GraphQL.MutationTypes
{
    public class UserMutations
    {
        // Adds a new user based on the provided information.
        public async Task<User> AddUser([Service] IUserService userService,User input)
        {
            User user = new User();
            user.FirstName = input.FirstName;
            user.LastName = input.LastName;
            user.Address = input.Address;
            return await userService.AddUser(user);
        }

        // Updates a user with the specified ID and optional new information.
        public async Task<User> UpdateUser([Service] IUserService userService,int id, string? firstName, string? lastName, string? email, string? address)
        {
            User users = new User();
            if (id <= 0)
            {
                return users;
            }

            User user = userService.GetUserById(id);

            if (user == null)
            {
                return users;
            }

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

            return await userService.UpdateUser(user);
        }


        // Deletes a user with the specified ID from the system.
        public async Task<bool> DeleteUser([Service] IUserService userService,int id)
        {
            return await userService.DeleteUser(id);
        }
    }
}
