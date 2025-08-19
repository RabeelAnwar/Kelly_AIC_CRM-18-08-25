using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class requisition_client : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClientRequisitions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<int>(type: "integer", nullable: false),
                    ClientId = table.Column<int>(type: "integer", nullable: false),
                    InternalReqCoordinatorId = table.Column<int>(type: "integer", nullable: false),
                    RequisitionType = table.Column<string>(type: "text", nullable: false),
                    Priority = table.Column<int>(type: "integer", nullable: false),
                    JobTitle = table.Column<string>(type: "text", nullable: false),
                    ClientReqNumber = table.Column<string>(type: "text", nullable: true),
                    ManagerId = table.Column<int>(type: "integer", nullable: false),
                    SalesRepId = table.Column<int>(type: "integer", nullable: false),
                    RecruiterAssignedId = table.Column<List<int>>(type: "integer[]", nullable: false),
                    Location = table.Column<string>(type: "text", nullable: true),
                    Duration = table.Column<string>(type: "text", nullable: true),
                    DurationTypes = table.Column<string>(type: "text", nullable: true),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    NumberOfPositions = table.Column<int>(type: "integer", nullable: false),
                    Comments = table.Column<string>(type: "text", nullable: true),
                    ProjectDepartmentOverview = table.Column<string>(type: "text", nullable: true),
                    JobDescription = table.Column<string>(type: "text", nullable: true),
                    PayRate = table.Column<string>(type: "text", nullable: true),
                    BillRate = table.Column<string>(type: "text", nullable: true),
                    Hours = table.Column<string>(type: "text", nullable: true),
                    Overtime = table.Column<string>(type: "text", nullable: true),
                    InterviewProcesses = table.Column<string>(type: "text", nullable: true),
                    PhoneHireIfOutOfArea = table.Column<string>(type: "text", nullable: true),
                    ClientMarkup = table.Column<string>(type: "text", nullable: true),
                    BillRateHighestBeforeResumeNotSent = table.Column<string>(type: "text", nullable: true),
                    SecondaryContact = table.Column<string>(type: "text", nullable: true),
                    HiringManagerVop = table.Column<string>(type: "text", nullable: true),
                    OtherWaysToFillPosition = table.Column<string>(type: "text", nullable: true),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    Responsibilities = table.Column<string>(type: "text", nullable: true),
                    Qualifications = table.Column<string>(type: "text", nullable: true),
                    SearchString1 = table.Column<string>(type: "text", nullable: true),
                    SearchString2 = table.Column<string>(type: "text", nullable: true),
                    SearchString3 = table.Column<string>(type: "text", nullable: true),
                    CodingValue = table.Column<int>(type: "integer", nullable: true),
                    AnalysisValue = table.Column<int>(type: "integer", nullable: true),
                    TestingValue = table.Column<int>(type: "integer", nullable: true),
                    OtherValue = table.Column<int>(type: "integer", nullable: true),
                    Hardware = table.Column<string>(type: "text", nullable: true),
                    OS = table.Column<string>(type: "text", nullable: true),
                    Languages = table.Column<string>(type: "text", nullable: true),
                    Databases = table.Column<string>(type: "text", nullable: true),
                    Protocols = table.Column<string>(type: "text", nullable: true),
                    SoftwareStandards = table.Column<string>(type: "text", nullable: true),
                    Others = table.Column<string>(type: "text", nullable: true),
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
                    table.PrimaryKey("PK_ClientRequisitions", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientRequisitions");
        }
    }
}
