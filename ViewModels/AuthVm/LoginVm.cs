using System.ComponentModel.DataAnnotations;

namespace Mamba.ViewModels.AuthVm
{
    public class LoginVm
    {
        public string UsernameorEmail { get; set; }
        [Required,DataType(DataType.Password)]
        public string Password { get; set; }
        public bool IsRemember { get; set; }
    }
}
