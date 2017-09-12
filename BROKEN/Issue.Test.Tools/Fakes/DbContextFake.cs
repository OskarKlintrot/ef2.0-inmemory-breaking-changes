using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Intranet.Test.Tools.Fakes
{
    public static class DbContextFake
    {
        /// <summary>
        /// Creates a new DbContext that is using an In Memory-database
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="databaseName">If not set this will be the name of the calling method</param>
        /// <returns></returns>
        public static TContext GetDbContext<TContext>([CallerMemberName] string databaseName = "")
            where TContext : DbContext
        {
            var options = new DbContextOptionsBuilder<TContext>()
                    .UseInMemoryDatabase(databaseName)
                    .Options;

            return (TContext)Activator.CreateInstance(typeof(TContext), options);
        }

        /// <summary>
        /// Seeds the database
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="action"></param>
        /// <param name="ensureDeleted">If true the database will be deleted if it exists</param>
        /// <param name="databaseName">If not set this will be the name of the calling method</param>
        public static void SeedDb<TContext>(Action<TContext> action, bool ensureDeleted = true, [CallerMemberName] string databaseName = "")
            where TContext : DbContext
        {
            using (var context = GetDbContext<TContext>(databaseName))
            {
                if (ensureDeleted) context.Database.EnsureDeleted();
                action(context);
                context.SaveChanges();
            }
        }
    }
}
