using Mamba.Contexts;
using Mamba.Models;

namespace Mamba.Helpers
{
    public class LayoutService
    {
        DataDbContext _db {  get; set; }

        public LayoutService(DataDbContext db)
        {
            _db = db;
        }
        public async Task<Setting> GetSeedData()
            => await _db.Settings.FindAsync(1);
    }
}
