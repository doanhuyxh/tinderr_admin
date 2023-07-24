using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace tinderr.Migrations
{
    public partial class bank_account : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Bankname",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Banknumber",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bankname",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Banknumber",
                table: "AspNetUsers");
        }
    }
}
