using System.IO;

namespace JsonParser
{
    public class ProjectConfig
    {
        public string ProjectName { get; set; }
        public string ProjectNumber { get; set; }
        public int Admin { get; set; }
        public string Environment { get; set; }
        public string ConnectionString { get; set; }
        public string LogPath { get; set; }
        public string LogBase { get; set; }
        public string ErrBase { get; set; }
        public int IncludeBrailleAndPaperPencil { get; set; }
        public string FormId { get; set; }
        public string SubjectCode { get; set; }
        public int? BatchSize { get; set; }
        public string Mailto { get; set; }

        private static readonly log4net.ILog l4log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    }
}