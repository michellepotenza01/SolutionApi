using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SolutionApi.Migrations
{
    /// <inheritdoc />
    public partial class AddAvisoBairroRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Abrigos",
                columns: table => new
                {
                    NomeAbrigo = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    Bairro = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    Tamanho = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Abrigos", x => x.NomeAbrigo);
                });

            migrationBuilder.CreateTable(
                name: "Avisos",
                columns: table => new
                {
                    TipoAviso = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    Ocorrencia = table.Column<string>(type: "NVARCHAR2(500)", maxLength: 500, nullable: false),
                    Gravidade = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    Bairro = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Avisos", x => x.TipoAviso);
                });

            migrationBuilder.CreateTable(
                name: "Pessoas",
                columns: table => new
                {
                    Nome = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    Idade = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    Bairro = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    PCD = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    Senha = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    CPF = table.Column<string>(type: "NVARCHAR2(11)", maxLength: 11, nullable: false),
                    Carreira = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pessoas", x => x.Nome);
                });

            migrationBuilder.CreateTable(
                name: "Voluntarios",
                columns: table => new
                {
                    RG = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    CPF = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    Funcao = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    NomeAbrigo = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Voluntarios", x => x.RG);
                    table.ForeignKey(
                        name: "FK_Voluntarios_Abrigos_NomeAbrigo",
                        column: x => x.NomeAbrigo,
                        principalTable: "Abrigos",
                        principalColumn: "NomeAbrigo",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Voluntarios_Pessoas_CPF",
                        column: x => x.CPF,
                        principalTable: "Pessoas",
                        principalColumn: "Nome",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Voluntarios_CPF",
                table: "Voluntarios",
                column: "CPF");

            migrationBuilder.CreateIndex(
                name: "IX_Voluntarios_NomeAbrigo",
                table: "Voluntarios",
                column: "NomeAbrigo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Avisos");

            migrationBuilder.DropTable(
                name: "Voluntarios");

            migrationBuilder.DropTable(
                name: "Abrigos");

            migrationBuilder.DropTable(
                name: "Pessoas");
        }
    }
}
