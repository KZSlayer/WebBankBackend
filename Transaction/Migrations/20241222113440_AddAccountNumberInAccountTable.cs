using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Transaction.Migrations
{
    /// <inheritdoc />
    public partial class AddAccountNumberInAccountTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "AccountNumber",
                table: "accounts",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountNumber",
                table: "accounts");
        }
    }
}
