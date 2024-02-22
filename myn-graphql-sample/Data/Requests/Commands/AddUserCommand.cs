using MediatR;
using myn_graphql_sample.Entities;

namespace myn_graphql_sample.Data.Requests.Commands
{
    //public class AddUserCommand : IRequest<User>
    //{
    //    private readonly User User;

    //    public AddUserCommand(User userModel)
    //    {
    //        User = userModel;
    //    }

    //}

    public record AddUserCommand(User input) : IRequest<User>;
}
