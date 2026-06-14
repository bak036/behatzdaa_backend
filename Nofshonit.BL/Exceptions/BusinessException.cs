using Nofshonit.Logs;
using System;

namespace Nofshonit.BL.Exceptions
{

    public class BusinessException : Exception
    {
        public string BusinessMessage { get; set; }
        public int BusinessMessageId { get; set; }
        private readonly BaseBL baseBL = new BaseBL();

        public BusinessException()
        { }

        public BusinessException(string message) : base(message)
        {
            this.LogMemberIdIfExist(ref message);
            BusinessMessage = message;
        }

        public BusinessException(string message, int messageId) : base(message)
        {
            this.LogMemberIdIfExist(ref message);
            BusinessMessage = message;
            BusinessMessageId = messageId;
        }

        public BusinessException(string message, Exception inner = null) : base(message, inner)
        {
            this.LogMemberIdIfExist(ref message);
            BusinessMessage = message;
        }

        private void LogMemberIdIfExist(ref string message)
        {
            if (!string.IsNullOrEmpty(baseBL.ContextManager.GetAccessToken()))
                LoggerHelper.Error($"{message}, memberId: {baseBL.ContextManager.CurrentUser().Id}");
        }
    }
}
