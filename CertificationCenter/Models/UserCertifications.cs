namespace CertificationCenter.Models {
    public class UserCertifications {
        public string UserId { get; set; }
        public string CertificationId { get; set; }
        public User User { get; set; }
        public Certification Certification { get; set; }
    }
}