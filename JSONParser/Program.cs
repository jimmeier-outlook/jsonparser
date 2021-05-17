
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
//using JSONParser;

namespace JsonParser
{
    public class Program
    {
        
        public static bool Verbose { get; private set; }
        public static string RunId { get; set; }

        public static string GetBasePath() => Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location);
        private static readonly log4net.ILog L4Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static void Main(string[] args)
        {
            _handler += new EventHandler(Handler);
            SetConsoleCtrlHandler(_handler, true);
           Verbose = true;
           RunId = args[1];

            var projectConfig = new ProjectConfig()
            {
                ProjectName = args[2], FormId = args[9], ProjectNumber = args[0], ConnectionString = args[4],
                Environment = args[3], LogPath = args[5], LogBase = args[6],
                IncludeBrailleAndPaperPencil = Convert.ToInt32(args[8]), Admin = Convert.ToInt32(args[10]),
                ErrBase = args[7], SubjectCode = args[11], BatchSize = Convert.ToInt32(args[12]), Mailto = args[13]
            };
            var withOrWithout = Verbose ? "with" : "without";
            //Logger.Configure(args[0],withOrWithout);

            

            //L4Log.Info($"Processing project ID {args[0]} {withOrWithout} verbose logging.");

            ProcessSelectedProjectConfigs(args[0], projectConfig);

            string totalErrors = ProjectConfigProcessor.ErrorCount.ToString();
            string totalSuccess = ProjectConfigProcessor.successCount.ToString();

            

            //L4Log.Info($"Total Errors {totalErrors}");
            //L4Log.Info($"Total Processed Successfully {totalSuccess}");
            //L4Log.Info($"Processing project ID {args[0]} is exiting in a completed state.");
        }

       

        private static void ProcessSelectedProjectConfigs(string projectid, ProjectConfig projectConfig)
        {
            var processor = new ProjectConfigProcessor(projectid, projectConfig ,RunId);
                processor.Process();
            
        }

        [DllImport("Kernel32")]
        private static extern bool SetConsoleCtrlHandler(EventHandler handler, bool add);

        private delegate bool EventHandler(CtrlType sig);
        static EventHandler _handler;

        enum CtrlType
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT = 1,
            CTRL_CLOSE_EVENT = 2,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT = 6
        }

        private static bool Handler(CtrlType sig)
        {
            switch (sig)
            {
                case CtrlType.CTRL_C_EVENT:
                case CtrlType.CTRL_LOGOFF_EVENT:
                case CtrlType.CTRL_SHUTDOWN_EVENT:
                case CtrlType.CTRL_CLOSE_EVENT:
                default:
                    return false;
            }
        }


    }
}