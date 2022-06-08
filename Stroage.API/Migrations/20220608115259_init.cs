using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stroage.API.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Materials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Materials", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "People",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_People", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Storehouses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Storehouses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pack",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaterialId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pack", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pack_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Bins",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StorehouseId = table.Column<int>(type: "int", nullable: false),
                    InTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PackId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bins_Pack_PackId",
                        column: x => x.PackId,
                        principalTable: "Pack",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Bins_Storehouses_StorehouseId",
                        column: x => x.StorehouseId,
                        principalTable: "Storehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ActionLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsIn = table.Column<bool>(type: "bit", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    PersonId = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    PackId = table.Column<int>(type: "int", nullable: false),
                    BinId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActionLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActionLogs_Bins_BinId",
                        column: x => x.BinId,
                        principalTable: "Bins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ActionLogs_Pack_PackId",
                        column: x => x.PackId,
                        principalTable: "Pack",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ActionLogs_People_PersonId",
                        column: x => x.PersonId,
                        principalTable: "People",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActionLogs_BinId",
                table: "ActionLogs",
                column: "BinId");

            migrationBuilder.CreateIndex(
                name: "IX_ActionLogs_PackId",
                table: "ActionLogs",
                column: "PackId");

            migrationBuilder.CreateIndex(
                name: "IX_ActionLogs_PersonId",
                table: "ActionLogs",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Bins_PackId",
                table: "Bins",
                column: "PackId");

            migrationBuilder.CreateIndex(
                name: "IX_Bins_StorehouseId",
                table: "Bins",
                column: "StorehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_Pack_MaterialId",
                table: "Pack",
                column: "MaterialId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActionLogs");

            migrationBuilder.DropTable(
                name: "Bins");

            migrationBuilder.DropTable(
                name: "People");

            migrationBuilder.DropTable(
                name: "Pack");

            migrationBuilder.DropTable(
                name: "Storehouses");

            migrationBuilder.DropTable(
                name: "Materials");
        }
    }
}
