using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookStore.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOrderStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "49416c2d-2414-4df2-91bf-a85c0df643d4");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ca14709c-9568-45fd-9f45-19c9702c8f17");

            migrationBuilder.AlterColumn<int>(
                name: "status",
                table: "Orders",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "9ede722f-4df0-4836-8278-30022d239ae0", null, "customer", "CUSTOMER" },
                    { "f0766dc3-db71-469f-a2c4-90226ac6f6c2", null, "admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9ede722f-4df0-4836-8278-30022d239ae0");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f0766dc3-db71-469f-a2c4-90226ac6f6c2");

            migrationBuilder.AlterColumn<string>(
                name: "status",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "49416c2d-2414-4df2-91bf-a85c0df643d4", null, "admin", "ADMIN" },
                    { "ca14709c-9568-45fd-9f45-19c9702c8f17", null, "customer", "CUSTOMER" }
                });
        }
    }
}
