using FluentMigrator.Runner;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xUnit.myn_graphql_sample
{
    public class MigrationTests
    {
        private ServiceProvider _serviceProvider;
        //private string DataSource = "Data Source=C:\\Projects\\myn-graphql\\xUnit.myn-graphql-sample\\myn-sqlite.db";
        private string DataSource = "Data Source=myn-sqlite.db";
        public MigrationTests()
        {
            // Set up FluentMigrator services
            _serviceProvider = new ServiceCollection()
                .AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    .AddSQLite()
                    .WithGlobalConnectionString(DataSource)
                    .ScanIn(typeof(MigrationTests).Assembly).For.Migrations())
                .AddLogging(lb => lb.AddFluentMigratorConsole())
                .BuildServiceProvider();
        }

        [Fact]
        public void TestMigration()
        {
            // Apply migrations for testing
            using (var scope = _serviceProvider.CreateScope())
            {
                var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
                runner.MigrateUp();
            }

            // Add your test logic here
            // Set up an in-memory SQLite database for testing
            var connection = new SqliteConnection(DataSource);
            connection.Open();

            try
            {
                // Set up DbContext with SQLite connection
                var options = new DbContextOptionsBuilder<AppDbContext>()
                    .UseSqlite(connection)
                    .Options;

                // Apply migrations for testing
                using (var context = new AppDbContext(options))
                {
                    context.Database.EnsureCreated(); // Ensure database is created
                                                      
                    
                    string tableName = "Users";
                    bool exists = TableExists(context, tableName); // Check if the table exists in the database
                    // Assert that tables or columns created/modified by migration exist
                    Assert.True(exists, "Users table should exist");

                }
            }
            finally
            {
                connection.Close();
            }
        }
        static bool TableExists(DbContext context, string tableName)
        {
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = $"SELECT name FROM sqlite_master WHERE type='table' AND name='{tableName}'";
                context.Database.OpenConnection(); // Open the connection
                using (var result = command.ExecuteReader())
                {
                    return result.HasRows;
                }
            }
        }
    }
}
