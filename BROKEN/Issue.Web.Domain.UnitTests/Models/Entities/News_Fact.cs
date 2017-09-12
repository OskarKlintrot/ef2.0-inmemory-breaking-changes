using Intranet.Web.Domain.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Intranet.Web.Domain.UnitTests.Models.Entities
{
    public class News_Fact
    {
        [Fact]
        public void HasEverBeenPublished_Is_False()
        {
            // Assign
            var news = new News();

            // Act

            // Assert
            Assert.False(news.HasEverBeenPublished);
            Assert.False(news.Published);
        }

        [Fact]
        public void Change_Publish_Changes_HasEverBeenPublished()
        {
            // Assign
            var news = new News();

            // Act
            news.Published = true;

            // Assert
            Assert.True(news.HasEverBeenPublished);
            Assert.True(news.Published);
        }

        [Fact]
        public void Can_Only_Change_HasEverBeenPublished_Once()
        {
            // Assign
            var news = new News();

            // Act
            news.Published = true;
            news.Published = false;

            // Assert
            Assert.True(news.HasEverBeenPublished);
            Assert.False(news.Published);
        }

        [Fact]
        public void HasEverBeenPublished_Wont_Toggle()
        {
            // Assign
            var news = new News();

            // Act
            news.Published = true;
            news.Published = false;
            news.Published = true;

            // Assert
            Assert.True(news.HasEverBeenPublished);
            Assert.True(news.Published);
        }

        [Fact]
        public void Set_Created_Sets_Updated()
        {
            // Assign
            var news = new News();
            var dateTimeOffset = new DateTimeOffset(2017, 7, 18, 0, 0, 0, TimeSpan.Zero);

            // Act
            news.Created = dateTimeOffset;

            // Assert
            Assert.Equal(news.Created, dateTimeOffset);
            Assert.Equal(news.Updated, dateTimeOffset);
        }

        [Fact]
        public void Updated_Can_Be_Set_Individualy()
        {
            // Assign
            var news = new News();
            var dateTimeOffset = new DateTimeOffset(2017, 7, 18, 0, 0, 0, TimeSpan.Zero);

            // Act
            news.Updated = dateTimeOffset;

            // Assert
            Assert.NotEqual(news.Created, dateTimeOffset);
            Assert.Equal(news.Updated, dateTimeOffset);
        }

        [Fact]
        public void Created_And_Update_Can_Differ()
        {
            // Assign
            var news = new News();
            var dateTimeOffsetCreated = new DateTimeOffset(2017, 7, 18, 0, 0, 0, TimeSpan.Zero);
            var dateTimeOffsetUpdated = new DateTimeOffset(2017, 7, 19, 0, 0, 0, TimeSpan.Zero);

            // Act
            news.Created = dateTimeOffsetCreated;
            news.Updated = dateTimeOffsetUpdated;

            // Assert
            Assert.NotEqual(dateTimeOffsetCreated, dateTimeOffsetUpdated);
            Assert.NotEqual(news.Created, news.Updated);
            Assert.Equal(news.Created, dateTimeOffsetCreated);
            Assert.Equal(news.Updated, dateTimeOffsetUpdated);
        }
    }
}
