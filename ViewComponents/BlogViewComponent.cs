using Mamba.Contexts;
using Mamba.ViewModels.BlogVm;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mamba.ViewComponents
{
    public class BlogViewComponent : ViewComponent
    {
        DataDbContext _db {  get; set; }

        public BlogViewComponent(DataDbContext db)
        {
            _db = db;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await _db.Blogs.Select(c => new BlogListItemVm
            {
                Id = c.Id,
                CreatedTime = c.CreatedTime,
                UpdatedTime = c.UpdatedTime,
                ImageUrl = c.ImageUrl,
                IsDeleted = c.IsDeleted,
                Name = c.Name,
                Profession = c.Profession,
            }).ToListAsync());
        }
    }
}
