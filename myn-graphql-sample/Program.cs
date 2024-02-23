using MediatR;
using Microsoft.EntityFrameworkCore;
using myn_graphql_sample.Data;
using myn_graphql_sample.Entities;
using myn_graphql_sample.GraphQL.MutationTypes;
using myn_graphql_sample.GraphQL.QueryTypes;
using myn_graphql_sample.Repositories;
using System.Reflection;
using Serilog;
using Serilog.Events;
using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);


var sqlConnectionString = builder.Configuration["ConnectionString"];

//Configure Serilog


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

var app = builder.Build();
//var loggerFactory = app.Services.GetService<ILoggerFactory>();
//loggerFactory.AddProvider(builder.Configuration["Logging:LogFilePath"].ToString());
// Configure the HTTP request pipeline.

var loggerFactory = app.Services.GetService<ILoggerFactory>();
loggerFactory.AddFile(builder.Configuration["LogFilePath"].ToString());

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
