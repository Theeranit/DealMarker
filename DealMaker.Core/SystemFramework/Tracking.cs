using System;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.Remoting.Messaging;
using System.Text;

namespace KK.DealMaker.Core.SystemFramework
{
    #region Enumerations

    /// <summary>
    /// Message Direction
    /// </summary>
    public enum Message
    {
        /// <summary>Message Request: before processed by Messenger Client</summary>
        Request,
        /// <summary>Message Response: interface message after it has been transformed by the Messenger Client</summary>
        Response
    }

    #endregion

    /// <summary>
    /// Utility class that provides trace functionality leveraging the ACA.NET Logging framework
    /// </summary>
    public sealed class Tracing
    {
        #region Constants

        // Default log settings
        private const int LOG_EVENT_ID = 100;
        private const string ERROR_CONFIGURATION_MISSING_SWITCH = "Unable to open tracing switch. Check application configuration file to see if switch exists";
        // Default log context items
        private const string LOG_CONTEXT_USER_ID = "user_id";
        private const string LOG_CONTEXT_FOREIGN_ID = "foreign_id";

        #endregion

        #region Enumerations

        /// <summary>
        /// Trace Category
        /// </summary>
        public enum Category
        {
            /// <summary>Trace</summary>
            Trace,
            /// <summary>Database</summary>
            Database,
            /// <summary>Performance</summary>
            Performance
        }

        #endregion

        #region Inner Classes, Structures and Delegates

        #endregion

        #region Events

        #endregion

        #region Class and Shared Variables

        #endregion

        #region Constructors

        /// <summary>
        /// Default Constructor
        /// </summary>
        private Tracing()
        {
        }

        #endregion

        #region Properties

        #endregion

        #region Methods

        /// <summary>
        /// Write a trace message (leveraging the ACA.Logging Framework)
        /// </summary>
        /// <param name="logCategory">ACA logging category</param>
        /// <param name="message">Message body of the trace</param>
        /// <param name="severityLevel">Trace Level</param>
        /// <param name="foreignId">Deal/App ID</param>
        /// <param name="userId">User ID</param>
        /// <remarks>
        /// This switch controls general messages. In order to 
        /// receive general trace messages change the value to the 
        /// appropriate level. "1" gives error messages, "2" gives errors 
        /// and warnings, "3" gives more detailed error information, and 
        /// "4" gives verbose trace information 
        /// </remarks>
        public static void WriteLine(Category logCategory, string message, TraceLevel severityLevel, Guid foreignId, Guid userId)
        {
            message = @"<![CDATA[" + message + @"]]>";
            string header = string.Empty;

            // Default level is info
            EventLogEntryType level = EventLogEntryType.Information;

            // If trace is disabled just pass through
            if (severityLevel != TraceLevel.Off)
            {
                // Determine whether the chosen trace level meets adequate levels again the defined trace switch 
                if (isLoggable(logCategory, TracingLevel(severityLevel)))
                {
                    switch (severityLevel)
                    {
                        case TraceLevel.Error:
                            level = EventLogEntryType.Error;
                            break;
                        case TraceLevel.Warning:
                            level = EventLogEntryType.Warning;
                            break;
                        case TraceLevel.Info:
                            level = EventLogEntryType.Information;
                            break;
                        case TraceLevel.Verbose:
                            break;
                    }

                    // Write to the log using ACA.NET. Destination Output is determined by the ACA Log settings
                    try
                    {
                        StackFrame sf = new StackFrame(1);
                        header = sf.GetMethod().Name.ToString();

                        CallContext.SetData(LOG_CONTEXT_FOREIGN_ID, null);
                        CallContext.SetData(LOG_CONTEXT_USER_ID, null);

                        // If the deal ID has a value, set it the logging context item appropriately
                        if (!foreignId.Equals(Guid.Empty))
                        {
                            CallContext.SetData(LOG_CONTEXT_FOREIGN_ID, foreignId);
                        }

                        // If the user ID has a value, set it the logging context item appropriately
                        if (!userId.Equals(Guid.Empty))
                        {
                            CallContext.SetData(LOG_CONTEXT_USER_ID, userId);
                        }
                        // Write to the log using ACA.NET. Destination Output is determined by the ACA Log settings
                        //Logger.WriteToLog(logCategory.ToString(), header, message, LOG_EVENT_ID, level);
                    }
                    catch (Exception e)
                    {
                        // If it failed to write to the category, attempt to write to the default category
                        string errorMessage = string.Format(CultureInfo.CurrentCulture, "Failed to log message to category {0} --> {1}: {2}", logCategory, message, e.ToString());
                        // Write to the log using ACA.NET. Destination Output is determined by the ACA Log settings
                        //Logger.WriteToLog(Category.Trace.ToString(), header, errorMessage, LOG_EVENT_ID, EventLogEntryType.Error);
                    }
                }
            }
        }

        /// <summary>
        /// Matches trace to corresponding trace number
        /// </summary>
        /// <param name="severityLevel">Trace level</param>
        /// <returns>Trace level number</returns>
        public static int TracingLevel(TraceLevel severityLevel)
        {
            switch (severityLevel)
            {
                case TraceLevel.Off:
                    return 0;
                case TraceLevel.Error:
                    return 1;
                case TraceLevel.Warning:
                    return 2;
                case TraceLevel.Info:
                    return 3;
                case TraceLevel.Verbose:
                    return 4;
                default:
                    return -1;
                //throw new ConfigurationException("Tracing level not supported");
            }
        }

        /// <summary>
        /// Compare the trace levels to determine whether the trace statement will be logged.
        /// </summary>
        /// <param name="logCategory">Switch name (which is the same as the ACA.NET Logging category name)</param>
        /// <param name="tracingLevel">Level of the trace as defined by the trace statement in code</param>
        /// <returns>Whether the statement can be logged</returns>
        public static bool isLoggable(Category logCategory, int tracingLevel)
        {
            bool traceStatus = false;
            int switchLevel = 0;

            // Create an instance of the trace switch
            TraceSwitch mySwitch = new TraceSwitch(logCategory.ToString(), "");

            // If valid, grab the level
            if (mySwitch != null)
                switchLevel = TracingLevel(mySwitch.Level);
            //else
            //    throw new ConfigurationException(ERROR_CONFIGURATION_MISSING_SWITCH);

            // Compare the levels if valid
            if (switchLevel > 0)
            {
                if (switchLevel >= tracingLevel)
                    traceStatus = true; // Minimal tracing level met
            }
            // Tracing is off, disable logging
            else
                traceStatus = true;

            return traceStatus;
        }

        /// <summary>
        /// Constructors a unique trace header
        /// </summary>
        /// <param name="messageId">Guid Id</param>
        /// <param name="message">Message Type</param>
        /// <returns>Formatted </returns>
        public static string TracingHeader(Guid messageId, Message message)
        {
            StringBuilder tracingText = new StringBuilder();
            tracingText.Append(DateTime.Now.ToShortDateString().Replace("/", "-"));
            tracingText.Append(" ");
            tracingText.Append(DateTime.Now.ToLongTimeString().Replace(":", ""));
            tracingText.Append(" ");
            tracingText.Append(messageId.ToString());
            tracingText.Append(" ");
            if (message == Message.Request)
                tracingText.Append("Request");
            else if (message == Message.Response)
                tracingText.Append("Response");

            return tracingText.ToString();
        }

        #endregion
    }
}
