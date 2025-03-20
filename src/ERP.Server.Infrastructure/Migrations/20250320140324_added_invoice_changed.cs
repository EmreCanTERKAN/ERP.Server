using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Server.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class added_invoice_changed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DepotId",
                table: "InvoiceDetail",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceDetail_DepotId",
                table: "InvoiceDetail",
                column: "DepotId");

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceDetail_Depots_DepotId",
                table: "InvoiceDetail",
                column: "DepotId",
                principalTable: "Depots",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceDetail_Depots_DepotId",
                table: "InvoiceDetail");

            migrationBuilder.DropIndex(
                name: "IX_InvoiceDetail_DepotId",
                table: "InvoiceDetail");

            migrationBuilder.DropColumn(
                name: "DepotId",
                table: "InvoiceDetail");
        }
    }
}
