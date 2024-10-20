using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace APIDemo161024.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Book",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Book", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Book",
                columns: new[] { "Id", "Description", "Price", "Quantity", "Title" },
                values: new object[,]
                {
                    { 1, "A novel written by American author F. Scott Fitzgerald.", 10.99, 50, "The Great Gatsby" },
                    { 2, "A dystopian social science fiction novel and cautionary tale, written by the English writer George Orwell.", 8.9900000000000002, 100, "1984" },
                    { 3, "A novel by Harper Lee published in 1960.", 12.99, 30, "To Kill a Mockingbird" },
                    { 4, "A romantic novel of manners written by Jane Austen.", 9.9900000000000002, 20, "Pride and Prejudice" },
                    { 5, "A novel by J. D. Salinger, partially published in serial form in 1945–1946.", 11.99, 40, "The Catcher in the Rye" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Book");
        }
    }
}
