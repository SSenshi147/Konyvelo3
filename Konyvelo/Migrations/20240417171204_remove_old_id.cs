using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Konyvelo.Logic.Migrations
{
    /// <inheritdoc />
    public partial class remove_old_id : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OldId",
                table: "transactions");

            migrationBuilder.DropColumn(
                name: "OldId",
                table: "currencies");

            migrationBuilder.DropColumn(
                name: "OldId",
                table: "accounts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OldId",
                table: "transactions",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OldId",
                table: "currencies",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OldId",
                table: "accounts",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
