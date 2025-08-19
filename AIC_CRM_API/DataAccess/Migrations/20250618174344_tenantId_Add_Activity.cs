using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class tenantId_Add_Activity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "ConsultantActivities",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatorUserId",
                table: "ConsultantActivities",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DeleterUserId",
                table: "ConsultantActivities",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionTime",
                table: "ConsultantActivities",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ConsultantActivities",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModificationTime",
                table: "ConsultantActivities",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifierUserId",
                table: "ConsultantActivities",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "ConsultantActivities",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "ConsultantActivities");

            migrationBuilder.DropColumn(
                name: "CreatorUserId",
                table: "ConsultantActivities");

            migrationBuilder.DropColumn(
                name: "DeleterUserId",
                table: "ConsultantActivities");

            migrationBuilder.DropColumn(
                name: "DeletionTime",
                table: "ConsultantActivities");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ConsultantActivities");

            migrationBuilder.DropColumn(
                name: "LastModificationTime",
                table: "ConsultantActivities");

            migrationBuilder.DropColumn(
                name: "LastModifierUserId",
                table: "ConsultantActivities");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "ConsultantActivities");
        }
    }
}
