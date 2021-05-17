using System.Collections.Generic;

namespace JsonParser.State
{
    public class TestState
    {
        public string testName { get; set; }
        public List<CurrentState> currentState { get; set; }
        public long timestamp { get; set; }
        public string formId { get; set; }

    }
}