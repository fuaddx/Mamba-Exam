using Mamba.Contexts;
using Mamba.Models;
using Mamba.ViewModels.BlogVm;
using Mamba.ViewModels.CommonVm;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mamba.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles =("Admin"))]
    public class BlogController : Controller
    {
        DataDbContext _db { get; set; }
        IWebHostEnvironment _env { get; set; }
        public BlogController(DataDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

       
       public async Task<IActionResult> Index()
        {
            int take = 4;
            var items = await _db.Blogs.Take(take).Select(c => new BlogListItemVm
            {
                Id = c.Id,
                CreatedTime = c.CreatedTime,
                UpdatedTime = c.UpdatedTime,
                ImageUrl = c.ImageUrl,
                IsDeleted = c.IsDeleted,
                Name = c.Name,
                Profession = c.Profession,
            }).ToListAsync();
            int total = await _db.Blogs.CountAsync();
            PaginationVm<IEnumerable<BlogListItemVm>> pag = new(total, 1, (int)Math.Ceiling((decimal)total / take), items);
            return View(pag);
        }
        public async Task<IActionResult>ProductPagination(int page=1,int count = 8)
        {
            var items = await _db.Blogs.Skip((page - 1) * count).Take(count).Select(c => new BlogListItemVm
            {
                Id = c.Id,
                CreatedTime = c.CreatedTime,
                UpdatedTime = c.UpdatedTime,
                ImageUrl = c.ImageUrl,
                IsDeleted = c.IsDeleted,
                Name = c.Name,
                Profession = c.Profession,
            }).ToListAsync();
            int totalCount = await _db.Blogs.CountAsync();
            PaginationVm<IEnumerable<BlogListItemVm>> pag = new(totalCount, page, (int)Math.Ceiling((decimal)totalCount / count), items);
            return PartialView("ProductPagination", pag);
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }
        public async Task<IActionResult> Cancel()
        {
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult>Create(BlogCreateVm vm)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            string filename = null;
            if (vm.MainIMage != null)
            {
                 filename = Guid.NewGuid() + Path.GetExtension(vm.MainIMage.FileName);
                using (Stream fs = new FileStream(Path.Combine(_env.WebRootPath, "Assets", "images", "stories", filename), FileMode.Create))
                {
                    await vm.MainIMage.CopyToAsync(fs);
                }

            }
            
            Blog blog = new Blog()
            {
                Profession = vm.Profession,
                Name = vm.Name,
                ImageUrl = filename
            };
            _db.Blogs.AddAsync(blog);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult>Update(int? id)
        {
            if (id == null) return BadRequest();
            var data = await _db.Blogs.FindAsync(id);
            if(data == null) return NotFound();
            return View(new BlogUpdateVm
            {
                ImageUrl = data.ImageUrl,
                Name = data.Name,
                Profession = data.Profession,
            });
        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id, BlogUpdateVm vm)
        {
            if (id == null|| id<0) return BadRequest();
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var data = await _db.Blogs.FindAsync(id);
            if (data == null) return NotFound();
            data.Profession = vm.Profession;
            data.Name = vm.Name;
            if (!string.IsNullOrEmpty(data.ImageUrl))
            {
                string filepath = Path.Combine(_env.WebRootPath, "Assets", "images", "stories", data.ImageUrl);
                if(System.IO.File.Exists(filepath))
                {
                    System.IO.File.Delete(filepath);
                }
            }
            string filename = Guid.NewGuid() + Path.GetExtension(vm.MainIMage.FileName);
            using (Stream fs = new FileStream(Path.Combine(_env.WebRootPath, "Assets", "images", "stories", filename), FileMode.Create))
            {
                await vm.MainIMage.CopyToAsync(fs);
            }
            data.ImageUrl = filename;
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult>DeleteProduct(int? id)
        {
            if (id == null) return BadRequest();
            var data = await _db.Blogs.FindAsync(id);
            if (data == null) return NotFound();
            data.IsDeleted = true;
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> RestoreProduct(int? id)
        {
            if (id == null) return BadRequest();
            var data = await _db.Blogs.FindAsync(id);
            if (data == null) return NotFound();
            data.IsDeleted = false;
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> DeleteFromData(int? id)
        {
            if (id == null) return BadRequest();
            var data = await _db.Blogs.FindAsync(id);
            if (data == null) return NotFound();
            _db.Blogs.Remove(data);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
