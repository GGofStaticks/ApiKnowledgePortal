using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiKnowledgePortal.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class _5try : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApiSpecs_SwaggerSources_SwaggerSourceId",
                table: "ApiSpecs");

            migrationBuilder.AddForeignKey(
                name: "FK_ApiSpecs_SwaggerSources_SwaggerSourceId",
                table: "ApiSpecs",
                column: "SwaggerSourceId",
                principalTable: "SwaggerSources",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApiSpecs_SwaggerSources_SwaggerSourceId",
                table: "ApiSpecs");

            migrationBuilder.AddForeignKey(
                name: "FK_ApiSpecs_SwaggerSources_SwaggerSourceId",
                table: "ApiSpecs",
                column: "SwaggerSourceId",
                principalTable: "SwaggerSources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
