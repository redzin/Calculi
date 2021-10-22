using System;
using System.Collections.Generic;
using System.Text;

namespace Calculi.Literal.Errors
{
    class UserMessageException : Exception
    {
        public ErrorCode Error { get; private set; }

        public UserMessageException(ErrorCode error)
        {
            Error = error;
        }
    }
}
