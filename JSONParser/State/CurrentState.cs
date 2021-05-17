namespace JsonParser.State
{
    public class CurrentState
    {
        public DisplayOptions displayOptions { get; set; }
        public string id { get; set; }
        public string bookmark { get; set; }
        public Result[] result { get; set; }
        public object humanScores { get; set; }
        public decimal? totalScore { get; set; }
        public string status { get; set; }
        public int linkIndex { get; set; }
        public int linkLabel { get; set; }
        public decimal? manualScore { get; set; }
        public decimal? totalMachineScore { get; set; }
        public object totalHumanScore { get; set; }
        public float score { get; set; }
        public object comment { get; set; }
        public string itemStatus { get; set; }
        public string interpretation { get; set; }
        public bool? hasAlert { get; set; }
        public ExternalScores[] externalScores { get; set; }
        public bool? attempted { get; set; }
    }
}