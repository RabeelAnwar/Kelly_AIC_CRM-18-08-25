using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class docupload_release : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Client",
                table: "CallRecords");

            migrationBuilder.AddColumn<int>(
                name: "ClientId",
                table: "CallRecords",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ConsultantId",
                table: "CallRecords",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LeadId",
                table: "CallRecords",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ManagerId",
                table: "CallRecords",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Source",
                table: "CallRecords",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "DocumentUploads",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<int>(type: "integer", nullable: false),
                    DocumentName = table.Column<string>(type: "text", nullable: true),
                    DocumentTypeId = table.Column<int>(type: "integer", nullable: false),
                    ClientId = table.Column<int>(type: "integer", nullable: true),
                    ClientManagerId = table.Column<int>(type: "integer", nullable: true),
                    ConsultantId = table.Column<int>(type: "integer", nullable: true),
                    DocumentTypeName = table.Column<string>(type: "text", nullable: true),
                    Source = table.Column<string>(type: "text", nullable: false),
                    DocumentFileName = table.Column<string>(type: "text", nullable: true),
                    CreatorUserId = table.Column<string>(type: "text", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifierUserId = table.Column<string>(type: "text", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeleterUserId = table.Column<string>(type: "text", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentUploads", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DocumentUploads");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "CallRecords");

            migrationBuilder.DropColumn(
                name: "ConsultantId",
                table: "CallRecords");

            migrationBuilder.DropColumn(
                name: "LeadId",
                table: "CallRecords");

            migrationBuilder.DropColumn(
                name: "ManagerId",
                table: "CallRecords");

            migrationBuilder.DropColumn(
                name: "Source",
                table: "CallRecords");

            migrationBuilder.AddColumn<int>(
                name: "Client",
                table: "CallRecords",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
