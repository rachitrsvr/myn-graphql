using Microsoft.EntityFrameworkCore;
using myn_graphql_sample.Data;
using myn_graphql_sample.Entities;
using myn_graphql_sample.GraphQL.MutationTypes;
using myn_graphql_sample.GraphQL.QueryTypes;
using myn_graphql_sample.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testcontainers.PostgreSql;

namespace xUnit.myn_graphql_sample
{
    public class UserTests
    {
        private readonly PostgreSqlContainer _postgres = new PostgreSqlBuilder()
          .WithImage("postgres:15-alpine")
          .Build();
        private readonly IRequestExecutorResolver _resolver;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly AppDbContext _context;
        public UserTests()
        {
            var sqlConnectionString = "Host=localhost;Database=postgres;Username=postgres;Password=start;Port=5432";
            // Initialize the service collection and configure HotChocolate
            var services = new ServiceCollection();
            services
                 .AddDbContext<AppDbContext>(options => options.UseNpgsql(sqlConnectionString), ServiceLifetime.Scoped)
                 .AddScoped<IUserService,UserService>()
                .AddGraphQLServer()
                  .AddQueryType<UserQueries>()
    .AddMutationType<UserMutations>();

            // Build the service provider and resolve the IRequestExecutorResolver
            var serviceProvider = services.BuildServiceProvider();
            _resolver = serviceProvider.GetRequiredService<IRequestExecutorResolver>();
            // Get IServiceScopeFactory
            _scopeFactory = serviceProvider.GetRequiredService<IServiceScopeFactory>();

            // Create scope and resolve DbContext
            using (var scope = _scopeFactory.CreateScope())
            {
                var service = scope.ServiceProvider;
                _context = service.GetRequiredService<AppDbContext>();
            }
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
            var data = _context.Users.ToList();
            // Create and execute the query
            var request = QueryRequestBuilder.New()
                .SetQuery(@"mutation {
                      addUser(input: {
                        id:5
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
                .Create();

            // Act
            IExecutionResult result = await executor.ExecuteAsync(request);
            // Assert
            //Assert.Null(result); // Ensure no errors occurred
            Assert.NotNull(result); // Ensure data is returned
            // You can perform additional assertions on the returned data if needed
        }
        [Fact]
        public async Task TestUpdateUsersQuery()
        {
            // Resolve the IRequestExecutor
            IRequestExecutor executor = await _resolver.GetRequestExecutorAsync();

            // Create and execute the query
            var request = QueryRequestBuilder.New()
                .SetQuery(@"mutation {
                      updateUser(id:6 
                      , firstName: ""Reema""
                      , lastName: ""Kapoor""
                      , email: ""reema@example.com""
                      , address: ""NewAddress"" 
                      ) 
                         {
                          id
                          firstName
                          lastName
                          email
                          address
                        }
  
                    }")
                .Create();

            // Act
            IExecutionResult result = await executor.ExecuteAsync(request);

            // Assert
            //Assert.Null(result); // Ensure no errors occurred
            Assert.NotNull(result); // Ensure data is returned
            // You can perform additional assertions on the returned data if needed
        }
        [Fact]
        public async Task TestDeleteUserQuery()
        {
            // Resolve the IRequestExecutor
            IRequestExecutor executor = await _resolver.GetRequestExecutorAsync();

            // Create and execute the query
            var request = QueryRequestBuilder.New()
                .SetQuery(@"mutation {
                      deleteUser(id: 1)
                    }")
                .Create();

            // Act
            IExecutionResult result = await executor.ExecuteAsync(request);

            // Assert
            //Assert.Null(result); // Ensure no errors occurred
            Assert.NotNull(result); // Ensure data is returned
            // You can perform additional assertions on the returned data if needed
        }
    }
}
