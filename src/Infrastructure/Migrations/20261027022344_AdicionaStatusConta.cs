using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlataformaNexbank.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdicionaStatusConta : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Contas",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "Ativa");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Contas");
        }
    }
}
