using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Konyvelo.Logic.Migrations
{
    /// <inheritdoc />
    public partial class naming : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NewAccounts_NewCurrencies_CurrencyId",
                table: "NewAccounts");

            migrationBuilder.DropForeignKey(
                name: "FK_NewTransactions_NewAccounts_AccountId",
                table: "NewTransactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NewTransactions",
                table: "NewTransactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NewCurrencies",
                table: "NewCurrencies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NewAccounts",
                table: "NewAccounts");

            migrationBuilder.RenameTable(
                name: "NewTransactions",
                newName: "transactions");

            migrationBuilder.RenameTable(
                name: "NewCurrencies",
                newName: "currencies");

            migrationBuilder.RenameTable(
                name: "NewAccounts",
                newName: "accounts");

            migrationBuilder.RenameColumn(
                name: "Total",
                table: "transactions",
                newName: "total");

            migrationBuilder.RenameColumn(
                name: "Info",
                table: "transactions",
                newName: "info");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "transactions",
                newName: "date");

            migrationBuilder.RenameColumn(
                name: "Category",
                table: "transactions",
                newName: "category");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "transactions",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "AccountId",
                table: "transactions",
                newName: "account_id");

            migrationBuilder.RenameIndex(
                name: "IX_NewTransactions_AccountId",
                table: "transactions",
                newName: "IX_transactions_account_id");

            migrationBuilder.RenameColumn(
                name: "Code",
                table: "currencies",
                newName: "code");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "currencies",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "accounts",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "accounts",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "CurrencyId",
                table: "accounts",
                newName: "currency_id");

            migrationBuilder.RenameIndex(
                name: "IX_NewAccounts_CurrencyId",
                table: "accounts",
                newName: "IX_accounts_currency_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_transactions",
                table: "transactions",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_currencies",
                table: "currencies",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_accounts",
                table: "accounts",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_accounts_currencies_currency_id",
                table: "accounts",
                column: "currency_id",
                principalTable: "currencies",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_transactions_accounts_account_id",
                table: "transactions",
                column: "account_id",
                principalTable: "accounts",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_accounts_currencies_currency_id",
                table: "accounts");

            migrationBuilder.DropForeignKey(
                name: "FK_transactions_accounts_account_id",
                table: "transactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_transactions",
                table: "transactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_currencies",
                table: "currencies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_accounts",
                table: "accounts");

            migrationBuilder.RenameTable(
                name: "transactions",
                newName: "NewTransactions");

            migrationBuilder.RenameTable(
                name: "currencies",
                newName: "NewCurrencies");

            migrationBuilder.RenameTable(
                name: "accounts",
                newName: "NewAccounts");

            migrationBuilder.RenameColumn(
                name: "total",
                table: "NewTransactions",
                newName: "Total");

            migrationBuilder.RenameColumn(
                name: "info",
                table: "NewTransactions",
                newName: "Info");

            migrationBuilder.RenameColumn(
                name: "date",
                table: "NewTransactions",
                newName: "Date");

            migrationBuilder.RenameColumn(
                name: "category",
                table: "NewTransactions",
                newName: "Category");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "NewTransactions",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "account_id",
                table: "NewTransactions",
                newName: "AccountId");

            migrationBuilder.RenameIndex(
                name: "IX_transactions_account_id",
                table: "NewTransactions",
                newName: "IX_NewTransactions_AccountId");

            migrationBuilder.RenameColumn(
                name: "code",
                table: "NewCurrencies",
                newName: "Code");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "NewCurrencies",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "NewAccounts",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "NewAccounts",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "currency_id",
                table: "NewAccounts",
                newName: "CurrencyId");

            migrationBuilder.RenameIndex(
                name: "IX_accounts_currency_id",
                table: "NewAccounts",
                newName: "IX_NewAccounts_CurrencyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NewTransactions",
                table: "NewTransactions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NewCurrencies",
                table: "NewCurrencies",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NewAccounts",
                table: "NewAccounts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_NewAccounts_NewCurrencies_CurrencyId",
                table: "NewAccounts",
                column: "CurrencyId",
                principalTable: "NewCurrencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NewTransactions_NewAccounts_AccountId",
                table: "NewTransactions",
                column: "AccountId",
                principalTable: "NewAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
