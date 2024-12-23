using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Transaction.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTransactionsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_transactions_accounts_FromAccountUserId",
                table: "transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_transactions_accounts_ToAccountUserId",
                table: "transactions");

            migrationBuilder.RenameColumn(
                name: "ToAccountUserId",
                table: "transactions",
                newName: "ToAccountNumber");

            migrationBuilder.RenameColumn(
                name: "FromAccountUserId",
                table: "transactions",
                newName: "FromAccountNumber");

            migrationBuilder.RenameIndex(
                name: "IX_transactions_ToAccountUserId",
                table: "transactions",
                newName: "IX_transactions_ToAccountNumber");

            migrationBuilder.RenameIndex(
                name: "IX_transactions_FromAccountUserId",
                table: "transactions",
                newName: "IX_transactions_FromAccountNumber");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_transactions_accounts_FromAccountNumber",
                table: "transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_transactions_accounts_ToAccountNumber",
                table: "transactions");

            migrationBuilder.RenameColumn(
                name: "ToAccountNumber",
                table: "transactions",
                newName: "ToAccountUserId");

            migrationBuilder.RenameColumn(
                name: "FromAccountNumber",
                table: "transactions",
                newName: "FromAccountUserId");

            migrationBuilder.RenameIndex(
                name: "IX_transactions_ToAccountNumber",
                table: "transactions",
                newName: "IX_transactions_ToAccountUserId");

            migrationBuilder.RenameIndex(
                name: "IX_transactions_FromAccountNumber",
                table: "transactions",
                newName: "IX_transactions_FromAccountUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_transactions_accounts_FromAccountUserId",
                table: "transactions",
                column: "FromAccountUserId",
                principalTable: "accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_transactions_accounts_ToAccountUserId",
                table: "transactions",
                column: "ToAccountUserId",
                principalTable: "accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
