using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediatRTest.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedWelcomeEmailSent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "WelcomeEmailSent",
                table: "Invoices",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WelcomeEmailSent",
                table: "Invoices");
        }
    }
}
