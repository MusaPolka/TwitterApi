using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RepositoryLayer.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    AccountType = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "Public"),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "Active"),
                    PhoneNumber = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    Bio = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Location = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FollowersFollowings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SourceUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FollowedUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FollowersFollowings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FollowersFollowings_Users_FollowedUserId",
                        column: x => x.FollowedUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FollowersFollowings_Users_SourceUserId",
                        column: x => x.SourceUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SendersRecievers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SenderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RecieverId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SendersRecievers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SendersRecievers_Users_RecieverId",
                        column: x => x.RecieverId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SendersRecievers_Users_SenderId",
                        column: x => x.SenderId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Tweets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(140)", maxLength: 140, nullable: false),
                    AttachedFile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tweets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tweets_Users_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TweetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(140)", maxLength: 140, nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AttachedFile = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_Tweets_TweetId",
                        column: x => x.TweetId,
                        principalTable: "Tweets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LikesLikedBy",
                columns: table => new
                {
                    LikedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LikesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LikesLikedBy", x => new { x.LikedById, x.LikesId });
                    table.ForeignKey(
                        name: "FK_LikesLikedBy_Tweets_LikesId",
                        column: x => x.LikesId,
                        principalTable: "Tweets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LikesLikedBy_Users_LikedById",
                        column: x => x.LikedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_TweetId",
                table: "Comments",
                column: "TweetId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserId",
                table: "Comments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_FollowersFollowings_FollowedUserId",
                table: "FollowersFollowings",
                column: "FollowedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_FollowersFollowings_SourceUserId",
                table: "FollowersFollowings",
                column: "SourceUserId");

            migrationBuilder.CreateIndex(
                name: "IX_LikesLikedBy_LikesId",
                table: "LikesLikedBy",
                column: "LikesId");

            migrationBuilder.CreateIndex(
                name: "IX_SendersRecievers_RecieverId",
                table: "SendersRecievers",
                column: "RecieverId");

            migrationBuilder.CreateIndex(
                name: "IX_SendersRecievers_SenderId",
                table: "SendersRecievers",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Tweets_OwnerId",
                table: "Tweets",
                column: "OwnerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "FollowersFollowings");

            migrationBuilder.DropTable(
                name: "LikesLikedBy");

            migrationBuilder.DropTable(
                name: "SendersRecievers");

            migrationBuilder.DropTable(
                name: "Tweets");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
