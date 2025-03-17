using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Server.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class refactor_order_ordernumber_nvarchar16 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "OrderNumber",
                table: "Orders",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(16)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "OrderNumber",
                table: "Orders",
                type: "varchar(16)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
