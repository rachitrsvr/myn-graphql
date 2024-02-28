using MediatR;
using Myn.GraphQL.Api.Entities;

namespace Myn.GraphQL.Api.Data.Requests.Queries
{
    public class GetUserById : IRequest<IEnumerable<User>>
    {
        public GetUserById(int id)
        {
            Id = id;
        }

        public int Id { get; }
    }
}
