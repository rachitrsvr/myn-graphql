using MediatR;
using Microsoft.EntityFrameworkCore;
using Myn.GraphQL.Api.Data.Requests.Queries;
using Myn.GraphQL.Api.Entities;

namespace Myn.GraphQL.Api.Data.Handlers.Queries
{
    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, IEnumerable<User>>
    {
        private readonly AppDbContext _context;

        public GetUsersQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            return await _context.Users.ToListAsync(cancellationToken);
        }
    }

    public class GetUserByIdHandler : IRequestHandler<GetUserById, IEnumerable<User>>
    {
        private readonly AppDbContext _context;

        public GetUserByIdHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> Handle(GetUserById request, CancellationToken cancellationToken)
        {
            var users = await _context.Users.Where(u => u.Id == request.Id).ToListAsync(cancellationToken);
            return users;
        }
    }
}
