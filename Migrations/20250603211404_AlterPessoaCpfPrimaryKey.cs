using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SolutionApi.Migrations
{
    /// <inheritdoc />
    public partial class AlterPessoaCpfPrimaryKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Voluntarios_Pessoas_CPF",
                table: "Voluntarios");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Pessoas",
                table: "Pessoas");

            migrationBuilder.AlterColumn<string>(
                name: "CPF",
                table: "Voluntarios",
                type: "NVARCHAR2(11)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR2(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Nome",
                table: "Pessoas",
                type: "NVARCHAR2(2000)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR2(450)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Pessoas",
                table: "Pessoas",
                column: "CPF");

            migrationBuilder.AddForeignKey(
                name: "FK_Voluntarios_Pessoas_CPF",
                table: "Voluntarios",
                column: "CPF",
                principalTable: "Pessoas",
                principalColumn: "CPF",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Voluntarios_Pessoas_CPF",
                table: "Voluntarios");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Pessoas",
                table: "Pessoas");

            migrationBuilder.AlterColumn<string>(
                name: "CPF",
                table: "Voluntarios",
                type: "NVARCHAR2(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR2(11)");

            migrationBuilder.AlterColumn<string>(
                name: "Nome",
                table: "Pessoas",
                type: "NVARCHAR2(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR2(2000)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Pessoas",
                table: "Pessoas",
                column: "Nome");

            migrationBuilder.AddForeignKey(
                name: "FK_Voluntarios_Pessoas_CPF",
                table: "Voluntarios",
                column: "CPF",
                principalTable: "Pessoas",
                principalColumn: "Nome",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
