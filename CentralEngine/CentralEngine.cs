using System.IO;
using Ghc.Utility.Security;

namespace GHC.Operations
{
    /// <summary>
    /// The CentralEngine is designed to provide common properties and functions to 
    /// applications executed through moveIT Central.
    /// </summary>
    public class CentralEngine
    {
        private System.Reflection.Assembly assemblyInfo;

        /// <summary>
        /// The path to the current executable.  This can be used for logging or as a base
        /// location for determining relative resources and paths.
        /// </summary>
        public string AppPath { get; set; }

        /// <summary>
        /// The name of the current application.
        /// </summary>
        public string AppName { get; set; }

        /// <summary>
        /// The path to the files directory for the application.  By default, the file directory
        /// contains all the input files and resources required by the application, as well as
        /// output folders.
        /// </summary>
        public string InputPath { get; set; }

        /// <summary>
        /// The path to the processing folder, for objects used during the execution of the application
        /// (ie, temporary files, etc), logging, and final output.
        /// </summary>
        public string OutputPath { get; set; }        

        public CentralEngine()
        {            
            assemblyInfo = System.Reflection.Assembly.GetCallingAssembly();

            // assign the default values.  these can be overriden by the calling application.
            AppPath = System.IO.Path.GetDirectoryName(assemblyInfo.Location);
            AppName = assemblyInfo.GetName().Name;            
            
            // check if input path exists, if it doesn't then try to create it
            if(!Directory.Exists(this.AppPath + "\\Files\\"))
            {
                Directory.CreateDirectory(this.AppPath + "\\Files\\");
            }            
            InputPath = this.AppPath + "\\Files\\";
            OutputPath = InputPath;
        }

        /// <summary>
        /// This Constructor is meant to be used for SSIS packages.
        /// As the path for input/output files had to be built dynamically
        /// </summary>
        /// <remarks></remarks>
        public CentralEngine(string strUserName, string strAppName, string machineName)
        {
            string strUserID;
            int index;

            //Strip User Name
            index = strUserName.LastIndexOf(@"\");
            if (index > 0)
            {
                index = index + 1;
            }
            strUserID = strUserName.Substring(index);

            AppName = strAppName;

            if (GHCActiveDirectory.IsMemberOf(machineName, GHCActiveDirectory.PrincipalType.Machine, "Dev_App_Environment"))
            {
                AppPath = "\\\\asnas\\SourceCode\\" + strUserID + "\\Integration\\Operations\\SSIS\\" + AppName;
            }
            else
            {
                AppPath = "\\\\asnas\\Central\\Bin\\" + AppName;
            }

            // check if input path exists, if it doesn't then try to create it
            if (!Directory.Exists(this.AppPath + "\\Files\\"))
            {
                Directory.CreateDirectory(this.AppPath + "\\Files\\");
            }
            InputPath = this.AppPath + "\\Files\\";
            OutputPath = InputPath;
            
        }

    }
}
