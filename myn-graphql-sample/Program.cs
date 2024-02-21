using Microsoft.EntityFrameworkCore;
using myn_graphql_sample.Data;
using myn_graphql_sample.GraphQL.MutationTypes;
using myn_graphql_sample.GraphQL.QueryTypes;
using myn_graphql_sample.Repositories;
using System;

var builder = WebApplication.CreateBuilder(args);


var sqlConnectionString = builder.Configuration["ConnectionString"];
// Add services to the container.


builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(sqlConnectionString));
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddGraphQLServer()
    .AddQueryType<UserQueries>()
    .AddMutationType<UserMutations>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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
