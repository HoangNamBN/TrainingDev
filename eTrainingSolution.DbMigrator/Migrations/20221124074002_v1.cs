using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eTrainingSolution.DbMigrator.Migrations
{
    /// <inheritdoc />
    public partial class v1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Faculty_FacultysID",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Schools_SchoolsId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_FacultysID",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "FacultysID",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IDClass",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "SchoolsId",
                table: "Users",
                newName: "SchoolID");

            migrationBuilder.RenameColumn(
                name: "IDSchool",
                table: "Users",
                newName: "FacultyID");

            migrationBuilder.RenameColumn(
                name: "IDFacultys",
                table: "Users",
                newName: "ClassID");

            migrationBuilder.RenameIndex(
                name: "IX_Users_SchoolsId",
                table: "Users",
                newName: "IX_Users_SchoolID");

            migrationBuilder.CreateIndex(
                name: "IX_Users_FacultyID",
                table: "Users",
                column: "FacultyID");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Faculty_FacultyID",
                table: "Users",
                column: "FacultyID",
                principalTable: "Faculty",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Schools_SchoolID",
                table: "Users",
                column: "SchoolID",
                principalTable: "Schools",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Faculty_FacultyID",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Schools_SchoolID",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_FacultyID",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "SchoolID",
                table: "Users",
                newName: "SchoolsId");

            migrationBuilder.RenameColumn(
                name: "FacultyID",
                table: "Users",
                newName: "IDSchool");

            migrationBuilder.RenameColumn(
                name: "ClassID",
                table: "Users",
                newName: "IDFacultys");

            migrationBuilder.RenameIndex(
                name: "IX_Users_SchoolID",
                table: "Users",
                newName: "IX_Users_SchoolsId");

            migrationBuilder.AddColumn<Guid>(
                name: "FacultysID",
                table: "Users",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "IDClass",
                table: "Users",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_FacultysID",
                table: "Users",
                column: "FacultysID");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Faculty_FacultysID",
                table: "Users",
                column: "FacultysID",
                principalTable: "Faculty",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Schools_SchoolsId",
                table: "Users",
                column: "SchoolsId",
                principalTable: "Schools",
                principalColumn: "Id");
        }
    }
}
