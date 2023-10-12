using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TwitterAPI.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUniqueIdx : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Tweets_ReplyToId",
                table: "Tweets");

            migrationBuilder.CreateIndex(
                name: "IX_Tweets_ReplyToId",
                table: "Tweets",
                column: "ReplyToId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Tweets_ReplyToId",
                table: "Tweets");

            migrationBuilder.CreateIndex(
                name: "IX_Tweets_ReplyToId",
                table: "Tweets",
                column: "ReplyToId",
                unique: true);
        }
    }
}
