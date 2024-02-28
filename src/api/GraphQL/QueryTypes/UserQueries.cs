using MediatR;
using Myn.GraphQL.Api.Data.Requests.Queries;
using Myn.GraphQL.Api.Entities;

namespace Myn.GraphQL.Api.GraphQL.QueryTypes
{
    public class UserQueries
    {
        private readonly IMediator _mediator;
        public UserQueries(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task<IEnumerable<User>>  GetUserList()
        {
            return await _mediator.Send(new GetUsersQuery());
        }

        public async Task<IEnumerable<User>> GetUserById(int id)
        {
            return await _mediator.Send(new GetUserById(id));
        }
    }
}
