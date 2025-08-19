using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class consultantlinkedinurlupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LinkedinUrl",
                table: "Consultants",
                newName: "LinkedInUrl");

            migrationBuilder.RenameColumn(
                name: "LinkedinImage",
                table: "Consultants",
                newName: "LinkedInImage");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LinkedInUrl",
                table: "Consultants",
                newName: "LinkedinUrl");

            migrationBuilder.RenameColumn(
                name: "LinkedInImage",
                table: "Consultants",
                newName: "LinkedinImage");
        }
    }
}
