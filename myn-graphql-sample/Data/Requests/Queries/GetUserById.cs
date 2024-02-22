using MediatR;
using myn_graphql_sample.Entities;
using System.Collections.Generic;

namespace myn_graphql_sample.Data.Requests.Queries
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
