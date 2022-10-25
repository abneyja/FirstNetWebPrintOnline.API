using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FirstNetWebPrintOnline.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PrintRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Ordernumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Timestamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Timestale = table.Column<int>(type: "int", nullable: false),
                    Printsonar = table.Column<bool>(type: "bit", nullable: false),
                    Printphone = table.Column<bool>(type: "bit", nullable: false),
                    Printorder = table.Column<bool>(type: "bit", nullable: false),
                    Printbarcodes = table.Column<bool>(type: "bit", nullable: false),
                    Printcoam = table.Column<bool>(type: "bit", nullable: false),
                    Automode = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrintRequests", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PrintRequests");
        }
    }
}
