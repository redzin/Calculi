using System;
using System.Collections.Generic;
using System.Text;

namespace Calculi.Literal.Errors
{
    class UserMessageException : Exception
    {
        public UserMessageException(Error error) : base(error.ToString())
        {

        }
    }
}
