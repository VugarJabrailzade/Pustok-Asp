using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pustok.Migrations
{
    public partial class Change_BasketPr_ColorId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BasketProducts_Colors_ColorID",
                table: "BasketProducts");

            migrationBuilder.RenameColumn(
                name: "ColorID",
                table: "BasketProducts",
                newName: "ColorId");

            migrationBuilder.RenameIndex(
                name: "IX_BasketProducts_ColorID",
                table: "BasketProducts",
                newName: "IX_BasketProducts_ColorId");

            migrationBuilder.AddForeignKey(
                name: "FK_BasketProducts_Colors_ColorId",
                table: "BasketProducts",
                column: "ColorId",
                principalTable: "Colors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BasketProducts_Colors_ColorId",
                table: "BasketProducts");

            migrationBuilder.RenameColumn(
                name: "ColorId",
                table: "BasketProducts",
                newName: "ColorID");

            migrationBuilder.RenameIndex(
                name: "IX_BasketProducts_ColorId",
                table: "BasketProducts",
                newName: "IX_BasketProducts_ColorID");

            migrationBuilder.AddForeignKey(
                name: "FK_BasketProducts_Colors_ColorID",
                table: "BasketProducts",
                column: "ColorID",
                principalTable: "Colors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
