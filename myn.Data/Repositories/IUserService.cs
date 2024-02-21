using myn.Data.Entities;

namespace myn.Data.Repositories
{
    public interface IUserService
    {
        // Retrieves a list of all users from the system.
        List<User> GetAllUsers();
        // Retrieves a user from the system based on the specified ID.
        User GetUserById(int id);

        // Adds a new user to the system.
        User AddUser(User user);

        // Updates the information of an existing user in the system.
        User UpdateUser(User user);

        // Deletes a user with the specified ID from the system.
        bool DeleteUser(int id);
    }
}
