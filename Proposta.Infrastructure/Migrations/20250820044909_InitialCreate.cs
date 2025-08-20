using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Proposta.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Propostas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", nullable: true),
                    Cpf = table.Column<string>(type: "TEXT", nullable: true),
                    Email = table.Column<string>(type: "TEXT", nullable: true),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    Tipo = table.Column<string>(type: "TEXT", nullable: true),
                    ValorSegurado = table.Column<decimal>(type: "TEXT", nullable: true),
                    Premio = table.Column<decimal>(type: "TEXT", nullable: true),
                    InicioVigencia = table.Column<DateTime>(type: "TEXT", nullable: true),
                    PrazoMeses = table.Column<int>(type: "INTEGER", nullable: false),
                    Assistencia24h = table.Column<bool>(type: "INTEGER", nullable: true),
                    Franquia = table.Column<decimal>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Propostas", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Propostas");
        }
    }
}
