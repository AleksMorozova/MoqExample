using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication1
{
    class Logger
    {
        private readonly ILogWriter _logWriter;

        public Logger(ILogWriter logWriter)
        {
            _logWriter = logWriter;
        }

        public void WriteLine(string message)
        {
            _logWriter.Write(message);
        }

    }
}
