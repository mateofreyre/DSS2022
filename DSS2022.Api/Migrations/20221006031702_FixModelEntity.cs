using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DSS2022.Api.Migrations
{
    public partial class FixModelEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ModelType");

            migrationBuilder.RenameColumn(
                name: "PlazoFabricacion",
                table: "Collections",
                newName: "ManufacturingTime");

            migrationBuilder.RenameColumn(
                name: "FechaLanzamientoEstimada",
                table: "Collections",
                newName: "ReleaseDate");

            migrationBuilder.AddColumn<int>(
                name: "ModelType",
                table: "Model",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ModelType",
                table: "Model");

            migrationBuilder.RenameColumn(
                name: "ReleaseDate",
                table: "Collections",
                newName: "FechaLanzamientoEstimada");

            migrationBuilder.RenameColumn(
                name: "ManufacturingTime",
                table: "Collections",
                newName: "PlazoFabricacion");

            migrationBuilder.CreateTable(
                name: "ModelType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CollectionId = table.Column<int>(type: "integer", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModelType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ModelType_Collections_CollectionId",
                        column: x => x.CollectionId,
                        principalTable: "Collections",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ModelType_CollectionId",
                table: "ModelType",
                column: "CollectionId");
        }
    }
}
