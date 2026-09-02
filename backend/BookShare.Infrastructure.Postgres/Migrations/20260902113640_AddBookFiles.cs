using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookShare.Infrastructure.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class AddBookFiles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_books_FileHash",
                table: "books");

            migrationBuilder.DropColumn(
                name: "FileHash",
                table: "books");

            migrationBuilder.DropColumn(
                name: "FileKey",
                table: "books");

            migrationBuilder.DropColumn(
                name: "FileSize",
                table: "books");

            migrationBuilder.CreateTable(
                name: "book_files",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BookId = table.Column<Guid>(type: "uuid", nullable: false),
                    Format = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    FileKey = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    FileHash = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_book_files", x => x.Id);
                    table.ForeignKey(
                        name: "FK_book_files_books_BookId",
                        column: x => x.BookId,
                        principalTable: "books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_book_files_BookId_Format",
                table: "book_files",
                columns: new[] { "BookId", "Format" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_book_files_FileHash",
                table: "book_files",
                column: "FileHash",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "book_files");

            migrationBuilder.AddColumn<string>(
                name: "FileHash",
                table: "books",
                type: "character varying(64)",
                maxLength: 64,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FileKey",
                table: "books",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "FileSize",
                table: "books",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_books_FileHash",
                table: "books",
                column: "FileHash",
                unique: true);
        }
    }
}
