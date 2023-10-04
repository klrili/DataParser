using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SteamBigData.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SoldInfos",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    itemNameId = table.Column<int>(type: "int", nullable: false),
                    buyerUserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    buyerAvatarUrl = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    sellerUserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    sellerAvatarUrl = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    price = table.Column<decimal>(type: "decimal(6,2)", nullable: false),
                    timestamp = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SoldInfos", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SoldInfos");
        }
    }
}
