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

        public MigrationTests()
        {
            // Set up FluentMigrator services
            _serviceProvider = new ServiceCollection()
                .AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    .AddSQLite()
                    .WithGlobalConnectionString("Data Source=:memory:")
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
            var connection = new SqliteConnection("DataSource=:memory:");
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
                                                      // Check if the table exists in the database
                    var isTableExists = context.Database.GetAppliedMigrations().Any();
                    // Assert that tables or columns created/modified by migration exist
                    Assert.True(isTableExists, "Users table should exist");

                    // Assert that data inserted/modified by migration is present
                    //var user = context.Users.FirstOrDefault(u => u.FirstName == "John");
                    //Assert.NotNull(user);
                }
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
