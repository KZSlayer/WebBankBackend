using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Payments.Migrations
{
    /// <inheritdoc />
    public partial class SeedPaymentProvidersAndPhoneRanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "payment_providers",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Оператор мобильной связи", "Билайн" },
                    { 2, "Оператор мобильной связи", "Мегафон" },
                    { 3, "Оператор мобильной связи", "МТС" }
                });

            migrationBuilder.InsertData(
                table: "phone_number_ranges",
                columns: new[] { "Id", "EndRange", "PaymentProviderId", "Prefix", "StartRange" },
                values: new object[,]
                {
                    { 1, 6999999L, 1, "963", 6470000L },
                    { 2, 7999999L, 1, "906", 7000000L },
                    { 3, 5399999L, 2, "936", 5000000L },
                    { 4, 9999999L, 3, "916", 0L }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "phone_number_ranges",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "phone_number_ranges",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "phone_number_ranges",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "phone_number_ranges",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "payment_providers",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "payment_providers",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "payment_providers",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
