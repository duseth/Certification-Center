using Microsoft.AspNetCore.Identity;

namespace CertificationCenter.Models {
    public class User : IdentityUser {
        public override string Id { get; set; }
        public override string UserName { get; set; }
        public override string Email { get; set; }
    }
}