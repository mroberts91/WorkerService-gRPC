using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WeatherService.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Rain",
                columns: table => new
                {
                    RainID = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OneHourRainfall = table.Column<double>(nullable: true),
                    ThreeHourRainfall = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rain", x => x.RainID);
                });

            migrationBuilder.CreateTable(
                name: "Snow",
                columns: table => new
                {
                    SnowID = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OneHourSnowfall = table.Column<double>(nullable: true),
                    ThreeHourSnowfall = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Snow", x => x.SnowID);
                });

            migrationBuilder.CreateTable(
                name: "Weather",
                columns: table => new
                {
                    WeatherID = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Temperature = table.Column<double>(nullable: true),
                    Pressure = table.Column<double>(nullable: true),
                    Humidity = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weather", x => x.WeatherID);
                });

            migrationBuilder.CreateTable(
                name: "Wind",
                columns: table => new
                {
                    WindID = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Speed = table.Column<double>(nullable: true),
                    Direction = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wind", x => x.WindID);
                });

            migrationBuilder.CreateTable(
                name: "Conditions",
                columns: table => new
                {
                    ConditionsID = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    WindID = table.Column<long>(nullable: true),
                    RainID = table.Column<long>(nullable: true),
                    SnowID = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conditions", x => x.ConditionsID);
                    table.ForeignKey(
                        name: "FK_Conditions_Rain_RainID",
                        column: x => x.RainID,
                        principalTable: "Rain",
                        principalColumn: "RainID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Conditions_Snow_SnowID",
                        column: x => x.SnowID,
                        principalTable: "Snow",
                        principalColumn: "SnowID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Conditions_Wind_WindID",
                        column: x => x.WindID,
                        principalTable: "Wind",
                        principalColumn: "WindID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "City",
                columns: table => new
                {
                    ZipCode = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CityId = table.Column<int>(nullable: false),
                    CityName = table.Column<string>(nullable: true),
                    Longitude = table.Column<double>(nullable: true),
                    Lattitude = table.Column<double>(nullable: true),
                    Visibility = table.Column<double>(nullable: true),
                    WeatherID = table.Column<long>(nullable: true),
                    ConditionsID = table.Column<long>(nullable: true),
                    Updated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_City", x => x.ZipCode);
                    table.ForeignKey(
                        name: "FK_City_Conditions_ConditionsID",
                        column: x => x.ConditionsID,
                        principalTable: "Conditions",
                        principalColumn: "ConditionsID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_City_Weather_WeatherID",
                        column: x => x.WeatherID,
                        principalTable: "Weather",
                        principalColumn: "WeatherID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_City_ConditionsID",
                table: "City",
                column: "ConditionsID");

            migrationBuilder.CreateIndex(
                name: "IX_City_WeatherID",
                table: "City",
                column: "WeatherID");

            migrationBuilder.CreateIndex(
                name: "IX_Conditions_RainID",
                table: "Conditions",
                column: "RainID");

            migrationBuilder.CreateIndex(
                name: "IX_Conditions_SnowID",
                table: "Conditions",
                column: "SnowID");

            migrationBuilder.CreateIndex(
                name: "IX_Conditions_WindID",
                table: "Conditions",
                column: "WindID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "City");

            migrationBuilder.DropTable(
                name: "Conditions");

            migrationBuilder.DropTable(
                name: "Weather");

            migrationBuilder.DropTable(
                name: "Rain");

            migrationBuilder.DropTable(
                name: "Snow");

            migrationBuilder.DropTable(
                name: "Wind");
        }
    }
}
