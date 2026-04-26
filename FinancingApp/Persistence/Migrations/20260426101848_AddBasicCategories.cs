using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinancingApp.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddBasicCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO Categories (Name) " +
                "VALUES ('Einkommen'), ('Gesundheit'), ('Lebenshaltung'), ('Lohn'), ('Sonstiges');");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
