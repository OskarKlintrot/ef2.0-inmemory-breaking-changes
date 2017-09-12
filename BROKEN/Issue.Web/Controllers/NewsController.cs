using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Intranet.Web.Domain.Data;
using Intranet.Web.Domain.Models.Entities;
using Intranet.Web.ViewModels;
using Intranet.Web.Common.Factories;
using Microsoft.AspNetCore.Http;
using X.PagedList;

namespace Intranet.Web.Controllers
{
    public class NewsController : Controller
    {
        private readonly IntranetApiContext _context;
        private readonly IDateTimeFactory _dateTimeFactory;

        public NewsController(IntranetApiContext context,
                              IDateTimeFactory dateTimeFactory)
        {
            _context = context;
            _dateTimeFactory = dateTimeFactory;
        }

        #region GET
        // GET: News
        public async Task<IActionResult> Index([FromQuery(Name = "page")]int pageNumber = 1)
        {
            try
            {
                var news = await _context.News
                    .Include(n => n.HeaderImage)
                    .Include(n => n.User)
                    .Include(n => n.NewsTags)
                        .ThenInclude(nt => nt.Tag)
                    .ToListAsync();

                var newsViewModels = news
                    .Select(n => new NewsViewModel(n))
                    .OrderBy(m => m.Published)
                    .ThenByDescending(m => m.Created)
                    .ToList();

                var onePageOfNews = newsViewModels.ToPagedList(pageNumber, pageSize: 5);

                return View(onePageOfNews);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // GET: News/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var news = await _context.News
                    .Include(n => n.HeaderImage)
                    .Include(n => n.User)
                    .Include(n => n.NewsTags)
                        .ThenInclude(nt => nt.Tag)
                    .SingleOrDefaultAsync(n => n.Id == id);

                if (news == null)
                {
                    return NotFound();
                }

                var newsViewModel = new NewsViewModel(news);

                return View(newsViewModel);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // GET: News/2017/05/20/title-of-the-news
        [Route("[controller]/{year:int}/{month:int}/{day:int}/{url}")]
        [HttpGet]
        public async Task<IActionResult> Details(int year, int month, int day, string url)
        {
            try
            {
                var date = new DateTimeOffset(year, month, day, 0, 0, 0, TimeSpan.Zero);

                var news = await _context.News
                    .Include(n => n.HeaderImage)
                    .Include(n => n.User)
                    .Include(n => n.NewsTags)
                        .ThenInclude(nt => nt.Tag)
                    .SingleOrDefaultAsync(n => n.Created.Date == date.Date && n.Url == url);

                if (news == null)
                {
                    return NotFound();
                }

                var newsViewModel = new NewsViewModel(news);

                return View(newsViewModel);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        #endregion

        #region Private Helpers
        private bool NewsExists(int id)
        {
            return _context.News.Any(e => e.Id == id);
        }

        private List<Tag> GetAllTagEntitiesInternal(NewsViewModel news, IEnumerable<string> tags)
        {
            if (tags == null)
            {
                return new List<Tag>();
            }

            return _context.Tags?
            .Include(k => k.NewsTags)
                .ThenInclude(nt => nt.News)?
            .Where(k => tags.Contains(k.Name, StringComparer.OrdinalIgnoreCase) || k.NewsTags.Any(nt => nt.NewsId.Equals(news.Id)))
            .ToList();
        }
        #endregion
    }
}
