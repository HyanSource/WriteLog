using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Hyan.IO
{
    class LogIO
    {
        private static LogIO Main;

        private static object locker = new object();

        private LogIO()
        {

        }

        public static LogIO GetInstance()
        {
            if (Main == null)
            {
                lock (locker)
                {
                    if (Main == null)
                    {
                        Main = new LogIO();
                    }
                }
            }

            return Main;
        }

        private string LogPath = @"Log\";

        private ConcurrentQueue<string> LogCQ = new ConcurrentQueue<string>();

        public void WriteInLog(string msg)
        {
            LogCQ.Enqueue(msg);
        }

        public void Start()
        {
            if (!Directory.Exists(LogPath))
            {
                Console.WriteLine("建立資料夾");
                Directory.CreateDirectory(LogPath);
            }
            else
            {
                Console.WriteLine("已經有資料夾");
            }

            Task.Run(() => LogTask());
        }

        public async Task LogTask()
        {
            string m = "";

            while (true)
            {
                try
                {
                    if (!LogCQ.IsEmpty)
                    {
                        StringBuilder all = new StringBuilder();
                        while (LogCQ.TryDequeue(out m))
                        {
                            all.AppendLine(m);
                        }
                        string time = LogPath + DateTime.Now.ToString("yyyy-MM-dd HH");

                        using (StreamWriter sw = new StreamWriter(Path.Combine(time + " Log.txt"),true))
                        {
                            sw.Write(all);
                        }
                    }
                    else
                    {
                        await Task.Delay(1000);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
