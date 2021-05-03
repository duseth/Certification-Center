namespace CertificationCenter.Models {
    public class Question {
        public string Id { get; set; }
        public string CertificationId { get; set; }
        public string QuestionString { get; set; }
        public Certification Certification { get; set; }
    }
}