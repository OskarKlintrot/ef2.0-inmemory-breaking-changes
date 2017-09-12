using Intranet.Web.Domain.Models.Entities;
using Intranet.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Intranet.Web.UnitTests.ViewModels
{
    public class NewsViewModel_Fact
    {
        [Fact]
        public void If_Null_Return_Null()
        {
            // Assign
            var newsViewModel = new NewsViewModel();

            newsViewModel.NewsTags = null;
            // Act

            // Assert
            Assert.Null(newsViewModel.Tags);
        }

        [Fact]
        public void Return_Tag()
        {
            // Assign
            var newsViewModel = new NewsViewModel();

            newsViewModel.NewsTags = new List<NewsTag>
            {
                new NewsTag
                {
                    Tag = new Tag
                    {
                        Name = "test",
                    }
                }
            };
            // Act

            // Assert
            Assert.Equal(newsViewModel.Tags, "test");
        }

        [Fact]
        public void Return_Tags()
        {
            // Assign
            var newsViewModel = new NewsViewModel();

            newsViewModel.NewsTags = new List<NewsTag>
            {
                new NewsTag
                {
                    Tag = new Tag
                    {
                        Name = "test",
                    }
                },
                new NewsTag
                {
                    Tag = new Tag
                    {
                        Name = "test2",
                    }
                },
            };
            // Act

            // Assert
            Assert.Equal(newsViewModel.Tags, "test,test2");
        }

        [Fact]
        public void Return_Tags_From_News()
        {
            // Assign
            var news = new News
            {
                NewsTags = new List<NewsTag>
                {
                    new NewsTag
                    {
                        Tag = new Tag
                        {
                            Name = "test",
                        }
                    },
                    new NewsTag
                    {
                        Tag = new Tag
                        {
                            Name = "test2",
                        }
                    },
                },
            };

            var newsViewModel = new NewsViewModel(news);
            // Act

            // Assert
            Assert.Equal(newsViewModel.Tags, "test,test2");
        }

        [Fact]
        public void Should_Be_Possible_To_Set_Tags()
        {
            // Assign
            var newsViewModel = new NewsViewModel();

            // Act
            newsViewModel.Tags = "test";

            // Assert
            Assert.Equal(newsViewModel.Tags, "test");
        }
    }
}
