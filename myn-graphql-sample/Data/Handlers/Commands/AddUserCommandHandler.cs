using MediatR;
using myn_graphql_sample.Entities;

namespace myn_graphql_sample.Data.Handlers.Commands
{
    public record AddUserCommandHandler(User user) : IRequest<User>;
}
