using MediatR;
using Microsoft.EntityFrameworkCore;
using myn_graphql_sample.Data.Requests.Queries;
using myn_graphql_sample.Entities;

namespace myn_graphql_sample.Data.Handlers.Queries
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
            throw new NotImplementedException();
        }
    }
}
