namespace Mamba.ViewModels.BlogVm
{
    public class BlogListItemVm
    {
        public int Id { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? UpdatedTime { get; set; }
        public bool IsDeleted { get; set; }
        public string? ImageUrl { get; set; }
        public string Name { get; set; }
        public string Profession { get; set; }
    }
}
