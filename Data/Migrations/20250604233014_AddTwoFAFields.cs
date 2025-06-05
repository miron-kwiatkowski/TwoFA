using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TwoFA.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTwoFAFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GoogleAuthenticatorKey",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsTwoFactorEnabled",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "TwoFactorSecret",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GoogleAuthenticatorKey",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IsTwoFactorEnabled",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TwoFactorSecret",
                table: "AspNetUsers");
        }
    }
}
