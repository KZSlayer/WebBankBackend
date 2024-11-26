using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Transaction.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTransactionsForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_transactions_accounts_FromAccountId",
                table: "transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_transactions_accounts_ToAccountId",
                table: "transactions");

            migrationBuilder.RenameColumn(
                name: "ToAccountId",
                table: "transactions",
                newName: "ToAccountUserId");

            migrationBuilder.RenameColumn(
                name: "FromAccountId",
                table: "transactions",
                newName: "FromAccountUserId");

            migrationBuilder.RenameIndex(
                name: "IX_transactions_ToAccountId",
                table: "transactions",
                newName: "IX_transactions_ToAccountUserId");

            migrationBuilder.RenameIndex(
                name: "IX_transactions_FromAccountId",
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                newName: "ToAccountId");

            migrationBuilder.RenameColumn(
                name: "FromAccountUserId",
                table: "transactions",
                newName: "FromAccountId");

            migrationBuilder.RenameIndex(
                name: "IX_transactions_ToAccountUserId",
                table: "transactions",
                newName: "IX_transactions_ToAccountId");

            migrationBuilder.RenameIndex(
                name: "IX_transactions_FromAccountUserId",
                table: "transactions",
                newName: "IX_transactions_FromAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_transactions_accounts_FromAccountId",
                table: "transactions",
                column: "FromAccountId",
                principalTable: "accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_transactions_accounts_ToAccountId",
                table: "transactions",
                column: "ToAccountId",
                principalTable: "accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
