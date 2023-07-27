using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace tinderr.Migrations
{
    public partial class chang_game : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "item3",
                table: "HistoryGame");

            migrationBuilder.DropColumn(
                name: "item4",
                table: "HistoryGame");

            migrationBuilder.AlterColumn<int>(
                name: "item2",
                table: "HistoryGame",
                type: "int",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<int>(
                name: "item1",
                table: "HistoryGame",
                type: "int",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "item2",
                table: "HistoryGame",
                type: "bit",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<bool>(
                name: "item1",
                table: "HistoryGame",
                type: "bit",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<bool>(
                name: "item3",
                table: "HistoryGame",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "item4",
                table: "HistoryGame",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
