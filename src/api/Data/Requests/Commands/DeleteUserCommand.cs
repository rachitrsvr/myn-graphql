using MediatR;

namespace Myn.GraphQL.Api.Data.Requests.Commands
{
    public class DeleteUserCommand : IRequest<bool>
    {
        public int Id { get; }

        public DeleteUserCommand(int id)
        {
            Id = id;
        }
    }
}
