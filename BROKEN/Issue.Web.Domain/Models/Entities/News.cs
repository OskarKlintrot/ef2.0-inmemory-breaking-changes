using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Intranet.Web.Domain.Models.Entities
{
    public class News
    {
        // Backing fields: https://docs.microsoft.com/en-us/ef/core/modeling/backing-field
        private bool _hasEverBeenPublished;
        private bool _published;
        private DateTimeOffset _created;

        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        /// <summary>
        /// Setting this property will set <see cref="Updated"/> as well if <see cref="Updated"/> is not yet set.
        /// </summary>
        public DateTimeOffset Created
        {
            get => _created;
            set
            {
                if (Updated < value)
                {
                    Updated = value;
                }

                _created = value;
            }
        }

        public DateTimeOffset Updated { get; set; }

        [Required]
        public string Text { get; set; }

        [Required]
        public string UserId { get; set; }

        public User User { get; set; }

        public Image HeaderImage { get; set; }

        /// <summary>
        /// Can only be changed once, from false to true, via <see cref="Published"/>.
        /// </summary>
        public bool HasEverBeenPublished
        {
            get => _hasEverBeenPublished;
        }

        /// <summary>
        /// Indicates wether or not the News is published or not. The first time <see cref="HasEverBeenPublished"/> will also be set to true.
        /// </summary>
        public bool Published
        {
            get => _published;
            set
            {
                if (_hasEverBeenPublished == false && value == true)
                {
                    _published = true;
                    _hasEverBeenPublished = true;
                }
                else
                {
                    _published = value;
                }
            }
        }

        public ICollection<NewsTag> NewsTags { get; set; }

        [Required]
        public string Url { get; set; }
    }
}
