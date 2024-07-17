using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RPG.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GameLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Character = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Time_Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Buff_Strength_Points = table.Column<int>(type: "int", nullable: false),
                    Buff_Agility_Points = table.Column<int>(type: "int", nullable: false),
                    Buff_Intelligence_Points = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameLogs", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameLogs");
        }
    }
}
