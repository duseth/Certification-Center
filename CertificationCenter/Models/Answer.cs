namespace CertificationCenter.Models {
    public class Answer {
        public string Id { get; set; }
        public string QuestionId { get; set; }
        public string ResultId { get; set; }
        public string AnswerString { get; set; }
        public bool IsCorrect { get; set; }
        public Question Question { get; set; }

        public Result Result { get; set; }
    }
}