using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eTrainingSolution.DbMigrator.Migrations
{
    /// <inheritdoc />
    public partial class v5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SchoolID",
                table: "Classes",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Classes_SchoolID",
                table: "Classes",
                column: "SchoolID");

            migrationBuilder.AddForeignKey(
                name: "FK_Classes_Schools_SchoolID",
                table: "Classes",
                column: "SchoolID",
                principalTable: "Schools",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Classes_Schools_SchoolID",
                table: "Classes");

            migrationBuilder.DropIndex(
                name: "IX_Classes_SchoolID",
                table: "Classes");

            migrationBuilder.DropColumn(
                name: "SchoolID",
                table: "Classes");
        }
    }
}
