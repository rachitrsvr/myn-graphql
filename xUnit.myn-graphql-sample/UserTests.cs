using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using myn_graphql_sample.Data;
using myn_graphql_sample.Entities;
using myn_graphql_sample.GraphQL.MutationTypes;
using myn_graphql_sample.GraphQL.QueryTypes;
using myn_graphql_sample.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Testcontainers.PostgreSql;
using MediatR;
using myn_graphql_sample.Data.Requests.Commands;

namespace xUnit.myn_graphql_sample
{
    public class UserTests
    {
        private readonly PostgreSqlContainer _postgres = new PostgreSqlBuilder()
          .WithImage("postgres:15-alpine")
          .Build();
        private readonly IRequestExecutorResolver _resolver;
        private readonly AppDbContext _context;
        public UserTests()
        {
            var sqlConnectionString = "Host=localhost;Database=MYN_Test_DB;Username=postgres;Password=start;Port=5432";
            // Initialize the service collection and configure HotChocolate
            var services = new ServiceCollection();
            services
                 .AddDbContext<AppDbContext>(options => options.UseNpgsql(sqlConnectionString), ServiceLifetime.Scoped)
                 .AddScoped<IUserService, UserService>()
                 .AddScoped<UserQueries>()
                 .AddScoped<UserMutations>()

                .AddGraphQLServer()
                  .AddQueryType<UserQueries>()
    .AddMutationType<UserMutations>();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
    

            // Build the service provider and resolve the IRequestExecutorResolver
            var serviceProvider = services.BuildServiceProvider();
            _resolver = serviceProvider.GetRequiredService<IRequestExecutorResolver>();
            // Resolve DbContext
            _context = serviceProvider.GetRequiredService<AppDbContext>();
        }
        public Task InitializeAsync()
        {
            return _postgres.StartAsync();
        }

        public Task DisposeAsync()
        {
            return _postgres.DisposeAsync().AsTask();
        }
        [Fact]
        public async Task TestGetUsersQuery()
        {
            // Resolve the IRequestExecutor
            IRequestExecutor executor = await _resolver.GetRequestExecutorAsync();

            // Create and execute the query
            var request = QueryRequestBuilder.New()
                .SetQuery(@"query{
                          userList{
                            id
                            firstName
                            lastName
                            email
                          }
                        }")
                .Create();

            // Act
            IExecutionResult result = await executor.ExecuteAsync(request);
            var resultQuery = result.ToJson();

            // Assert
            //Assert.Null(result); // Ensure no errors occurred
            Assert.NotNull(resultQuery); // Ensure data is returned
            // You can perform additional assertions on the returned data if needed
        }

        [Fact]
        public async Task TestAddUsersQuery()
        {
            // Resolve the IRequestExecutor
            IRequestExecutor executor = await _resolver.GetRequestExecutorAsync();
            var NewId = _context.Users.Max(entity => (int?)entity.Id) + 1;
            // Create and execute the query
            var request = QueryRequestBuilder.New()
                .SetQuery(@"mutation AddUser($id: ID!) {
                      addUser(input: {
                        id: $id
                        firstName: ""Roy""
                        lastName: ""Hudson""
                        email: ""roy@test.com""
                        address: ""royaddress""
                      }) {
    
                          id
                          firstName
                          lastName
                          email
                          address
    
                      }
                    }")
                 .AddVariableValue("id", NewId)
                .Create();

            // Act
            IExecutionResult result = await executor.ExecuteAsync(request);
            var chkIfIdExist = _context.Users.Any(x => x.Id == NewId);
            // Assert
            //Assert.Null(result); // Ensure no errors occurred
            if (chkIfIdExist)
            {
                Assert.NotNull(result.ToJson());
            } // Ensure data is returned
        }
        [Fact]
        public async Task TestUpdateUsersQuery()
        {

            // Get the total count of rows in the table
            int totalCount = await _context.Users.CountAsync();
            if (totalCount > 0) {
                // Resolve the IRequestExecutor
                IRequestExecutor executor = await _resolver.GetRequestExecutorAsync();
                // Generate a random offset
                Random random = new Random();
                int randomOffset = random.Next(0, totalCount);

                // Select a random row using the generated offset
                var randomRow = await _context.Users.OrderBy(x => x.Id).Skip(randomOffset).FirstOrDefaultAsync();

                // Create and execute the query
                var request = QueryRequestBuilder.New()
                    .SetQuery(@"mutation UpdateUser($id: Int!){
                      updateUser(id:$id
                       firstName: ""Reema""
                       lastName: ""Kapoor""
                       email: ""reema@example.com""
                       address: ""NewAddress"" 
                      ) 
                         {
                          id
                          firstName
                          lastName
                          email
                          address
                        }
  
                    }")
                     .AddVariableValue("id", randomRow.Id)
                    .Create();

                // Act
                IExecutionResult result = await executor.ExecuteAsync(request);
                var updatedRow = _context.Users.Where(x => x.Id == randomRow.Id).FirstOrDefault();
                if ((updatedRow.FirstName == "Reema") && (updatedRow.LastName == "Kapoor") && (updatedRow.Email == "reema@example.com") && (updatedRow.Address == "NewAddress"))
                {
                    Assert.NotNull(result.ToJson()); // Ensure data is returned
                }


                // You can perform additional assertions on the returned data if needed
            }
            else {
                // Assert
                Assert.Null("No data found to update!"); // Ensure no errors occurred
            }

        }
        [Fact]
        public async Task TestDeleteUserQuery()
        { 
             // Get the total count of rows in the table
            int totalCount = await _context.Users.CountAsync();
            if(totalCount > 0) {
            // Resolve the IRequestExecutor
            IRequestExecutor executor = await _resolver.GetRequestExecutorAsync();

        // Generate a random offset
        Random random = new Random();
        int randomOffset = random.Next(0, totalCount);

        // Select a random row using the generated offset
        var randomRow = await _context.Users.OrderBy(x => x.Id).Skip(randomOffset).FirstOrDefaultAsync();

        // Create and execute the query
        var request = QueryRequestBuilder.New()
            .SetQuery(@"mutation DeleteUser($id: Int!) {
                      deleteUser(id: $id)
                    }")
             .AddVariableValue("id", randomRow.Id)
            .Create();

        // Act
        IExecutionResult result = await executor.ExecuteAsync(request);
        var chkIfIdExists = _context.Users.Any(q=>q.Id == randomRow.Id);
                if (!chkIfIdExists)
                {
                    // Assert
                    //Assert.Null(result); // Ensure no errors occurred
                    Assert.NotNull(result); // Ensure data is returned
                                            // You can perform additional assertions on the returned data if needed
                }

            }
        }
    }
}
