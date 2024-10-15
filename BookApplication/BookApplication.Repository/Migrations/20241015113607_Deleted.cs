using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookApplication.Repository.Migrations
{
    /// <inheritdoc />
    public partial class Deleted : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookInOrders");

            migrationBuilder.AddColumn<Guid>(
                name: "shoppingCartId",
                table: "Orders",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Orders_shoppingCartId",
                table: "Orders",
                column: "shoppingCartId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_ShoppingCarts_shoppingCartId",
                table: "Orders",
                column: "shoppingCartId",
                principalTable: "ShoppingCarts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_ShoppingCarts_shoppingCartId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_shoppingCartId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "shoppingCartId",
                table: "Orders");

            migrationBuilder.CreateTable(
                name: "BookInOrders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BookId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookInOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookInOrders_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BookInOrders_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookInOrders_BookId",
                table: "BookInOrders",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_BookInOrders_OrderId",
                table: "BookInOrders",
                column: "OrderId");
        }
    }
}
