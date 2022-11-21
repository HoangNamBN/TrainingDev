using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eTrainingSolution.DbMigrator.Migrations
{
    /// <inheritdoc />
    public partial class v2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Classes_Faculty_FacultysID",
                table: "Classes");

            migrationBuilder.DropForeignKey(
                name: "FK_Faculty_Schools_SchoolsId",
                table: "Faculty");

            migrationBuilder.RenameColumn(
                name: "SchoolsId",
                table: "Faculty",
                newName: "SchoolID");

            migrationBuilder.RenameIndex(
                name: "IX_Faculty_SchoolsId",
                table: "Faculty",
                newName: "IX_Faculty_SchoolID");

            migrationBuilder.RenameColumn(
                name: "FacultysID",
                table: "Classes",
                newName: "FacultyID");

            migrationBuilder.RenameIndex(
                name: "IX_Classes_FacultysID",
                table: "Classes",
                newName: "IX_Classes_FacultyID");

            migrationBuilder.AddForeignKey(
                name: "FK_Classes_Faculty_FacultyID",
                table: "Classes",
                column: "FacultyID",
                principalTable: "Faculty",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Faculty_Schools_SchoolID",
                table: "Faculty",
                column: "SchoolID",
                principalTable: "Schools",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Classes_Faculty_FacultyID",
                table: "Classes");

            migrationBuilder.DropForeignKey(
                name: "FK_Faculty_Schools_SchoolID",
                table: "Faculty");

            migrationBuilder.RenameColumn(
                name: "SchoolID",
                table: "Faculty",
                newName: "SchoolsId");

            migrationBuilder.RenameIndex(
                name: "IX_Faculty_SchoolID",
                table: "Faculty",
                newName: "IX_Faculty_SchoolsId");

            migrationBuilder.RenameColumn(
                name: "FacultyID",
                table: "Classes",
                newName: "FacultysID");

            migrationBuilder.RenameIndex(
                name: "IX_Classes_FacultyID",
                table: "Classes",
                newName: "IX_Classes_FacultysID");

            migrationBuilder.AddForeignKey(
                name: "FK_Classes_Faculty_FacultysID",
                table: "Classes",
                column: "FacultysID",
                principalTable: "Faculty",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Faculty_Schools_SchoolsId",
                table: "Faculty",
                column: "SchoolsId",
                principalTable: "Schools",
                principalColumn: "Id");
        }
    }
}
