using myn_graphql_sample.Data;
using myn_graphql_sample.Entities;

namespace myn_graphql_sample.Repositories
{
    public class UserService:IUserService
    {
        AppDbContext _context;

        public UserService(AppDbContext contex)
        {
            _context = contex;
        }


        // Retrieves a list of all users from the system.
        public List<User> GetAllUsers()
        {

            var list = _context.Users.ToList();
            return list;

        }
        public User GetUserById(int id)
        {
            return _context.Users.FirstOrDefault(t => t.Id == id);
        }
        // Retrieves an IQueryable collection of users from the system.
        public User AddUser(User user)
        {

            _context.Users.Add(user);
            _context.SaveChanges();

            // Get the last inserted user
            var lastUser = _context.Users.OrderByDescending(u => u.Id).FirstOrDefault();
            return lastUser;
        }

        // Updates the information of an existing user in the system.
        public User UpdateUser(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
            return _context.Users.FirstOrDefault(t => t.Id == user.Id);

        }

        // Deletes a user with the specified ID from the system.
        public bool DeleteUser(int id)
        {
            var entity = _context.Users.FirstOrDefault(t => t.Id == id);
            _context.Users.Remove(entity);
            _context.SaveChanges();
            return true;

        }
    }
}
