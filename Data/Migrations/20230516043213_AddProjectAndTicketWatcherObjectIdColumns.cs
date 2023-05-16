using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SD_340_W22SD_Final_Project_Group6.Data.Migrations
{
    public partial class AddProjectAndTicketWatcherObjectIdColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_AspNetUsers_CreatedById",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketWatchers_AspNetUsers_WatcherId",
                table: "TicketWatchers");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketWatchers_Tickets_TicketId",
                table: "TicketWatchers");

            migrationBuilder.DropIndex(
                name: "IX_TicketWatchers_WatcherId",
                table: "TicketWatchers");

            migrationBuilder.DropIndex(
                name: "IX_Projects_CreatedById",
                table: "Projects");

            migrationBuilder.AlterColumn<int>(
                name: "WatcherId",
                table: "TicketWatchers",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

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

            migrationBuilder.AlterColumn<int>(
                name: "CreatedById",
                table: "Projects",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedById1",
                table: "Projects",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TicketWatchers_WatcherId1",
                table: "TicketWatchers",
                column: "WatcherId1");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_CreatedById1",
                table: "Projects",
                column: "CreatedById1");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_AspNetUsers_CreatedById1",
                table: "Projects",
                column: "CreatedById1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TicketWatchers_AspNetUsers_WatcherId1",
                table: "TicketWatchers",
                column: "WatcherId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TicketWatchers_Tickets_TicketId",
                table: "TicketWatchers",
                column: "TicketId",
                principalTable: "Tickets",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_AspNetUsers_CreatedById1",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketWatchers_AspNetUsers_WatcherId1",
                table: "TicketWatchers");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketWatchers_Tickets_TicketId",
                table: "TicketWatchers");

            migrationBuilder.DropIndex(
                name: "IX_TicketWatchers_WatcherId1",
                table: "TicketWatchers");

            migrationBuilder.DropIndex(
                name: "IX_Projects_CreatedById1",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "WatcherId1",
                table: "TicketWatchers");

            migrationBuilder.DropColumn(
                name: "CreatedById1",
                table: "Projects");

            migrationBuilder.AlterColumn<string>(
                name: "WatcherId",
                table: "TicketWatchers",
                type: "nvarchar(450)",
                nullable: true,
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

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                table: "Projects",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TicketWatchers_WatcherId",
                table: "TicketWatchers",
                column: "WatcherId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_CreatedById",
                table: "Projects",
                column: "CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_AspNetUsers_CreatedById",
                table: "Projects",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TicketWatchers_AspNetUsers_WatcherId",
                table: "TicketWatchers",
                column: "WatcherId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TicketWatchers_Tickets_TicketId",
                table: "TicketWatchers",
                column: "TicketId",
                principalTable: "Tickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
