using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SteamBigData.Migrations
{
    /// <inheritdoc />
    public partial class AddItemInfoObject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ItemInfoid",
                table: "SoldInfos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ItemInfo",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    itemId = table.Column<int>(type: "int", nullable: false),
                    itemName = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemInfo", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SoldInfos_ItemInfoid",
                table: "SoldInfos",
                column: "ItemInfoid");

            migrationBuilder.AddForeignKey(
                name: "FK_SoldInfos_ItemInfo_ItemInfoid",
                table: "SoldInfos",
                column: "ItemInfoid",
                principalTable: "ItemInfo",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SoldInfos_ItemInfo_ItemInfoid",
                table: "SoldInfos");

            migrationBuilder.DropTable(
                name: "ItemInfo");

            migrationBuilder.DropIndex(
                name: "IX_SoldInfos_ItemInfoid",
                table: "SoldInfos");

            migrationBuilder.DropColumn(
                name: "ItemInfoid",
                table: "SoldInfos");
        }
    }
}
