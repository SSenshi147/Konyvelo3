using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Konyvelo.Logic.Migrations
{
    /// <inheritdoc />
    public partial class total : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Count",
                table: "Transactions");

            migrationBuilder.RenameColumn(
                name: "Value",
                table: "Transactions",
                newName: "Total");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Total",
                table: "Transactions",
                newName: "Value");

            migrationBuilder.AddColumn<int>(
                name: "Count",
                table: "Transactions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
