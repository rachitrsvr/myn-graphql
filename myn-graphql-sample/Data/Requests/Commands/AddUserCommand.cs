using myn_graphql_sample.Entities;

namespace myn_graphql_sample.Data.Requests.Commands
{
    public class AddUserCommand
    {
        public AddUserCommand(User user)
        {
            User = user;
        }

        public User User { get; }
    }
}
