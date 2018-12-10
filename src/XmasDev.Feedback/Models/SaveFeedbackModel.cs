namespace XmasDev.Feedback.Models
{
    public class SaveFeedbackModel
    {
        public string UserCode { get; set; }
        public string ProductCode { get; set; }
        public int Rating { get; set; }

        public bool None { get; set; }
        public bool Success { get; set; }
    }
}