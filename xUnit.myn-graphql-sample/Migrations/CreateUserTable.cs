using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore.Migrations;
using Org.BouncyCastle.Utilities.Zlib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentMigrator;

namespace xUnit.myn_graphql_sample.Migrations
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
