using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class callrecordnew : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Source",
                table: "CallRecords");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "CallRecords");

            migrationBuilder.AlterColumn<bool>(
                name: "RemindStatus",
                table: "CallRecords",
                type: "boolean",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);




            migrationBuilder.AddColumn<int>(
                name: "TypeId",
                table: "CallRecords",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TypeId",
                table: "CallRecords");

            migrationBuilder.AlterColumn<string>(
                name: "RemindStatus",
                table: "CallRecords",
                type: "text",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Source",
                table: "CallRecords",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "CallRecords",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
