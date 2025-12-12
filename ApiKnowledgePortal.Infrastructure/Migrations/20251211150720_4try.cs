using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiKnowledgePortal.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class _4try : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Url",
                table: "SwaggerSources",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "SwaggerSources",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AddColumn<Guid>(
                name: "SwaggerSourceId",
                table: "ApiSpecs",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ApiSpecs_SwaggerSourceId",
                table: "ApiSpecs",
                column: "SwaggerSourceId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApiSpecs_SwaggerSources_SwaggerSourceId",
                table: "ApiSpecs",
                column: "SwaggerSourceId",
                principalTable: "SwaggerSources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApiSpecs_SwaggerSources_SwaggerSourceId",
                table: "ApiSpecs");

            migrationBuilder.DropIndex(
                name: "IX_ApiSpecs_SwaggerSourceId",
                table: "ApiSpecs");

            migrationBuilder.DropColumn(
                name: "SwaggerSourceId",
                table: "ApiSpecs");

            migrationBuilder.AlterColumn<string>(
                name: "Url",
                table: "SwaggerSources",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "SwaggerSources",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
