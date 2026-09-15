using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dangl.OpenCDE.Migrations
{
    /// <inheritdoc />
    public partial class InitialSqlite : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FileMimeTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    MimeType = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileMimeTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 400, nullable: false),
                    IdenticonId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedAtUtc = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Files",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    MimeTypeId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedAtUtc = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "TEXT", nullable: true),
                    ContainerName = table.Column<string>(type: "TEXT", maxLength: 63, nullable: false),
                    FileName = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    SizeInBytes = table.Column<long>(type: "INTEGER", nullable: false),
                    FileAvailableInStorage = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Files", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Files_FileMimeTypes_MimeTypeId",
                        column: x => x.MimeTypeId,
                        principalTable: "FileMimeTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Files_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OpenCdeDocumentDownloadSessions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ValidUntilUtc = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    ClientCallbackUrl = table.Column<string>(type: "TEXT", nullable: false),
                    AuthenticationInformationJson = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpenCdeDocumentDownloadSessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OpenCdeDocumentDownloadSessions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OpenCdeDocumentUploadSessions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ValidUntilUtc = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    ClientCallbackUrl = table.Column<string>(type: "TEXT", nullable: false),
                    AuthenticationInformationJson = table.Column<string>(type: "TEXT", nullable: true),
                    SelectedProjectId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpenCdeDocumentUploadSessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OpenCdeDocumentUploadSessions_Projects_SelectedProjectId",
                        column: x => x.SelectedProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OpenCdeDocumentUploadSessions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Documents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ProjectId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 400, nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    FileId = table.Column<Guid>(type: "TEXT", nullable: true),
                    CreatedAtUtc = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Documents_Files_FileId",
                        column: x => x.FileId,
                        principalTable: "Files",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Documents_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OpenCdeDocumentSelections",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    DocumentId = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpenCdeDocumentSelections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OpenCdeDocumentSelections_Documents_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OpenCdeDocumentSelections_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PendingOpenCdeUploadFiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UploadSessionId = table.Column<Guid>(type: "TEXT", nullable: false),
                    FileName = table.Column<string>(type: "TEXT", nullable: false),
                    SessionFileId = table.Column<string>(type: "TEXT", nullable: false),
                    LinkedCdeDocumentId = table.Column<Guid>(type: "TEXT", nullable: true),
                    IsCancelled = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PendingOpenCdeUploadFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PendingOpenCdeUploadFiles_Documents_LinkedCdeDocumentId",
                        column: x => x.LinkedCdeDocumentId,
                        principalTable: "Documents",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PendingOpenCdeUploadFiles_OpenCdeDocumentUploadSessions_UploadSessionId",
                        column: x => x.UploadSessionId,
                        principalTable: "OpenCdeDocumentUploadSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Documents_FileId",
                table: "Documents",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_Name",
                table: "Documents",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_ProjectId",
                table: "Documents",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_FileMimeTypes_MimeType",
                table: "FileMimeTypes",
                column: "MimeType",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Files_ContainerName",
                table: "Files",
                column: "ContainerName");

            migrationBuilder.CreateIndex(
                name: "IX_Files_CreatedAtUtc",
                table: "Files",
                column: "CreatedAtUtc");

            migrationBuilder.CreateIndex(
                name: "IX_Files_CreatedByUserId",
                table: "Files",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Files_FileName",
                table: "Files",
                column: "FileName");

            migrationBuilder.CreateIndex(
                name: "IX_Files_MimeTypeId",
                table: "Files",
                column: "MimeTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_OpenCdeDocumentDownloadSessions_UserId",
                table: "OpenCdeDocumentDownloadSessions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_OpenCdeDocumentSelections_DocumentId",
                table: "OpenCdeDocumentSelections",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_OpenCdeDocumentSelections_UserId",
                table: "OpenCdeDocumentSelections",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_OpenCdeDocumentUploadSessions_SelectedProjectId",
                table: "OpenCdeDocumentUploadSessions",
                column: "SelectedProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_OpenCdeDocumentUploadSessions_UserId",
                table: "OpenCdeDocumentUploadSessions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PendingOpenCdeUploadFiles_LinkedCdeDocumentId",
                table: "PendingOpenCdeUploadFiles",
                column: "LinkedCdeDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_PendingOpenCdeUploadFiles_UploadSessionId",
                table: "PendingOpenCdeUploadFiles",
                column: "UploadSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_Name",
                table: "Projects",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OpenCdeDocumentDownloadSessions");

            migrationBuilder.DropTable(
                name: "OpenCdeDocumentSelections");

            migrationBuilder.DropTable(
                name: "PendingOpenCdeUploadFiles");

            migrationBuilder.DropTable(
                name: "Documents");

            migrationBuilder.DropTable(
                name: "OpenCdeDocumentUploadSessions");

            migrationBuilder.DropTable(
                name: "Files");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "FileMimeTypes");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
