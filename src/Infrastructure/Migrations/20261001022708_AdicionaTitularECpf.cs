using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlataformaNexbank.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdicionaTitularECpf : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Titular",
                table: "Contas",
                newName: "TitularNome");

            migrationBuilder.AddColumn<string>(
                name: "Cpf",
                table: "Contas",
                type: "character varying(11)",
                maxLength: 11,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Contas_Cpf",
                table: "Contas",
                column: "Cpf",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Contas_Cpf",
                table: "Contas");

            migrationBuilder.DropColumn(
                name: "Cpf",
                table: "Contas");

            migrationBuilder.RenameColumn(
                name: "TitularNome",
                table: "Contas",
                newName: "Titular");
        }
    }
}
