using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SD_340_W22SD_Final_Project_Group6.Data.Migrations
{
    public partial class AddCommentTicketIdColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TicketWatchers_AspNetUsers_WatcherId1",
                table: "TicketWatchers");

            migrationBuilder.DropIndex(
                name: "IX_TicketWatchers_WatcherId1",
                table: "TicketWatchers");

            migrationBuilder.DropColumn(
                name: "WatcherId1",
                table: "TicketWatchers");

            migrationBuilder.AlterColumn<string>(
                name: "WatcherId",
                table: "TicketWatchers",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TicketId",
                table: "TicketWatchers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TicketWatchers_WatcherId",
                table: "TicketWatchers",
                column: "WatcherId");

            migrationBuilder.AddForeignKey(
                name: "FK_TicketWatchers_AspNetUsers_WatcherId",
                table: "TicketWatchers",
                column: "WatcherId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TicketWatchers_AspNetUsers_WatcherId",
                table: "TicketWatchers");

            migrationBuilder.DropIndex(
                name: "IX_TicketWatchers_WatcherId",
                table: "TicketWatchers");

            migrationBuilder.AlterColumn<int>(
                name: "WatcherId",
                table: "TicketWatchers",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<int>(
                name: "TicketId",
                table: "TicketWatchers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "WatcherId1",
                table: "TicketWatchers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TicketWatchers_WatcherId1",
                table: "TicketWatchers",
                column: "WatcherId1");

            migrationBuilder.AddForeignKey(
                name: "FK_TicketWatchers_AspNetUsers_WatcherId1",
                table: "TicketWatchers",
                column: "WatcherId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
