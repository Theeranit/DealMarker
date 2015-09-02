using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace KK.DealMaker.Core.SystemFramework
{
    /// <summary>
    /// Summary description for return message form UIP to UI
    /// </summary>
    /// <example>
    /// <res>
    ///    <res_msgtype>Information</res_msgtype>
    ///    <res_code>101</res_code>
    ///    <res_msg>Success</res_msg>
    ///    <res_value></resvalue>
    /// </res>
    /// </example>
    [XmlRoot("result")]
    public class ResultData
    {
        private string _res_msg = "Unhandled Exception: ";
        private string _res_id = string.Empty;
        private ResultCode _res_code = ResultCode.None;
        private ResultType _res_msgtype = ResultType.None;
        private string _res_value = "undefined";
        private string _res_hidden_value = "undefined";

        /// <summary>
        /// Initializes a new instance of the <see cref="result"/> class.
        /// </summary>
        public ResultData()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="result"/> class.
        /// </summary>
        /// <param name="result">The result.</param>
        public ResultData(string result)
        {
            this.res_msgtype = ResultType.Successful;
            this.res_code = ResultCode.Success;
            _res_msg = result;

        }
        /// <summary>
        /// Initializes a new instance of the <see cref="result"/> class.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <param name="id">The id.</param>
        public ResultData(string result, string id)
        {
            this.res_msgtype = ResultType.Successful;
            this.res_code = ResultCode.Success;
            _res_msg = result;
            _res_id = id;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="result"/> class.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <param name="id">The id.</param>
        /// <param name="value">The value.</param>
        public ResultData(string result, string id, string value)
        {
            this.res_msgtype = ResultType.Successful;
            this.res_code = ResultCode.Success;
            _res_msg = result;
            _res_id = id;
            _res_value = value;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="result"/> class.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <param name="id">The id.</param>
        /// <param name="value">The value.</param>
        /// <param name="hvalue">The hvalue.</param>
        public ResultData(string result, string id, string value, string hvalue)
        {
            this.res_msgtype = ResultType.Successful;
            this.res_code = ResultCode.Success;
            _res_msg = result;
            _res_id = id;
            _res_value = value;
            _res_hidden_value = hvalue;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="result"/> class.
        /// </summary>
        /// <param name="ex">The ex.</param>
        public ResultData(Exception ex)
        {
            if (ex.GetType().BaseType != typeof(ExceptionWrapper))
                _res_msg += ex.Message + ex.StackTrace;
            else
            {
                _res_msg = "มีข้อผิดพลาด ดังนี้ ";
                _res_msg += ex.Message;
            }

            this.res_msgtype = ResultType.Information;
            this.res_code = ResultCode.Error;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="result"/> class.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <param name="message">The message.</param>
        public ResultData(Exception ex, string message)
        {
            if (ex.GetType().BaseType != typeof(ExceptionWrapper))
                _res_msg += message + ex.StackTrace;
            else
            {
                _res_msg = "มีข้อผิดพลาด ดังนี้ ";
                _res_msg += message;
            }

            this.res_msgtype = ResultType.Information;
            this.res_code = ResultCode.Error;
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<result>");
            sb.Append("<res_id>");
            sb.Append(res_id.ToString());
            sb.Append("</res_id>");
            sb.Append("<res_msgtype>");
            sb.Append(res_msgtype.ToString());
            sb.Append("</res_msgtype>");
            sb.Append("<res_code>");
            sb.Append(((int)res_code).ToString());
            sb.Append("</res_code>");
            sb.Append("<res_msg>");
            sb.Append(res_msg);
            sb.Append("</res_msg> ");
            if (res_value != null)
            {
                sb.Append("<res_value>");
                sb.Append(res_value);
                sb.Append("</res_value> ");
            }
            if (res_hidden_value != null)
            {
                sb.Append("<res_hidden_value>");
                sb.Append(res_hidden_value);
                sb.Append("</res_hidden_value> ");
            }
            sb.Append("</result>");
            return sb.ToString();


        }

        /// <summary>
        /// Type of Message will be returned
        /// </summary>
        /// <value>The res_msgtype.</value>
        [XmlElement("res_msgtype")]
        public ResultType res_msgtype
        {
            get
            {
                return _res_msgtype;
            }
            set
            {
                _res_msgtype = value;
            }
        }

        /// <summary>
        /// Message code please see ResCode Enum for desc.
        /// </summary>
        /// <value>The res_code.</value>
        [XmlElement("res_code")]
        public ResultCode res_code
        {
            get
            {
                return _res_code;
            }
            set
            {
                _res_code = value;
            }
        }
        /// <summary>
        /// Result message
        /// </summary>
        /// <value>The res_msg.</value>
        [XmlElement("res_msg", IsNullable = true)]
        public string res_msg
        {
            get
            {
                return _res_msg;
            }
            set
            {
                _res_msg = value;
            }
        }

        /// <summary>
        /// Gets or sets the res_value.
        /// </summary>
        /// <value>The res_value.</value>
        [XmlElement("res_value")]
        public string res_value
        {
            get { return _res_value; }
            set { _res_value = value; }
        }
        /// <summary>
        /// Gets or sets the res_id.
        /// </summary>
        /// <value>The res_id.</value>
        [XmlElement("res_id")]
        public string res_id
        {
            get { return _res_id; }
            set { _res_id = value; }
        }
        /// <summary>
        /// Gets or sets the res_hidden_value.
        /// </summary>
        /// <value>The res_hidden_value.</value>
        [XmlElement("res_hidden_value")]
        public string res_hidden_value
        {
            get { return _res_hidden_value; }
            set { _res_hidden_value = value; }
        }

    }
    /// <summary>
    /// Gets the result code
    /// </summary>
    public enum ResultCode
    {
        Success = 101,
        Error = 501,
        None = 0
    }
    /// <summary>
    /// Gets the result type
    /// </summary>
    public enum ResultType
    {
        Successful,
        Information,
        Warning,
        None
    }
}
