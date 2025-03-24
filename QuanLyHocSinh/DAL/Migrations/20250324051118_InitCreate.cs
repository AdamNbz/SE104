using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class InitCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HOCSINH",
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
                    table.PrimaryKey("PK_HOCSINH", x => x.MaHS);
                });

            migrationBuilder.CreateTable(
                name: "THAMSO",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TuoiToiDa = table.Column<int>(type: "INTEGER", nullable: false),
                    TuoiToiThieu = table.Column<int>(type: "INTEGER", nullable: false),
                    SiSoToiDa = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_THAMSO", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "HOCSINH",
                columns: new[] { "MaHS", "DiaChi", "Email", "GioiTinh", "HoTen", "NgaySinh" },
                values: new object[,]
                {
                    { "HS0001", "Ở đây", "thuandq@uit.edu.vn", "Nam", "Dương Quốc Thuận", new DateTime(2009, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { "HS0002", "Ở đây", "1234@lmao.com", "Nam", "Tiền Minh Dương", new DateTime(2009, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "THAMSO",
                columns: new[] { "Id", "SiSoToiDa", "TuoiToiDa", "TuoiToiThieu" },
                values: new object[] { 1, 40, 20, 15 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HOCSINH");

            migrationBuilder.DropTable(
                name: "THAMSO");
        }
    }
}
