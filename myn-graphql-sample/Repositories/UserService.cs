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

        public async Task<User> AddUser(User user)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    _context.Users.Add(user);

                    // Save changes
                    await _context.SaveChangesAsync();

                    // Commit the transaction if everything is successful
                    await transaction.CommitAsync();

                    // Get the last inserted user
                    var lastUser = _context.Users.OrderByDescending(u => u.Id).FirstOrDefault();
                    return lastUser;
                }
                catch (Exception ex)
                {
                    // Handle exceptions and optionally log them

                    // Rollback the transaction in case of an exception
                    await transaction.RollbackAsync();

                    return null;
                }
            }
        }


        // Updates the information of an existing user in the system.
        public async Task<User> UpdateUser(User user)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    _context.Users.Update(user);

                    // Save changes
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    var lastUser = _context.Users.FirstOrDefault(t => t.Id == user.Id);
                    return lastUser;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return null;
                }
            }
        }

        // Deletes a user with the specified ID from the system.
        public async Task<bool> DeleteUser(int id)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var entity = _context.Users.FirstOrDefault(t => t.Id == id);
                    _context.Users.Remove(entity);
                    await _context.SaveChangesAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return false;
                }
            }
        }
    }
}
