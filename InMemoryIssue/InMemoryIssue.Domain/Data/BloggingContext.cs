using InMemoryIssue.Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace InMemoryIssue.Domain.Data
{
    public class BloggingContext : DbContext
    {
        public BloggingContext(DbContextOptions options)
            : base(options)
        {
            // Empty
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Use the Class name as Table name
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                entity.Relational().TableName = entity.ClrType.Name;
            }

            base.OnModelCreating(modelBuilder);
        }

        public virtual DbSet<BlogPost> BlogPosts { get; set; }
        public virtual DbSet<User> Users { get; set; }
    }
}
