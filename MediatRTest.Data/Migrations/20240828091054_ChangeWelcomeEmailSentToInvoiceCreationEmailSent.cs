using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediatRTest.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeWelcomeEmailSentToInvoiceCreationEmailSent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WelcomeEmailSent",
                table: "Invoices",
                newName: "InvoiceCreationEmailSent");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "InvoiceCreationEmailSent",
                table: "Invoices",
                newName: "WelcomeEmailSent");
        }
    }
}
