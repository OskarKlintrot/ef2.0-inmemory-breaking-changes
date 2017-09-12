using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Intranet.Web.Domain.Models.Entities;
using System.Linq;

namespace Intranet.Web.Domain.Data
{
    /// <summary>
    /// Configuration of database
    /// </summary>
    public class IntranetApiContext : DbContext
    {
        public IntranetApiContext(DbContextOptions<IntranetApiContext> options)
            : base(options)
        {
            // Empty
        }

        /// <summary>
        /// Setup of entities
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Use the Class name as Table name
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                entity.Relational().TableName = entity.ClrType.Name;
            }

            #region News
            modelBuilder.Entity<News>()
                .Property(n => n.HasEverBeenPublished)
                .UsePropertyAccessMode(PropertyAccessMode.FieldDuringConstruction);

            modelBuilder.Entity<News>()
                .Property(n => n.Published)
                .UsePropertyAccessMode(PropertyAccessMode.Property);

            modelBuilder.Entity<News>()
                .HasIndex(n => n.Url);
            #endregion

            #region NewsTag
            modelBuilder.Entity<NewsTag>()
                .HasKey(k => new { k.NewsId, k.TagId });

            modelBuilder.Entity<NewsTag>()
                .HasOne(nt => nt.News)
                .WithMany(n => n.NewsTags)
                .HasForeignKey(nt => nt.NewsId);

            modelBuilder.Entity<NewsTag>()
                .HasOne(nt => nt.Tag)
                .WithMany(k => k.NewsTags)
                .HasForeignKey(nt => nt.TagId);
            #endregion

            #region Tag
            modelBuilder.Entity<Tag>()
                .HasIndex(k => k.Url)
                .IsUnique();

            modelBuilder.Entity<Tag>()
                .HasIndex(k => k.Name)
                .IsUnique();
            #endregion

            base.OnModelCreating(modelBuilder);
        }

        public virtual DbSet<News> News { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }
        public virtual DbSet<Image> Images { get; set; }
        public virtual DbSet<User> Users { get; set; }
    }
}
