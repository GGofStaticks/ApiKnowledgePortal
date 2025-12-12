using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using ApiKnowledgePortal.Domain.ApiSpecifications;
using ApiKnowledgePortal.Domain.ParsedApiSpecs;
using ApiKnowledgePortal.Domain.SwaggerSources;
using ApiKnowledgePortal.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace ApiKnowledgePortal.Infrastructure.Persistence
{
    public class ApiKnowledgePortalDbContext : DbContext
    {
        public ApiKnowledgePortalDbContext(DbContextOptions<ApiKnowledgePortalDbContext> options)
            : base(options)
        {
        }

        // дбсет для сущности аписпек
        public DbSet<ApiSpecifications> ApiSpecs { get; set; }

        // дбсет для источников свагера
        public DbSet<SwaggerSource> SwaggerSources { get; set; } = default!;

        // дбсет для парсера свагера
        public DbSet<ParsedApiSpec> ParsedApiSpecs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApiSpecifications>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                      .HasConversion(
                          id => id.Value,
                          value => new ApiSpecId(value));

                entity.Property(e => e.SwaggerSourceId).IsRequired(false);

                entity.HasOne<SwaggerSource>()
                      .WithMany()
                      .HasForeignKey(e => e.SwaggerSourceId)
                      .OnDelete(DeleteBehavior.SetNull) 
                      .IsRequired(false);

                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Version).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Content).IsRequired().HasColumnType("jsonb"); ;
                entity.Property(e => e.CreatedAt).IsRequired();

                //entity.HasIndex(e => new { e.Name, e.Version }).IsUnique(); // для уникальности Name+Version
            });

            modelBuilder.Entity<SwaggerSource>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.Url).IsRequired();
                entity.Property(e => e.IsActive).IsRequired();
                entity.Property(e => e.LastFetchStatus).HasMaxLength(1000);
                entity.Property(e => e.LastFetchTime);
            });

            modelBuilder.Entity<ParsedApiSpec>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Path).IsRequired();
                entity.Property(e => e.Method).IsRequired();
                entity.Property(e => e.OperationId).IsRequired();
                entity.Property(e => e.Summary).IsRequired();
                entity.Property(e => e.CreatedAt).IsRequired();
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}