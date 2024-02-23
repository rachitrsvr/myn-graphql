using MediatR;
using Microsoft.EntityFrameworkCore;
using myn_graphql_sample.Data;
using myn_graphql_sample.Entities;
using myn_graphql_sample.GraphQL.MutationTypes;
using myn_graphql_sample.GraphQL.QueryTypes;
using myn_graphql_sample.Repositories;
using System.Reflection;

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

//builder.Host.ConfigureLogging(logging =>
//{
//    logging.ClearProviders();
//    logging.AddConsole();
//});


var app = builder.Build();
//var loggerFactory = app.Services.GetService<ILoggerFactory>();
//loggerFactory.AddProvider(builder.Configuration["Logging:LogFilePath"].ToString());
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapGraphQL();
app.MapControllers();
app.Run();
