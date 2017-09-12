using Intranet.Web.Controllers;
using Intranet.Web.Domain.Data;
using Intranet.Web.Domain.Models.Entities;
using Intranet.Test.Tools.Fakes;
using Intranet.Test.Tools.Extensions;
using Intranet.Web.ViewModels;
using Intranet.Web.Common.Factories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using Xunit;
using System.Threading.Tasks;
using X.PagedList;

namespace Intranet.Web.UnitTests.Controllers
{
    public class NewsController_Fact
    {
        #region GET
        [Fact]
        public async Task Get_News_Id_Should_Return_News()
        {
            // Assign
            var id = 1;
            var news = GetFakeNews();
            var dateTimeFactory = new DateTimeFactory();

            DbContextFake.SeedDb<IntranetApiContext>(c => c.News.AddRange(news));

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var newsController = new NewsController(context, dateTimeFactory);

                // Act
                var response = await newsController.Details(id);
                var newsContent = response.GetModelAs<NewsViewModel>();

                // Assert
                Assert.IsType<ViewResult>(response);
                Assert.Equal(id, newsContent.Id);
            }
        }

        [Fact]
        public async Task ReturnOkObjectResultWhenSearchById()
        {
            // Assign
            int id = 1;
            var news = GetFakeNews();
            var dateTimeFactory = new DateTimeFactory();

            DbContextFake.SeedDb<IntranetApiContext>(c => c.News.AddRange(news));

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var newsController = new NewsController(context, dateTimeFactory);

                // Act
                var response = await newsController.Details(id);
                var result = response.GetModelAs<NewsViewModel>();

                // Assert
                Assert.IsType<ViewResult>(response);
                Assert.Equal(1, result.Id);
            }
        }

        [Fact]
        public async Task ReturnNotFoundResultWhenSearchById()
        {
            // Assign
            int id = 2;
            var news = GetFakeNews();
            var dateTimeFactory = new DateTimeFactory();

            DbContextFake.SeedDb<IntranetApiContext>(c => c.News.AddRange(news));

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var newsController = new NewsController(context, dateTimeFactory);

                // Act
                var result = await newsController.Details(id);

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }

        [Fact]
        public async Task ReturnNewsByUrl()
        {
            // Assign
            var url = "news-title-1";
            var news = GetFakeNews();
            var dateTimeFactory = new DateTimeFactory();

            var newsToFind = news.SingleOrDefault(n => n.Url == url);

            DbContextFake.SeedDb<IntranetApiContext>(c => c.News.AddRange(news));

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var newsController = new NewsController(context, dateTimeFactory);

                // Act
                var response = await newsController.Details(newsToFind.Created.Year, newsToFind.Created.Month, newsToFind.Created.Day, url);
                var newsContent = response.GetModelAs<NewsViewModel>();

                // Assert
                Assert.Equal(url, newsContent.Url);
                Assert.IsType<ViewResult>(response);
            }
        }

        [Fact]
        public async Task ReturnNotFoundResultWhenSearchByUrl()
        {
            // Assign
            var url = "news-title-2";
            var news = GetFakeNews();
            var dateTimeFactory = new DateTimeFactory();

            DbContextFake.SeedDb<IntranetApiContext>(c => c.News.AddRange(news));

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var newsController = new NewsController(context, dateTimeFactory);

                // Act
                var result = await newsController.Details(2017, 7, 21, url);

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }

        [Fact]
        public async Task ReturnNotFoundResultWhenSearchByWrongDateUrl()
        {
            // Assign
            var dateTimeOffsetCreated = new DateTimeOffset(2017, 7, 18, 0, 0, 0, TimeSpan.Zero);
            var url = "news-title-1";
            var news = GetFakeNews(dateTimeOffsetCreated);
            var dateTimeFactory = new DateTimeFactory();

            DbContextFake.SeedDb<IntranetApiContext>(c => c.News.AddRange(news));

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var newsController = new NewsController(context, dateTimeFactory);

                // Act
                var result = await newsController.Details(2017, 7, 21, url);

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }

        [Theory]
        [InlineData(1, "2017-04-03 02:00:00", "News title 1", "This is a content placeholder for news title 1.", "anne.the.admin", "news-title-1")]
        public async Task Get_All_News_Should_Return_All_News(int id, string newsDate, string title, string text, string username, string url)
        {
            // Assign
            var utcDate = new DateTimeOffset(Convert.ToDateTime(newsDate));
            var news = GetFakeNews(id, utcDate, title, text, username, url);
            var dateTimeFactory = new DateTimeFactory();

            DbContextFake.SeedDb<IntranetApiContext>(c => c.News.AddRange(news));

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var newsController = new NewsController(context, dateTimeFactory);

                // Act
                var response = await newsController.Index();
                var models = response.GetModelAs<IEnumerable<NewsViewModel>>();
                var newsFromController = models.First();

                // Assert
                Assert.Equal(newsFromController.Id, id);
                Assert.Equal(newsFromController.Created, utcDate);
                Assert.Equal(newsFromController.Title, title);
                Assert.Equal(newsFromController.Text, text);
                Assert.Equal(newsFromController.UserId, username);
            }
        }

        [Fact]
        public async Task Get_All_News_Should_Return_Paginated_News()
        {
            // Assign
            var news = GetFakeNews();
            var dateTimeFactory = new DateTimeFactory();

            DbContextFake.SeedDb<IntranetApiContext>(c => c.News.AddRange(news));

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var newsController = new NewsController(context, dateTimeFactory);
                var newsController2 = new NewsController(context, dateTimeFactory);

                // Act
                var firstPageResponse = await newsController.Index(1);
                var secondPageResponse = await newsController2.Index(2);
                var firstPageModels = firstPageResponse.GetModelAs<IPagedList<NewsViewModel>>();
                var secondPageModels = secondPageResponse.GetModelAs<IPagedList<NewsViewModel>>();


                // Assert
                Assert.Equal(1, firstPageModels.Count);
                Assert.Equal(0, secondPageModels.Count);
            }
        }

        [Fact]
        public async Task Get_All_News_Should_Return_Empty_List()
        {
            // Assign
            var dateTimeFactory = new DateTimeFactory();

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var newsController = new NewsController(context, dateTimeFactory);

                // Act
                var response = await newsController.Index();
                var result = response.GetModelAs<IEnumerable<News>>();

                // Assert
                Assert.IsType<ViewResult>(response);
                Assert.Equal(result.Count(), 0);
            }
        }

        [Fact]
        public async Task ReturnOkObjectResultWhenGetAllNews()
        {
            // Assign
            var news = GetFakeNews();
            var dateTimeFactory = new DateTimeFactory();

            DbContextFake.SeedDb<IntranetApiContext>(c => c.News.AddRange(news));

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var newsController = new NewsController(context, dateTimeFactory);

                // Act
                var newsFromController = await newsController.Index();
                var count = newsFromController.GetModelAs<IEnumerable<NewsViewModel>>().Count();

                // Assert
                Assert.IsType<ViewResult>(newsFromController);
                Assert.Equal(1, count);
            }
        }
        #endregion

        #region Private Helpers
        private IEnumerable<News> GetFakeNews()
        {
            return GetFakeNews(newsDate: DateTimeOffset.Now);
        }

        private IEnumerable<News> GetFakeNews(DateTimeOffset newsDate)
        {
            return GetFakeNews
                (
                    id: 1,
                    date: newsDate,
                    title: "News title 1",
                    text: "This is a content placeholder for news title 1",
                    username: "anne.the.admin",
                    url: "news-title-1"
                );
        }

        /// <summary>
        /// Generate and return dummy news 
        /// </summary>
        /// <returns></returns>
        private IEnumerable<News> GetFakeNews(int id,
                                              DateTimeOffset date,
                                              string title,
                                              string text,
                                              string username,
                                              string url)
        {
            return new News[]
            {
                new News
                {
                    Id = id,
                    Created = date,
                    Title = title,
                    Text = text,
                    UserId = username,
                    Url = url,
                }
            };
        }
        #endregion
    }
}
