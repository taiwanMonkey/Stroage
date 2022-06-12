using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stroage.API.Migrations
{
    public partial class add_token : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "Token",
                table: "People",
                type: "uniqueidentifier",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Token",
                table: "People");
        }
    }
}
