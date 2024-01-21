namespace Mamba.Models
{
    public class Setting:BaseEntity
    {
        public string Title { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }
}
