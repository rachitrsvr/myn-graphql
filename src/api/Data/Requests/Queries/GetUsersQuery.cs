using MediatR;
using Myn.GraphQL.Api.Entities;

namespace Myn.GraphQL.Api.Data.Requests.Queries
{
    public class GetUsersQuery : IRequest<IEnumerable<User>> { }
   
}
