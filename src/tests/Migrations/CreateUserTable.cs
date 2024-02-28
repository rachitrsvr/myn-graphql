namespace Myn.GraphQL.Tests.Migrations
{
    [FluentMigrator.Migration(202402271721)]
    public class CreateUserTable : FluentMigrator.Migration
    {
        public override void Up()
        {
            Create.Table("Users")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("FirstName").AsString().Nullable()
                .WithColumn("LastName").AsString().Nullable()
                .WithColumn("Email").AsString().Nullable()
                .WithColumn("Address").AsString().Nullable();
        }

        public override void Down()
        {
            Delete.Table("Users");
        }
    }
}
