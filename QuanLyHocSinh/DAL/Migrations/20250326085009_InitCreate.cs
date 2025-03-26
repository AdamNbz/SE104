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
                name: "KHOI",
                columns: table => new
                {
                    MaKhoi = table.Column<string>(type: "TEXT", maxLength: 4, nullable: false),
                    TenKhoi = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KHOI", x => x.MaKhoi);
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

            migrationBuilder.CreateTable(
                name: "LOP",
                columns: table => new
                {
                    MaLop = table.Column<string>(type: "TEXT", maxLength: 4, nullable: false),
                    TenLop = table.Column<string>(type: "TEXT", nullable: false),
                    MaKhoi = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LOP", x => x.MaLop);
                    table.ForeignKey(
                        name: "FK_LOP_KHOI_MaKhoi",
                        column: x => x.MaKhoi,
                        principalTable: "KHOI",
                        principalColumn: "MaKhoi",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HOCSINH",
                columns: table => new
                {
                    MaHS = table.Column<string>(type: "TEXT", nullable: false),
                    HoTen = table.Column<string>(type: "TEXT", nullable: true),
                    GioiTinh = table.Column<string>(type: "TEXT", nullable: true),
                    NgaySinh = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DiaChi = table.Column<string>(type: "TEXT", nullable: true),
                    Email = table.Column<string>(type: "TEXT", nullable: true),
                    MaLop = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HOCSINH", x => x.MaHS);
                    table.ForeignKey(
                        name: "FK_HOCSINH_LOP_MaLop",
                        column: x => x.MaLop,
                        principalTable: "LOP",
                        principalColumn: "MaLop");
                });

            migrationBuilder.InsertData(
                table: "KHOI",
                columns: new[] { "MaKhoi", "TenKhoi" },
                values: new object[,]
                {
                    { "K010", "Khối 10" },
                    { "K011", "Khối 11" },
                    { "K012", "Khối 12" }
                });

            migrationBuilder.InsertData(
                table: "THAMSO",
                columns: new[] { "Id", "SiSoToiDa", "TuoiToiDa", "TuoiToiThieu" },
                values: new object[] { 1, 40, 20, 15 });

            migrationBuilder.InsertData(
                table: "LOP",
                columns: new[] { "MaLop", "MaKhoi", "TenLop" },
                values: new object[,]
                {
                    { "10A1", "K010", "10A1" },
                    { "10A2", "K010", "10A2" },
                    { "10A3", "K010", "10A3" },
                    { "10A4", "K010", "10A4" },
                    { "11A1", "K011", "11A1" },
                    { "11A2", "K011", "11A2" },
                    { "11A3", "K011", "11A3" },
                    { "12A1", "K012", "12A1" },
                    { "12A2", "K012", "12A2" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_HOCSINH_MaLop",
                table: "HOCSINH",
                column: "MaLop");

            migrationBuilder.CreateIndex(
                name: "IX_LOP_MaKhoi",
                table: "LOP",
                column: "MaKhoi");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HOCSINH");

            migrationBuilder.DropTable(
                name: "THAMSO");

            migrationBuilder.DropTable(
                name: "LOP");

            migrationBuilder.DropTable(
                name: "KHOI");
        }
    }
}
