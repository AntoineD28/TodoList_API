using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TodoAPI.Migrations
{
    /// <inheritdoc />
    public partial class FK_UserId_In_TodoItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "TodoItem",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TodoItem_UserId",
                table: "TodoItem",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TodoItem_User_UserId",
                table: "TodoItem",
                column: "UserId",
                principalTable: "User",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TodoItem_User_UserId",
                table: "TodoItem");

            migrationBuilder.DropIndex(
                name: "IX_TodoItem_UserId",
                table: "TodoItem");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "TodoItem");
        }
    }
}
