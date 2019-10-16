using Microsoft.EntityFrameworkCore.Migrations;

namespace WeatherService.Migrations
{
    public partial class AddedWeatherColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ConditionDescription",
                table: "Conditions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ConditionIcon",
                table: "Conditions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Conditon",
                table: "Conditions",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConditionDescription",
                table: "Conditions");

            migrationBuilder.DropColumn(
                name: "ConditionIcon",
                table: "Conditions");

            migrationBuilder.DropColumn(
                name: "Conditon",
                table: "Conditions");
        }
    }
}
