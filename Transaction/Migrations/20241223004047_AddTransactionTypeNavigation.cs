using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Transaction.Migrations
{
    /// <inheritdoc />
    public partial class AddTransactionTypeNavigation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TransactionTypeId1",
                table: "transactions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_transactions_TransactionTypeId1",
                table: "transactions",
                column: "TransactionTypeId1");

            migrationBuilder.AddForeignKey(
                name: "FK_transactions_transaction_types_TransactionTypeId1",
                table: "transactions",
                column: "TransactionTypeId1",
                principalTable: "transaction_types",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_transactions_transaction_types_TransactionTypeId1",
                table: "transactions");

            migrationBuilder.DropIndex(
                name: "IX_transactions_TransactionTypeId1",
                table: "transactions");

            migrationBuilder.DropColumn(
                name: "TransactionTypeId1",
                table: "transactions");
        }
    }
}
