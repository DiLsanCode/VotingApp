using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VotingApp.Data.Migrations
{
    public partial class ParticipantVoteCountProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VoteCount",
                table: "Participants",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VoteCount",
                table: "Participants");
        }
    }
}
