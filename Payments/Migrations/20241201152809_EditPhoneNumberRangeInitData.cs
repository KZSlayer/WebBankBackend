using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Payments.Migrations
{
    /// <inheritdoc />
    public partial class EditPhoneNumberRangeInitData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "phone_number_ranges",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndRange", "StartRange" },
                values: new object[] { 9636999999L, 9636470000L });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "phone_number_ranges",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndRange", "StartRange" },
                values: new object[] { 9366999999L, 9366470000L });
        }
    }
}
