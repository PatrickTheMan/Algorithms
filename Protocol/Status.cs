namespace Algorithms.Protocol {
    public enum StatusCode {
        OK = 200,
        Bad_Reqest = 400,
        I_Am_A_Teapot = 418,
        Internal_Server_Error = 500,
        Internal_Verification_Error = 700,
        Duplicate,
        Incomplete,
        Bad_EAN,
        Bad_Number,
    }

    public partial class Status {
        public StatusCode StatusCode { get; set; }
        public string StatusMessage { get; set; }

        public const string defaultMessage = "The server refuses the attempt to brew coffee with a teapot.";

        public Status(StatusCode StatusCode = StatusCode.I_Am_A_Teapot, string StatusMessage = defaultMessage) {

            this.StatusCode = StatusCode;
            this.StatusMessage = StatusMessage;

        }
    }
}
