using Microsoft.Extensions.FileProviders;
using Microsoft.VisualBasic;

namespace MaterialGatePassTracker.Middleware
{
    
        public class LogWriterClass
        {
        private readonly RequestDelegate _next;

        public LogWriterClass(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            await _next(context);
        }
            private static string m_exePath = string.Empty;
            public static void LogWrite(string logMessage, string path)
            {
                var filepath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory())).Root + $@"";

                m_exePath = filepath + path;
                string fullpath = m_exePath + "\\" + "log_" + DateTime.Now.ToString("dd-MMM-yyyy") + ".txt";

                if (!File.Exists(fullpath))
                {
                    File.Create(fullpath);
                }

                try
                {

                    FileStream fs = new FileStream(fullpath, FileMode.Append);

                    using (StreamWriter w = new StreamWriter(fs))
                        AppendLog(logMessage, w);



                }
                catch (Exception ex)
                {
                    //AppendLog(ex.ToString());
                }

            }

            private static void AppendLog(string logMessage, TextWriter txtWriter)
            {
                try
                {                
                    txtWriter.WriteLine("{0},{1}", DateTime.Now.ToLongDateString() + " ", DateTime.Now.ToLongTimeString() + " :" + logMessage);
                }
                catch (Exception ex)
                {
                    txtWriter.Write(ex.Message);
                }
            }

        }

    
}
