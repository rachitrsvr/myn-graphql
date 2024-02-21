using myn_graphql_sample.Entities;
using myn_graphql_sample.Repositories;

namespace myn_graphql_sample.GraphQL.QueryTypes
{
    public class UserQueries
    {
        //private readonly IUserService _IUserService;
        //public UserQueries(IUserService IUserService)
        //{
        //    _IUserService = IUserService;
        //}
        public List<User>  GetUserList([Service] IUserService userService)
        {
            return userService.GetAllUsers();
        }
        public User GetUserById([Service] IUserService userService,[ID] int id)
        {
            return userService.GetUserById(id);
        }

    }
}
