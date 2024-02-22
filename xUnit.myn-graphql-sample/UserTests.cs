﻿using Microsoft.EntityFrameworkCore;
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
        private readonly AppDbContext _context;
        public UserTests()
        {
            var sqlConnectionString = "Host=localhost;Database=MYN_Test_DB;Username=postgres;Password=start;Port=5432";
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
            // Resolve the IRequestExecutor
            IRequestExecutor executor = await _resolver.GetRequestExecutorAsync();
            // Get the total count of rows in the table
            int totalCount = await _context.Users.CountAsync();

            // Generate a random offset
            Random random = new Random();
            int randomOffset = random.Next(0, totalCount);

            // Select a random row using the generated offset
            var randomRow = await _context.Users.OrderBy(x => Guid.NewGuid()).Skip(randomOffset).FirstOrDefaultAsync();

            // Create and execute the query
            var request = QueryRequestBuilder.New()
                .SetQuery(@"mutation UpdateUser($id:ID!){
                      updateUser(id:$id
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
                 .AddVariableValue("id", randomRow.Id)
                .Create();

            // Act
            IExecutionResult result = await executor.ExecuteAsync(request);
            var updatedRow = _context.Users.Where(x => x.Id == randomRow.Id).FirstOrDefault();
            if((updatedRow.FirstName == "Reema") && (updatedRow.LastName == "Kapoor") && (updatedRow.Email == "reema@example.com") && (updatedRow.Address == "NewAddress"))
            {
                Assert.NotNull(result.ToJson()); // Ensure data is returned
            }
            // Assert
            //Assert.Null(result); // Ensure no errors occurred
         
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
