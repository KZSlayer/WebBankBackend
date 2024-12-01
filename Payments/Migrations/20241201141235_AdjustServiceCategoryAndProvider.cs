using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Payments.Migrations
{
    /// <inheritdoc />
    public partial class AdjustServiceCategoryAndProvider : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_service_categories_payment_providers_PaymentProviderId",
                table: "service_categories");

            migrationBuilder.DropIndex(
                name: "IX_service_categories_PaymentProviderId",
                table: "service_categories");

            migrationBuilder.DropColumn(
                name: "PaymentProviderId",
                table: "service_categories");

            migrationBuilder.AddColumn<int>(
                name: "ServiceCategoryId",
                table: "payment_providers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "payment_providers",
                keyColumn: "Id",
                keyValue: 1,
                column: "ServiceCategoryId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "payment_providers",
                keyColumn: "Id",
                keyValue: 2,
                column: "ServiceCategoryId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "payment_providers",
                keyColumn: "Id",
                keyValue: 3,
                column: "ServiceCategoryId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "phone_number_ranges",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndRange", "StartRange" },
                values: new object[] { 9366999999L, 9366470000L });

            migrationBuilder.UpdateData(
                table: "phone_number_ranges",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EndRange", "StartRange" },
                values: new object[] { 9067999999L, 9067000000L });

            migrationBuilder.UpdateData(
                table: "phone_number_ranges",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "EndRange", "StartRange" },
                values: new object[] { 9365399999L, 9365000000L });

            migrationBuilder.UpdateData(
                table: "phone_number_ranges",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "EndRange", "StartRange" },
                values: new object[] { 9169999999L, 9160L });

            migrationBuilder.InsertData(
                table: "service_categories",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[] { 1, "Оплата мобильной связи", "Мобильная связь" });

            migrationBuilder.CreateIndex(
                name: "IX_payment_providers_ServiceCategoryId",
                table: "payment_providers",
                column: "ServiceCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_payment_providers_service_categories_ServiceCategoryId",
                table: "payment_providers",
                column: "ServiceCategoryId",
                principalTable: "service_categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_payment_providers_service_categories_ServiceCategoryId",
                table: "payment_providers");

            migrationBuilder.DropIndex(
                name: "IX_payment_providers_ServiceCategoryId",
                table: "payment_providers");

            migrationBuilder.DeleteData(
                table: "service_categories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DropColumn(
                name: "ServiceCategoryId",
                table: "payment_providers");

            migrationBuilder.AddColumn<int>(
                name: "PaymentProviderId",
                table: "service_categories",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "phone_number_ranges",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndRange", "StartRange" },
                values: new object[] { 6999999L, 6470000L });

            migrationBuilder.UpdateData(
                table: "phone_number_ranges",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EndRange", "StartRange" },
                values: new object[] { 7999999L, 7000000L });

            migrationBuilder.UpdateData(
                table: "phone_number_ranges",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "EndRange", "StartRange" },
                values: new object[] { 5399999L, 5000000L });

            migrationBuilder.UpdateData(
                table: "phone_number_ranges",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "EndRange", "StartRange" },
                values: new object[] { 9999999L, 0L });

            migrationBuilder.CreateIndex(
                name: "IX_service_categories_PaymentProviderId",
                table: "service_categories",
                column: "PaymentProviderId");

            migrationBuilder.AddForeignKey(
                name: "FK_service_categories_payment_providers_PaymentProviderId",
                table: "service_categories",
                column: "PaymentProviderId",
                principalTable: "payment_providers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
