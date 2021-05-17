using System.Collections.Generic;

namespace JsonParser.State
{
    public class Response
    {
        public string intractionId { get; set; }
        public string interactionType { get; set; }
        public List<ResponseChoice> responseChoices { get; set; }
        public decimal? score { get; set; }
        public string status { get; set; }
        public bool isCorrect { get; set; }
        public string interactionStatus { get; set; }
        public string interpretation { get; set; }
        public bool? attempted { get; set; }
    }
}