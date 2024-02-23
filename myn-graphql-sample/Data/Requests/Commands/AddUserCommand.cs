using MediatR;
using myn_graphql_sample.Entities;

namespace myn_graphql_sample.Data.Requests.Commands
{
    public record AddUserCommand(User input) : IRequest<User>;
}
