using System;
using System.IO;
using System.Reflection;

namespace GHC.Operations
{

    #region "helper classes for enum string attributes"
    // enum string attribute
    internal class EnumStringAttribute : Attribute
    {
        private string stringValue;
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }
        public EnumStringAttribute(string stringValue)
        {
            this.stringValue = stringValue;
        }
    }

    // extension class for getting enum string attribute
    internal static class ExtensionClass
    {
        public static string GetStringValue(this Enum value)
        {
            Type type = value.GetType();
            FieldInfo fieldInfo = type.GetField(value.ToString());

            // get the stringvalue attributes
            EnumStringAttribute[] attribs = fieldInfo.GetCustomAttributes(
                typeof(EnumStringAttribute), false) as EnumStringAttribute[];

            // return the first if there was a match
            return attribs.Length > 0 ? attribs[0].StringValue : null;
        }
    }
    #endregion

    // enum
    public enum eLogLevel
    {
        [EnumStringAttribute("ERR")]
        Error = 1,
        [EnumStringAttribute("WRN")]
        Warning = 2,
        [EnumStringAttribute("INF")]
        Informational = 3,
        [EnumStringAttribute("DBG")]
        Debug = 4        
    }

    /// <summary>
    /// this class is a simple logger for moveIT Central/GHC Operations jobs.  it is a quick and dirty
    /// component to get something usable, but does have some issues. the calling application MUST call 
    /// the Close method in order fo the log to flush.  ideally, the stream would be setup in a using 
    /// statement, probably in WriteLine for each call (ie, WriteLine would instantiate the stream,
    /// passing in the log name/location, open the stream, write the line, and close the stream.
    /// </summary>
    public class CentralLogger
    {
        #region "private members"

        private StreamWriter logWriter;
        private string fullLogPath;
        private string logPath;
        private string appName;

        #endregion

        #region "public properties"

        /// <summary>
        /// The location for the log.  This is set in the constructor, but can be overridden 
        /// through this property.  Changing this property changes the location of the log as
        /// soon as it is set.
        /// </summary>
        public string LogPath 
        {
            get { return logPath; }
            set 
            {
                logPath = value;

                SetFullLogPath();
            }
        }

        /// <summary>
        /// The name of the application.  This is set in the constructor, but can be overriden
        /// through this property.  Changing this property changes the location of the log
        /// as soon as it is set.
        /// </summary>
        public string AppName 
        {
            get { return appName; }
            set
            {
                appName = value;

                SetFullLogPath();
            }
        }

        /// <summary>
        /// The full log path, including the constructed log file name.  This is based on
        /// the LogPath, AppName, and a datetime string.  This property is read-only.
        /// </summary>
        public string FullLogPath 
        {
            get { return fullLogPath; } 
        }

        /// <summary>
        /// The default logging level for the log.  Only messages that are the same as the default
        /// logging level or higher priority will display in the log.  For example, Informational
        /// will displays messages logged at the Informational level, as well as Warnings and Errors.
        /// </summary>
        public eLogLevel LogLevel { get; set; }

        #endregion

        #region "constructors"

        /// <summary>
        /// Minimum constructor which requires the log path and application name.  These are used
        /// to build the file path for the log.
        /// </summary>
        /// <param name="logPath">The location where the log file should be created.</param>
        /// <param name="appName">The name of the application, used in the log file name.</param>
        public CentralLogger(string inLogPath, string inAppName)
        {
            logPath = inLogPath;
            appName = inAppName;

            // set default log level to informational. this will log all messages flagged as 
            // informational, warning, or error.  if debug messages are necessary the LogLevel
            // must be set to Debug.
            LogLevel = eLogLevel.Informational;

            Initialize();
        }

        /// <summary>
        /// Overloaded constructor that requires the log path and application name.  These are used
        /// to build the file path for the log.  The log level is also required, which sets the 
        /// default message visibility.
        /// </summary>
        /// <param name="inLogPath">The location where the log file should be created.</param>
        /// <param name="inAppName">The name of the application, used in the log file name.</param>
        /// <param name="logLevel">Error, Warning, Informational, Debug</param>
        public CentralLogger(string inLogPath, string inAppName, eLogLevel logLevel)
        {
            logPath = inLogPath;
            appName = inAppName;
            LogLevel = logLevel;

            Initialize();
        }

        #endregion

        #region "public methods"

        /// <summary>
        /// write the message to the log with a default log level of warning.  if the default
        /// log level was overridden to errors only then this message will not get written
        /// to the log, and one of the WriteLine overloads should be used.
        /// </summary>
        /// <param name="message"></param>
        public void WriteLine(string message)
        {
            WriteLine(message, eLogLevel.Informational, false);
        }

        /// <summary>
        /// write the message to the log with the passed in log level.  the message will
        /// only be logged if that level is less than or equal to the log level instantiated
        /// for the log itself.  for example, if the log was instantiated with a default
        /// level of 2 (warning), only messages with a log level of warning or error will
        /// appear in the log.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="msgLevel"></param>
        public void WriteLine(string message, eLogLevel msgLevel)
        {
            WriteLine(message, msgLevel, false);
        }        

        /// <summary>
        /// accepts writeToConsole, which will write the message to standard output regardless
        /// of the logging level.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="msgLevel"></param>
        /// <param name="writeToConsole"></param>
        public void WriteLine(string message, eLogLevel msgLevel, bool writeToConsole)
        {
            if (msgLevel <= LogLevel)
            {                   
                WriteToFile(DateTime.Now.ToString() + " [" + msgLevel.GetStringValue() + "] " + message);
            }

            // by default, if an error message output to standard error as well as log file
            if (msgLevel == eLogLevel.Error)
            {
                WriteToStandardError(message);
            }

            if (writeToConsole)
            {
                WriteToStandardOutput(message);
            }
        }

        #endregion

        #region "private methods"

        /// <summary>
        /// common initialization method for all constructors.  checks if required parameters
        /// are passed in, sets the log path, and writes the log header.
        /// </summary>
        private void Initialize()
        {
            // check inputs
            if (logPath == null)
            {
                throw new ArgumentNullException("logPath", "LogPath is required!");
            }

            if (appName == null)
            {
                throw new ArgumentNullException("appName", "AppName is required!");
            }

            SetFullLogPath();

            // Write Header
            WriteToFile("Application: " + appName);
            WriteToFile("");
        }

        /// <summary>
        /// sets the full log path based on the LogPath and AppName, through buildLogName.
        /// </summary>
        private void SetFullLogPath()
        {
            fullLogPath = logPath + buildLogName();
        }

        /// <summary>
        /// write the actual message to the file.  all the WriteLine methods are used to build the string for 
        /// line item entries, and restrict messages based on log level.  this method will output those lines, 
        /// and can be used for other types of formatted lines, like the header/footer.
        /// </summary>
        /// <param name="message"></param>
        private void WriteToFile(string message)
        {
            using (var writer = new StreamWriter(fullLogPath, true))
            {
                writer.WriteLine(message);
            }
        }
        
        /// <summary>
        /// writes to the standard error output.  this can be redirected on the console using "2>".
        /// </summary>
        /// <param name="message"></param>
        private void WriteToStandardError(string message)
        {
            TextWriter errorWriter = Console.Error;            

            errorWriter.WriteLine("*** ERR *** :: " + message);
        }

        /// <summary>
        /// writes to the standard output.  this can be redirected on the console using ">".
        /// </summary>
        /// <param name="message"></param>
        private void WriteToStandardOutput(string message)
        {
            Console.WriteLine(message);
        }

        /// <summary>
        /// build the log name using the application name and a date/time string for uniqueness.
        /// </summary>
        /// <returns></returns>
        private string buildLogName()
        {
            string logName = "";

            logName = appName + "_" + DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".log";

            return logName;
        }

        #endregion

    }
}
