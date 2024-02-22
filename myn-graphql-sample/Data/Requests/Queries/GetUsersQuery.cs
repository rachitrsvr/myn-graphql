using MediatR;
using myn_graphql_sample.Entities;

namespace myn_graphql_sample.Data.Requests.Queries
{
    public class GetUsersQuery : IRequest<IEnumerable<User>> { }
   
}
