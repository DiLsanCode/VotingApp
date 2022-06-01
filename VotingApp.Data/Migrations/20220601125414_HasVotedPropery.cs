using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VotingApp.Data.Migrations
{
    public partial class HasVotedPropery : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasVoted",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasVoted",
                table: "AspNetUsers");
        }
    }
}
