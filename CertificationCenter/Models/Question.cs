namespace CertificationCenter.Models {
    public class Question {
        public string Id { get; set; }
        public string TopicId { get; set; }
        public string QuestionString { get; set; }
        public string AnswerString { get; set; }
        public Topic Topic { get; set; }
    }
}