using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BetStatusTracker.Repositories.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BetGroup",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BetClientRef = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    SequenceNo = table.Column<int>(unicode: false, maxLength: 10, nullable: false),
                    BetCombinationNo = table.Column<int>(nullable: false),
                    BetSubmittedTime = table.Column<DateTimeOffset>(nullable: false),
                    CustomerId = table.Column<string>(unicode: false, maxLength: 20, nullable: false),
                    StakeAmount = table.Column<decimal>(nullable: false),
                    UpdatedDate = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BetGroup", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BetGroup_BetClientRef_SequenceNo",
                table: "BetGroup",
                columns: new[] { "BetClientRef", "SequenceNo" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BetGroup");
        }
    }
}
