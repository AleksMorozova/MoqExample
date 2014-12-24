using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication1
{
    public interface ILogWriter
    {
        string GetLogger();
        void SetLogger(string logger);
        void Write(string message);
    }

}
