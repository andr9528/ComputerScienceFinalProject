using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers
{
    public static class Logger
    {
        public static void LogMessage(string message, TextWriter writer)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(Environment.NewLine + "Log Entry : ");
            builder.Append(Environment.NewLine + string.Format("{0} {1}", DateTime.Now.ToLongTimeString(), DateTime.Now.ToLongDateString()));
            builder.Append(Environment.NewLine + "  :");
            builder.Append(Environment.NewLine + string.Format("  :{0}", message));
            builder.Append(Environment.NewLine + "------------------------------------------------------------");

            writer.Write(builder.ToString());
        }

        public static void LogException(Exception exception, TextWriter writer)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(Environment.NewLine + "Log Entry : ");
            builder.Append(Environment.NewLine + string.Format("{0} {1}", DateTime.Now.ToLongTimeString(), DateTime.Now.ToLongDateString()));
            builder.Append(Environment.NewLine + "  :");

            builder.Append(Environment.NewLine + string.Format("  :{0}", exception.Message));
            builder.Append(Environment.NewLine + string.Format("  :{0}", exception.StackTrace));

            if (exception.InnerException != null)
            {
                builder = LogException(exception.InnerException, builder);
            }

            builder.Append(Environment.NewLine + "------------------------------------------------------------");

            writer.Write(builder.ToString());
        }

        private static StringBuilder LogException(Exception exception, StringBuilder builder)
        {
            builder.Append(Environment.NewLine + "Caused by -->");
            builder.Append(Environment.NewLine + string.Format("  :{0}", exception.Message));
            builder.Append(Environment.NewLine + string.Format("  :{0}", exception.StackTrace));

            if (exception.InnerException != null)
            {
                builder = LogException(exception.InnerException, builder);
            }

            return builder;
        }
    }
}
