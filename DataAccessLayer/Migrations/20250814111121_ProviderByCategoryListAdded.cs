using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class ProviderByCategoryListAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ServiceCategoryId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ServiceCategoryId",
                table: "AspNetUsers",
                column: "ServiceCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_ServiceCategories_ServiceCategoryId",
                table: "AspNetUsers",
                column: "ServiceCategoryId",
                principalTable: "ServiceCategories",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_ServiceCategories_ServiceCategoryId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ServiceCategoryId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ServiceCategoryId",
                table: "AspNetUsers");
        }
    }
}
