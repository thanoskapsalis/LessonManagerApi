using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    public partial class DilosiMathimatvn2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dilosis_Students_studentId",
                table: "Dilosis");

            migrationBuilder.RenameColumn(
                name: "studentId",
                table: "Dilosis",
                newName: "userId");

            migrationBuilder.RenameIndex(
                name: "IX_Dilosis_studentId",
                table: "Dilosis",
                newName: "IX_Dilosis_userId");

            migrationBuilder.AddForeignKey(
                name: "FK_Dilosis_Users_userId",
                table: "Dilosis",
                column: "userId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dilosis_Users_userId",
                table: "Dilosis");

            migrationBuilder.RenameColumn(
                name: "userId",
                table: "Dilosis",
                newName: "studentId");

            migrationBuilder.RenameIndex(
                name: "IX_Dilosis_userId",
                table: "Dilosis",
                newName: "IX_Dilosis_studentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Dilosis_Students_studentId",
                table: "Dilosis",
                column: "studentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
