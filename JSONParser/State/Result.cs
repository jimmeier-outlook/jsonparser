using System.Collections.Generic;

namespace JsonParser.State
{
    public class Result
    {
        public List<Response> responses { get; set; }
        public List<Note> notes { get; set; }
    }
}