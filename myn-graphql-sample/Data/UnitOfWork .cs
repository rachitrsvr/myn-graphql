
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using myn_graphql_sample.Data;
using myn_graphql_sample.Entities;

namespace myn_graphql_sample.Repositories
{
    public class UnitOfWork : DbContext
    {
        private readonly AppDbContext _context;
        private IUserService _userService;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public IUserService UserService
        {
            get
            {
                if (_userService == null)
                {
                    _userService = new UserService(_context);
                }
                return _userService;
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var result = await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return result;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }
    }
}
