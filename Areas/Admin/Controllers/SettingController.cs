using Mamba.Contexts;
using Mamba.ViewModels.BlogVm;
using Mamba.ViewModels.SettingVm;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mamba.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SettingController : Controller
    {
        DataDbContext _db { get; set; }
        public SettingController(DataDbContext db, IWebHostEnvironment env)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _db.Settings.Select(c=> new SettingListItemVm
            {
                Id = c.Id,
                Title = c.Title,
                Phone = c.Phone,
                Address = c.Address,
                Email = c.Email,
                UpdatedTime = c.UpdatedTime,
            }).ToListAsync());
        }
        public async Task<IActionResult> Cancel()
        {
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return BadRequest();
            var data = await _db.Settings.FindAsync(id);
            if (data == null) return NotFound();
            return View(new SettingUpdate
            {
                Title = data.Title,
                Email = data.Email,
                Address = data.Address,
                Phone = data.Phone,
            });
        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id, SettingUpdate vm)
        {
            if (id == null || id < 0) return BadRequest();
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var data = await _db.Settings.FindAsync(id);
            if (data == null) return NotFound();
            data.Title = vm.Title;
            data.Address = vm.Address;
            data.Phone = vm.Phone;
            data.Email = vm.Email;
            
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
