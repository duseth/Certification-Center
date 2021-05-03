namespace CertificationCenter.Models {
    public class Answer {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string CertificationId { get; set; }
        public string QuestionId { get; set; }
        public string AnswerString { get; set; }
        public bool IsCorrect { get; set; }
        public User User { get; set; }
        public Certification Certification { get; set; }
        public Question Question { get; set; }
    }
}