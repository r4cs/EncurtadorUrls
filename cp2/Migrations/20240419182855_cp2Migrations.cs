using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cp2.Migrations
{
    /// <inheritdoc />
    public partial class cp2Migrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EncurtadorUrls",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    UrlLonga = table.Column<string>(type: "NVARCHAR2(255)", maxLength: 255, nullable: false),
                    UrlCurta = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    Codigo = table.Column<string>(type: "NVARCHAR2(7)", maxLength: 7, nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EncurtadorUrls", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EncurtadorUrls_Codigo",
                table: "EncurtadorUrls",
                column: "Codigo",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EncurtadorUrls");
        }
    }
}
