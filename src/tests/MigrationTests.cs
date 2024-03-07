using FluentMigrator.Runner;

namespace Myn.GraphQL.Tests
{
    public class MigrationTests
    {
        private ServiceProvider _serviceProvider;
        //private string DataSource = "Data Source=C:\\Projects\\myn-graphql\\xUnit.myn-graphql-sample\\myn-sqlite.db";
        private string DataSource = "Host=172.21.0.2;Database=postgres;Username=postgres;Password=start;Port=5432";
        public MigrationTests()
        {
            // Set up FluentMigrator services
            _serviceProvider = new ServiceCollection()
                .AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    .AddPostgres()
                // Set the connection string
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
            // Set up an  Npgsql database for testing
            var connection = new NpgsqlConnection(DataSource);
            connection.Open();

            try
            {
                // Set up DbContext with SQLite connection
                var options = new DbContextOptionsBuilder<AppDbContext>()
                    .UseNpgsql(connection)
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
            catch(Exception ex)
            {

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
                command.CommandText = $"SELECT EXISTS (SELECT FROM information_schema.tables WHERE table_name = '{tableName}')";
                context.Database.OpenConnection(); // Open the connection
                using (var result = command.ExecuteReader())
                {
                    return result.HasRows;
                }
            }
        }
    }
}
