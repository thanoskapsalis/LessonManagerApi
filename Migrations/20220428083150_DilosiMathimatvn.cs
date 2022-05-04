using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
  public partial class DilosiMathimatvn : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.CreateTable(
          name: "Dilosis",
          columns: table => new
          {
            Id = table.Column<int>(type: "int", nullable: false)
                  .Annotation("SqlServer:Identity", "1, 1"),
            teachClassId = table.Column<int>(type: "int", nullable: false),
            studentId = table.Column<int>(type: "int", nullable: false),
            examMark = table.Column<float>(type: "real", nullable: false),
            labMark = table.Column<float>(type: "real", nullable: false),
            finalMark = table.Column<float>(type: "real", nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_Dilosis", x => x.Id);
            table.ForeignKey(
                      name: "FK_Dilosis_Students_studentId",
                      column: x => x.studentId,
                      principalTable: "Students",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
            table.ForeignKey(
                      name: "FK_Dilosis_TeachClasses_teachClassId",
                      column: x => x.teachClassId,
                      principalTable: "TeachClasses",
                      principalColumn: "Id"

                    );
          });

      migrationBuilder.CreateIndex(
          name: "IX_Dilosis_studentId",
          table: "Dilosis",
          column: "studentId");

      migrationBuilder.CreateIndex(
          name: "IX_Dilosis_teachClassId",
          table: "Dilosis",
          column: "teachClassId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropTable(
          name: "Dilosis");
    }
  }
}
