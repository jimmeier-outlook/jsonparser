using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace JsonParser
{
    public class Log
    {
        public string LogFilename { get; }

        public Log(string filename)
        {
            LogFilename = filename;
        }

        public async Task WriteLogLine(string message)
        {
            var semaphore = new SemaphoreSlim(1);
            await semaphore.WaitAsync();
            try
            {
                using (var stream = new FileStream(LogFilename, FileMode.OpenOrCreate))
                using (var writer = new StreamWriter(stream))
                {
                    writer.WriteLine($"{DateTime.Now:yyyy-MM-dd_HHmmss}   : {message}");
                }
            }
            catch
            {
                Console.WriteLine("File writing error");
            }
            finally
            {
                semaphore.Release();
            }
        }
    }
}