using InMemoryIssue.Domain.Data;
using InMemoryIssue.Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InMemoryIssue.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync(args).GetAwaiter().GetResult();
        }

        static async Task MainAsync(string[] args)
        {
            Console.WriteLine("Hello World!");

            var dbOptions = new DbContextOptionsBuilder<BloggingContext>()
                .UseInMemoryDatabase("databaseName")
                .Options;

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

            using (var db = new BloggingContext(dbOptions))
            {
                var repository = new BlogPostRepository(db);

                await repository.SetBlogPostsAsync(blogPosts);
                var count = await repository.SaveChangesAsync();
                Console.WriteLine("{0} records saved to database", count);
            }

            Console.WriteLine();

            using (var db = new BloggingContext(dbOptions))
            {
                var repository = new BlogPostRepository(db);

                Console.WriteLine("All blog posts in database:");
                foreach (var post in await repository.GetBlogPostsAsync())
                {
                    Console.WriteLine($"{post.Title} by {post.User.DisplayName}");
                }
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
