﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Server.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class i_changed_recipedetails_for_details : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Recipes_ProductId",
                table: "Recipes",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Recipes_Products_ProductId",
                table: "Recipes",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recipes_Products_ProductId",
                table: "Recipes");

            migrationBuilder.DropIndex(
                name: "IX_Recipes_ProductId",
                table: "Recipes");
        }
    }
}
