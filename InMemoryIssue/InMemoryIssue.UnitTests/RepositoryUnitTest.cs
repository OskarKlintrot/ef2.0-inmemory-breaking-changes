using InMemoryIssue.ConsoleApp;
using InMemoryIssue.Domain.Data;
using InMemoryIssue.Domain.Models.Entities;
using InMemoryIssue.UnitTests.Fakes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace InMemoryIssue.UnitTests
{
    public class UnitTest1
    {
        [Fact]
        public async Task GetAllPosts()
        {
            // Assign
            IEnumerable<BlogPost> blogPostsInDb = null;
            var blogPosts = new List<BlogPost>
            {
                new BlogPost
                {
                    Title = "First Blog Post",
                    Text = "The Body",
                    UserId = "oskar.klintrot",
                    User = new User { Username = "oskar.klintrot", DisplayName = "Oskar Klintrot" },
                },
            };

            DbContextFake.SeedDb<BloggingContext>(c => c.BlogPosts.AddRange(blogPosts));

            // Act
            using (var context = DbContextFake.GetDbContext<BloggingContext>())
            {
                var repository = new BlogPostRepository(context);
                blogPostsInDb = await repository.GetBlogPostsAsync();
            }

            // Assert
            Assert.Equal(1, blogPostsInDb.Count());
        }
    }
}
