using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TwitterAPI.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Color = table.Column<string>(type: "character varying(6)", maxLength: 6, nullable: false),
                    At = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Username = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    Bio = table.Column<string>(type: "character varying(144)", maxLength: 144, nullable: false),
                    City = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Country = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    BirthDate = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tweets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OwnerId = table.Column<int>(type: "integer", nullable: false),
                    Text = table.Column<string>(type: "character varying(144)", maxLength: 144, nullable: false),
                    ReplyToId = table.Column<int>(type: "integer", nullable: true),
                    Likes = table.Column<int>(type: "integer", nullable: false),
                    PostTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tweets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tweets_Tweets_ReplyToId",
                        column: x => x.ReplyToId,
                        principalTable: "Tweets",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Tweets_Users_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LikeHistory",
                columns: table => new
                {
                    TweetId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LikeHistory", x => new { x.TweetId, x.UserId });
                    table.ForeignKey(
                        name: "FK_LikeHistory_Tweets_TweetId",
                        column: x => x.TweetId,
                        principalTable: "Tweets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LikeHistory_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TweetsReplies",
                columns: table => new
                {
                    ReplyId = table.Column<int>(type: "integer", nullable: false),
                    ParentId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TweetsReplies", x => new { x.ReplyId, x.ParentId });
                    table.ForeignKey(
                        name: "FK_TweetsReplies_Tweets_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Tweets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TweetsReplies_Tweets_ReplyId",
                        column: x => x.ReplyId,
                        principalTable: "Tweets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LikeHistory_UserId",
                table: "LikeHistory",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Tweets_OwnerId",
                table: "Tweets",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Tweets_ReplyToId",
                table: "Tweets",
                column: "ReplyToId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TweetsReplies_ParentId",
                table: "TweetsReplies",
                column: "ParentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LikeHistory");

            migrationBuilder.DropTable(
                name: "TweetsReplies");

            migrationBuilder.DropTable(
                name: "Tweets");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
