using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Myn.GraphQL.Api.Data;
using Myn.GraphQL.Api.GraphQL.MutationTypes;
using Myn.GraphQL.Api.GraphQL.QueryTypes;
using Myn.GraphQL.Api.Repositories;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
var sqlConnectionString = builder.Configuration["ConnectionString"];



// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(sqlConnectionString));
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<UserQueries>();

builder.Services.AddGraphQLServer()
    .AddQueryType<UserQueries>()
    .AddMutationType<UserMutations>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//regiister MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));


builder.Services.AddLogging(builder =>
{
    builder.AddConsole(); // Configures console logging
                          // Add other logging providers if needed (e.g., AddDebug, AddEventLog, etc.)
});

// Build the application
var app = builder.Build();

// Check if the environment is set to development
if (app.Environment.IsDevelopment())
{
    // Enable Swagger for API documentation
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Log information about adding routes
app.Logger.LogInformation("Adding Routes");

// Define a GraphQL endpoint using MapGet
app.MapGet("/graphql", async (ILogger<Program> logger, HttpResponse response) =>
{
    // Log information for testing purposes
    logger.LogInformation("Testing logging in Program.cs");
    // Uncomment the line below if you want to send a response
    // await response.WriteAsync("Testing");
});

// Log information about starting the app
app.Logger.LogInformation("Starting the app");

// Enable HTTPS redirection
app.UseHttpsRedirection();

// Enable authorization
app.UseAuthorization();

// Map GraphQL endpoint
app.MapGraphQL();

// Map controllers
app.MapControllers();

// Run the application
app.Run();

