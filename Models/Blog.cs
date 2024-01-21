using System.Diagnostics;

namespace Mamba.Models
{
    public class Blog:BaseEntity
    {
        public string? ImageUrl { get; set; }
        public string Name { get; set; }
        public string Profession { get; set; }

    }
}
