using System.ComponentModel.DataAnnotations;

namespace Auth.Controllers.Resources
{
    public class RevokeTokenResource
    {
        [Required]
        public string Token { get; set; }
    }
}
