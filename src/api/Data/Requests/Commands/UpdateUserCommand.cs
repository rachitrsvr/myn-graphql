using MediatR;
using Myn.GraphQL.Api.Entities;

namespace Myn.GraphQL.Api.Data.Requests.Commands
{
    public record UpdateUserCommand(User input) : IRequest<User>;
}
