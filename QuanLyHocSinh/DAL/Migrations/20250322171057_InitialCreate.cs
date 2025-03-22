using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HocSinhs",
                columns: table => new
                {
                    MaHS = table.Column<string>(type: "TEXT", nullable: false),
                    HoTen = table.Column<string>(type: "TEXT", nullable: false),
                    GioiTinh = table.Column<string>(type: "TEXT", nullable: false),
                    NgaySinh = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DiaChi = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HocSinhs", x => x.MaHS);
                });

            migrationBuilder.InsertData(
                table: "HocSinhs",
                columns: new[] { "MaHS", "DiaChi", "Email", "GioiTinh", "HoTen", "NgaySinh" },
                values: new object[,]
                {
                    { "HS001", "Ha Noi", "hello", "Nam", "Nguyen Van A", new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { "HS002", "Ha Noi", "hello", "Nam", "Nguyen Van B", new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { "HS003", "Ha Noi", "hello", "Nam", "Nguyen Van C", new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HocSinhs");
        }
    }
}
