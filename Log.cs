using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xcat
{

    public static class Log
    {

        public static void Error(string msg)
        {
            string fg = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "cat.debug");

            if (System.IO.File.Exists(fg))
            {
                Write("ERROR " + msg);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Error:" + msg);
            }
        }

        public static void Info(string msg)
        {
#if DEBUG
            string fg = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "cat.debug");

            if (System.IO.File.Exists(fg))
            {
                Write("INFO " + msg);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Info:" + msg);
            }
#endif         

        }

        public static void Write(string text)
        {
            string logfile = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DateTime.Now.ToString("yyyyMMdd") + ".log");
            using (StreamWriter sw = new StreamWriter(logfile, true, Encoding.UTF8))
            {
                sw.Write(DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss] ") + text + "\r\n");
            }
        }
    }

}
