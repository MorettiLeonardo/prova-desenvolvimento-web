﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GuilhermeGabriel.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Consumos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Cpf = table.Column<string>(type: "TEXT", nullable: false),
                    Mes = table.Column<int>(type: "INTEGER", nullable: false),
                    Ano = table.Column<int>(type: "INTEGER", nullable: false),
                    M3Consumidos = table.Column<double>(type: "REAL", nullable: false),
                    Bandeira = table.Column<string>(type: "TEXT", nullable: false),
                    PossuiEsgoto = table.Column<bool>(type: "INTEGER", nullable: false),
                    Tarifa = table.Column<double>(type: "REAL", nullable: false),
                    ValorAgua = table.Column<double>(type: "REAL", nullable: false),
                    ConsumoFaturado = table.Column<double>(type: "REAL", nullable: false),
                    AdicionalBandeira = table.Column<double>(type: "REAL", nullable: false),
                    TaxaDeEsgoto = table.Column<double>(type: "REAL", nullable: false),
                    TotalGeral = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Consumos", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Consumos");
        }
    }
}
