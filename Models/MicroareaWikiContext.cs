using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace HICMigration.Models
{
    public partial class MicroareaWikiContext : DbContext
    {
        public ConnectInfo connectInfo = new ConnectInfo();
        public MicroareaWikiContext(
            IConfiguration configuration
        )
        {
            connectInfo = configuration.GetSection("ConnectInfo").Get<ConnectInfo>();
        }

        public MicroareaWikiContext(
            DbContextOptions<MicroareaWikiContext> options,
            IConfiguration configuration
        )
        : base(options)
        {
            connectInfo = configuration.GetSection("ConnectInfo").Get<ConnectInfo>();
        }

        public virtual DbSet<PageContent> PageContent { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer($"Server={connectInfo.Server};Database={connectInfo.Database};User Id={connectInfo.User};Password={connectInfo.Password};");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PageContent>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.Comment)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasColumnType("ntext");

                entity.Property(e => e.DateTime).HasColumnType("datetime");

                entity.Property(e => e.OriginalContent).HasColumnType("ntext");

                entity.Property(e => e.OriginalName).HasMaxLength(128);

                entity.Property(e => e.Page)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(64);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
