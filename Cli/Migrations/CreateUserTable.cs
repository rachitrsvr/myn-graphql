using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cli.Migrations
{
    [Migration(202402271721)]
    public class CreateUserTable : Migration
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
