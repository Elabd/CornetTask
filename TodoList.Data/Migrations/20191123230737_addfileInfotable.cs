using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TodoList.Data.Migrations
{
    public partial class addfileInfotable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ToDos",
                table: "ToDos");

            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "ToDos");

            migrationBuilder.RenameTable(
                name: "ToDos",
                newName: "Todo");

            migrationBuilder.AddColumn<Guid>(
                name: "FileTodoId",
                table: "Todo",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Todo",
                table: "Todo",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "File",
                columns: table => new
                {
                    TodoId = table.Column<Guid>(nullable: false),
                    Path = table.Column<string>(maxLength: 500, nullable: true),
                    Size = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_File", x => x.TodoId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Todo_FileTodoId",
                table: "Todo",
                column: "FileTodoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Todo_File_FileTodoId",
                table: "Todo",
                column: "FileTodoId",
                principalTable: "File",
                principalColumn: "TodoId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Todo_File_FileTodoId",
                table: "Todo");

            migrationBuilder.DropTable(
                name: "File");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Todo",
                table: "Todo");

            migrationBuilder.DropIndex(
                name: "IX_Todo_FileTodoId",
                table: "Todo");

            migrationBuilder.DropColumn(
                name: "FileTodoId",
                table: "Todo");

            migrationBuilder.RenameTable(
                name: "Todo",
                newName: "ToDos");

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "ToDos",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ToDos",
                table: "ToDos",
                column: "Id");
        }
    }
}
