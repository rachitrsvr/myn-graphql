using myn_graphql_sample.Entities;

namespace myn_graphql_sample.Repositories
{
    public interface IUserService
    {
        // Retrieves a list of all users from the system.
        List<User> GetAllUsers();
        // Retrieves a user from the system based on the specified ID.
        User GetUserById(int id);

        // Adds a new user to the system.
        Task<User> AddUser(User user);

        // Updates the information of an existing user in the system.
        Task<User> UpdateUser(User user);

        // Deletes a user with the specified ID from the system.
        Task<bool> DeleteUser(int id);
    }
}
