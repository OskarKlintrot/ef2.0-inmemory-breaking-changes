using InMemoryIssue.Domain.Data;
using InMemoryIssue.Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InMemoryIssue.ConsoleApp
{
    public class BlogPostRepository
    {
        private readonly BloggingContext _context;

        public BlogPostRepository(BloggingContext context)
        {
            _context = context;
        }

        public async Task SetBlogPostsAsync(IEnumerable<BlogPost> blogPosts)
        {
            await _context.AddRangeAsync(blogPosts);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<BlogPost>> GetBlogPostsAsync()
        {
            return await _context.BlogPosts
                .Include(bp => bp.User)
                .ToListAsync();
        }
    }
}
