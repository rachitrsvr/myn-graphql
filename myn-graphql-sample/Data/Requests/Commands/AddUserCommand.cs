using MediatR;
using myn_graphql_sample.Entities;

namespace myn_graphql_sample.Data.Requests.Commands
{
    public class AddUserCommand : IRequest<User>
    {
        public User User { get; set; }
    }
}
