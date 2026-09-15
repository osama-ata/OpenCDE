using Dangl.OpenCDE.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Dangl.OpenCDE.Data
{
    public class CdeDbContext : DbContext
    {
        public CdeDbContext(DbContextOptions<CdeDbContext> options) : base(options)
        {
        }

        public DbSet<CdeUser> Users { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<CdeAppFile> Files { get; set; }
        public DbSet<CdeAppFileMimeType> FileMimeTypes { get; set; }
        public DbSet<OpenCdeDocumentSelection> OpenCdeDocumentSelections { get; set; }
        public DbSet<OpenCdeDocumentUploadSession> OpenCdeDocumentUploadSessions { get; set; }
        public DbSet<OpenCdeDocumentDownloadSession> OpenCdeDocumentDownloadSessions { get; set; }
        public DbSet<PendingOpenCdeUploadFile> PendingOpenCdeUploadFiles { get; set; }
        public DbSet<Project> Projects { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            Document.OnModelCreating(builder);
            CdeAppFile.OnModelCreating(builder);
            CdeAppFileMimeType.OnModelCreating(builder);
            Project.OnModelCreating(builder);

            // Guid primary keys fall back to EF Core's client-side GuidValueGenerator
            // by convention now that there's no database-side default (SQL Server's
            // newsequentialid() has no SQLite equivalent).

            base.OnModelCreating(builder);
        }
    }
}
