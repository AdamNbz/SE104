using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddKhoiLop : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "HOCSINH",
                keyColumn: "MaHS",
                keyValue: "HS0001");

            migrationBuilder.DeleteData(
                table: "HOCSINH",
                keyColumn: "MaHS",
                keyValue: "HS0002");

            migrationBuilder.AddColumn<string>(
                name: "MaLop",
                table: "HOCSINH",
                type: "TEXT",
                nullable: true);

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

            migrationBuilder.AddForeignKey(
                name: "FK_HOCSINH_LOP_MaLop",
                table: "HOCSINH",
                column: "MaLop",
                principalTable: "LOP",
                principalColumn: "MaLop");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HOCSINH_LOP_MaLop",
                table: "HOCSINH");

            migrationBuilder.DropTable(
                name: "LOP");

            migrationBuilder.DropTable(
                name: "KHOI");

            migrationBuilder.DropIndex(
                name: "IX_HOCSINH_MaLop",
                table: "HOCSINH");

            migrationBuilder.DropColumn(
                name: "MaLop",
                table: "HOCSINH");

            migrationBuilder.InsertData(
                table: "HOCSINH",
                columns: new[] { "MaHS", "DiaChi", "Email", "GioiTinh", "HoTen", "NgaySinh" },
                values: new object[,]
                {
                    { "HS0001", "Ở đây", "thuandq@uit.edu.vn", "Nam", "Dương Quốc Thuận", new DateTime(2009, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { "HS0002", "Ở đây", "1234@lmao.com", "Nam", "Tiền Minh Dương", new DateTime(2009, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });
        }
    }
}
