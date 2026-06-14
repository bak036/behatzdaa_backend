using System;
using System.Collections.Generic;
using System.Text;

namespace Nofshonit.Common.Exceptions
{

    public class BusinessException : Exception
    {
        public string BusinessMessage { get; set; }
        public int BusinessMessageId { get; set; }


        public BusinessException()
        { }

        public BusinessException(string message) : base(message)
        {
            BusinessMessage = message;
        }

        public BusinessException(string message, int messageId) : base(message)
        {
            BusinessMessage = message;
            BusinessMessageId = messageId;
        }

        public BusinessException(string message, Exception inner = null) : base(message, inner)
        {
            BusinessMessage = message;
        }

    }
}
