using MediatR;
using myn_graphql_sample.Data.Requests.Queries;
using myn_graphql_sample.Entities;
using myn_graphql_sample.Repositories;

namespace myn_graphql_sample.GraphQL.QueryTypes
{
    public class UserQueries
    {
        private readonly IUserService _userService;
        private readonly IMediator _mediator;
        public UserQueries(IUserService IUserService, IMediator mediator)
        {
            _userService = IUserService;
            _mediator = mediator;
        }
        public async Task<IEnumerable<User>>  GetUserList()
        {
            return await _mediator.Send(new GetUsersQuery());
        }
        public User GetUserById([ID] int id)
        {
            return _userService.GetUserById(id);
        }

    }
}
