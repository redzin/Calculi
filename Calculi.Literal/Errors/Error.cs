using System;
using System.Collections.Generic;
using System.Text;

namespace Calculi.Literal.Errors
{
    class Error : Exception
    {
        public readonly ErrorCode Code;

        public Error(ErrorCode code)
        {
            Code = code;
        }
    }
}
