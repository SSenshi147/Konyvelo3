using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Konyvelo.Logic.Migrations
{
    /// <inheritdoc />
    public partial class newtablenames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Wallets_WalletId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Wallets_Currencies_CurrencyId",
                table: "Accounts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Wallets",
                table: "Accounts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Transactions",
                table: "Transactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Currencies",
                table: "Currencies");

            migrationBuilder.RenameTable(
                name: "Accounts",
                newName: "wallets_old");

            migrationBuilder.RenameTable(
                name: "Transactions",
                newName: "transactions_old");

            migrationBuilder.RenameTable(
                name: "Currencies",
                newName: "currencies_old");

            migrationBuilder.RenameIndex(
                name: "IX_Wallets_CurrencyId",
                table: "wallets_old",
                newName: "IX_wallets_old_CurrencyId");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_WalletId",
                table: "transactions_old",
                newName: "IX_transactions_old_WalletId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_wallets_old",
                table: "wallets_old",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_transactions_old",
                table: "transactions_old",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_currencies_old",
                table: "currencies_old",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_transactions_old_wallets_old_WalletId",
                table: "transactions_old",
                column: "WalletId",
                principalTable: "wallets_old",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_wallets_old_currencies_old_CurrencyId",
                table: "wallets_old",
                column: "CurrencyId",
                principalTable: "currencies_old",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_transactions_old_wallets_old_WalletId",
                table: "transactions_old");

            migrationBuilder.DropForeignKey(
                name: "FK_wallets_old_currencies_old_CurrencyId",
                table: "wallets_old");

            migrationBuilder.DropPrimaryKey(
                name: "PK_wallets_old",
                table: "wallets_old");

            migrationBuilder.DropPrimaryKey(
                name: "PK_transactions_old",
                table: "transactions_old");

            migrationBuilder.DropPrimaryKey(
                name: "PK_currencies_old",
                table: "currencies_old");

            migrationBuilder.RenameTable(
                name: "wallets_old",
                newName: "Accounts");

            migrationBuilder.RenameTable(
                name: "transactions_old",
                newName: "Transactions");

            migrationBuilder.RenameTable(
                name: "currencies_old",
                newName: "Currencies");

            migrationBuilder.RenameIndex(
                name: "IX_wallets_old_CurrencyId",
                table: "Accounts",
                newName: "IX_Wallets_CurrencyId");

            migrationBuilder.RenameIndex(
                name: "IX_transactions_old_WalletId",
                table: "Transactions",
                newName: "IX_Transactions_WalletId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Wallets",
                table: "Accounts",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Transactions",
                table: "Transactions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Currencies",
                table: "Currencies",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Wallets_WalletId",
                table: "Transactions",
                column: "WalletId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Wallets_Currencies_CurrencyId",
                table: "Accounts",
                column: "CurrencyId",
                principalTable: "Currencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
