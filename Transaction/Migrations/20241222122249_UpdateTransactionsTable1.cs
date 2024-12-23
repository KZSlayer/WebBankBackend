using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Transaction.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTransactionsTable1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_transactions_accounts_FromAccountNumber",
                table: "transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_transactions_accounts_ToAccountNumber",
                table: "transactions");

            migrationBuilder.AlterColumn<long>(
                name: "ToAccountNumber",
                table: "transactions",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "FromAccountNumber",
                table: "transactions",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_accounts_AccountNumber",
                table: "accounts",
                column: "AccountNumber");

            migrationBuilder.AddForeignKey(
                name: "FK_transactions_accounts_FromAccountNumber",
                table: "transactions",
                column: "FromAccountNumber",
                principalTable: "accounts",
                principalColumn: "AccountNumber",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_transactions_accounts_ToAccountNumber",
                table: "transactions",
                column: "ToAccountNumber",
                principalTable: "accounts",
                principalColumn: "AccountNumber",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_transactions_accounts_FromAccountNumber",
                table: "transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_transactions_accounts_ToAccountNumber",
                table: "transactions");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_accounts_AccountNumber",
                table: "accounts");

            migrationBuilder.AlterColumn<int>(
                name: "ToAccountNumber",
                table: "transactions",
                type: "integer",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "FromAccountNumber",
                table: "transactions",
                type: "integer",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_transactions_accounts_FromAccountNumber",
                table: "transactions",
                column: "FromAccountNumber",
                principalTable: "accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_transactions_accounts_ToAccountNumber",
                table: "transactions",
                column: "ToAccountNumber",
                principalTable: "accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
