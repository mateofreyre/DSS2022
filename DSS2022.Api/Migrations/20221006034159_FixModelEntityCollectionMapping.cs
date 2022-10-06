using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DSS2022.Api.Migrations
{
    public partial class FixModelEntityCollectionMapping : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Model_Collections_CollectionId",
                table: "Model");

            migrationBuilder.AlterColumn<int>(
                name: "CollectionId",
                table: "Model",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Model_Collections_CollectionId",
                table: "Model",
                column: "CollectionId",
                principalTable: "Collections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Model_Collections_CollectionId",
                table: "Model");

            migrationBuilder.AlterColumn<int>(
                name: "CollectionId",
                table: "Model",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_Model_Collections_CollectionId",
                table: "Model",
                column: "CollectionId",
                principalTable: "Collections",
                principalColumn: "Id");
        }
    }
}
