using MediatR;
using Myn.GraphQL.Api.Entities;

namespace Myn.GraphQL.Api.Data.Requests.Commands
{
    public record AddUserCommand(User input) : IRequest<User>;
}
