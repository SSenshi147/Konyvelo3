using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Konyvelo.Logic.Migrations
{
    /// <inheritdoc />
    public partial class better_data : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Type",
                table: "NewTransactions",
                newName: "Total");

            migrationBuilder.AlterColumn<string>(
                name: "Info",
                table: "NewTransactions",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "NewTransactions",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "NewTransactions");

            migrationBuilder.RenameColumn(
                name: "Total",
                table: "NewTransactions",
                newName: "Type");

            migrationBuilder.AlterColumn<string>(
                name: "Info",
                table: "NewTransactions",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);
        }
    }
}
