using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CmdLine;

namespace GHC.Operations
{    
    /// <summary>
    /// This is the default configuration for Central jobs.  Minimially they will allow for overriding the output path and input path.
    /// Any other parameters required for a job will be overriden in the base class.
    /// </summary>
    abstract public class CentralCommandLine
    {
        [CommandLineParameter(Command = "?", Default = false, Description = "Show Help", Name = "Help", IsHelp = true)]
        public bool Help { get; set; }

        [CommandLineParameter(Command = "o", Description = "Path for output files", Name = "output")]
        public string Output { get; set; }

        [CommandLineParameter(Command = "i", Description = "Path for input files", Name = "input")]
        public string Input { get; set; }

        [CommandLineParameter(Command = "l", Description = "Log Level", Name = "loglevel")]
        public int LogLevel { get; set; }
    }
}
