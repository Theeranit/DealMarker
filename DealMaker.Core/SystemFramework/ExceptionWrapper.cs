using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KK.DealMaker.Core.SystemFramework
{
    /// <summary>
    /// Exception Wrapper Inherit Exception
    /// </summary>
    public abstract class ExceptionWrapper : Exception
    {
        string _message = " ";
        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionWrapper"/> class.
        /// </summary>
        protected ExceptionWrapper()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionWrapper"/> class.
        /// </summary>
        /// <param name="ex">The ex.</param>
        protected ExceptionWrapper(Exception ex)
        {
            //_message += ex.Message + " " + ex.StackTrace;
        }

        /// <summary>
        /// Gets a message that describes the current exception.
        /// </summary>
        /// <value></value>
        /// <returns>The error message that explains the reason for the exception, or an empty string("").</returns>
        public override string Message
        {
            get
            {
                return _message;
            }
        }

        /// <summary>
        /// Gets the type of the exception.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <returns></returns>
        protected Exception GetExceptionType(Exception ex)
        {
            return ex.GetBaseException();
        }
    }

    public class DataServicesException : ExceptionWrapper
    {
        private int _sqlErrorNo = 0;
        private string _message;
        private Exception _ex;
        /// <summary>
        /// Initializes a new instance of the <see cref="DataServicesException"/> class.
        /// </summary>
        /// <param name="ex">The ex.</param>
        public DataServicesException(Exception ex)
            : base(ex)
        {
            _ex = ex;
            if (ex.GetType() == typeof(System.Data.SqlClient.SqlException))
            {
                System.Data.SqlClient.SqlException sqlEx = (System.Data.SqlClient.SqlException)ex;
                SqlErrorNumber = sqlEx.Number;
                _message = string.Format(KK.DealMaker.Core.Constraint.FormatTemplate.SQL_ERROR_NO, ((System.Data.SqlClient.SqlException)ex).Number) + _ex.Message + _ex.StackTrace;
            }
            else
                _message = _ex.Message + _ex.StackTrace;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataServicesException"/> class.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <param name="message">The message.</param>
        public DataServicesException(Exception ex, string message)
            : base(ex)
        {
            _ex = ex;
            if (ex.GetType() == typeof(System.Data.SqlClient.SqlException))
            {
                System.Data.SqlClient.SqlException sqlEx = (System.Data.SqlClient.SqlException)ex;
                SqlErrorNumber = sqlEx.Number;
                _message = string.Format(KK.DealMaker.Core.Constraint.FormatTemplate.SQL_ERROR_NO, ((System.Data.SqlClient.SqlException)ex).Number) + message;
            }
            else
                _message = message;
        }

        /// <summary>
        /// Gets or sets the SQL error number.
        /// </summary>
        /// <value>The SQL error number.</value>
        public int SqlErrorNumber
        {
            get
            {
                return _sqlErrorNo;
            }
            set
            {
                _sqlErrorNo = value;
            }
        }

        /// <summary>
        /// Gets a message that describes the current exception.
        /// </summary>
        /// <value></value>
        /// <returns>The error message that explains the reason for the exception, or an empty string("").</returns>
        public override string Message
        {
            get
            {
                return _message;
            }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="DataServicesException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public DataServicesException(string message)
        {

            _message = message;
        }
    }

    public class BusinessWorkflowsException : ExceptionWrapper
    {
        private string _message;
        private Exception _ex;
        /// <summary>
        /// Initializes a new instance of the <see cref="BusinessWorkflowsException"/> class.
        /// </summary>
        /// <param name="ex">The ex.</param>
        public BusinessWorkflowsException(Exception ex)
            : base(ex)
        {
            _ex = ex;
            _message = _ex.Message + _ex.StackTrace;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BusinessWorkflowsException"/> class.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <param name="message">The message.</param>
        public BusinessWorkflowsException(Exception ex, string message)
            : base(ex)
        {
            _ex = ex;
            _message = message;
        }

        /// <summary>
        /// Gets a message that describes the current exception.
        /// </summary>
        /// <value></value>
        /// <returns>The error message that explains the reason for the exception, or an empty string("").</returns>
        public override string Message
        {
            get
            {
                return _message;
            }
        }
    }

    public class UIPException : ExceptionWrapper
    {
        private string _message;
        private Exception _ex;

        /// <summary>
        /// Initializes a new instance of the <see cref="UIPException"/> class.
        /// </summary>
        /// <param name="ex">The ex.</param>
        public UIPException(Exception ex)
            : base(ex)
        {
            _ex = ex;
            _message = _ex.Message; // +_ex.StackTrace;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UIPException"/> class.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <param name="message">The message.</param>
        public UIPException(Exception ex, string message)
        {
            _ex = ex;
            _message += message + " " + _ex.Message + " " + _ex.StackTrace;
        }

        /// <summary>
        /// Gets a message that describes the current exception.
        /// </summary>
        /// <value></value>
        /// <returns>The error message that explains the reason for the exception, or an empty string("").</returns>
        public override string Message
        {
            get
            {
                return this.ToString();
            }
        }

        /// <summary>
        /// Converts errors to an xml string.
        /// </summary>
        /// <returns>
        /// A string representation of the current exception.
        /// </returns>
        /// <PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" PathDiscovery="*AllFiles*"/></PermissionSet>
        public override string ToString()
        {
            StringBuilder s = new StringBuilder();
            //s.Append("<error>");
            //s.Append("<type>Information</type>");
            //s.Append("<message>" + base.Message + _message + "</message>");
            //s.Append("</error>");
            s.Append(base.Message + _message);
            return s.ToString();
        }
    }

    public class DomainException : ExceptionWrapper
    {

    }

    public class UniqueKeyException : ExceptionWrapper
    {
        private string _defaultMsg = KK.DealMaker.Core.Constraint.Messages.DUPLICATE_KEY;
        private string _message;
        private Exception _ex;

        /// <summary>
        /// Initializes a new instance of the <see cref="UniqueKeyException"/> class.
        /// </summary>
        /// <param name="ex">The ex.</param>
        public UniqueKeyException(Exception ex)
            : base(ex)
        {
            _ex = ex;
            _message = _ex.Message + _ex.StackTrace;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UniqueKeyException"/> class.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <param name="message">The message.</param>
        public UniqueKeyException(Exception ex, string message)
            : base(ex)
        {
            _ex = ex;
            _message = message;
        }

        /// <summary>
        /// Gets a message that describes the current exception.
        /// </summary>
        /// <value></value>
        /// <returns>The error message that explains the reason for the exception, or an empty string("").</returns>
        public override string Message
        {
            get
            {
                return _defaultMsg;
            }
        }
    }
}
