using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediatRTest.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameBussinsIdToBusinessId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BussinsId",
                table: "Invoices",
                newName: "BusinessId");

            migrationBuilder.RenameIndex(
                name: "IX_Invoices_BussinsId",
                table: "Invoices",
                newName: "IX_Invoices_BusinessId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BusinessId",
                table: "Invoices",
                newName: "BussinsId");

            migrationBuilder.RenameIndex(
                name: "IX_Invoices_BusinessId",
                table: "Invoices",
                newName: "IX_Invoices_BussinsId");
        }
    }
}
