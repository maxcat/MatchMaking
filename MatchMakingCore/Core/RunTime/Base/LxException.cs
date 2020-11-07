using System;

namespace MatchMakingCore
{
    public struct LxExceptionData
    {
        public int ErrorCode;
        public string Message;
        public string StackTrace;

        public override string ToString()
        {
            return $"Error code: {ErrorCode} Message: {Message} StackTrace: {StackTrace}";
        }
    }

    public class LxException : Exception
    {
        public int ErrorCode;
        public LxExceptionData LxData => new LxExceptionData
        {
            StackTrace = StackTrace,
            ErrorCode = ErrorCode,
            Message = Message
        };

        public LxException(int errorCode) : base()
        {
            ErrorCode = errorCode;
        }

        public LxException(string message) : base(message)
        {
        }
    }
}
