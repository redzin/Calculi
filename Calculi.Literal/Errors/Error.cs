using System;
using System.Collections.Generic;
using System.Text;

namespace Calculi.Literal.Errors
{
    enum Error
    {
        EMPTY_EXPRESSION,
        MISMATCHING_PARENTHESIS,
        MISSING_TERM,
        MISSING_FACTOR,
        MISSING_NUMERATOR,
        MISSING_DENOMINATOR,
        DIVISION_BY_ZERO,
        INVALID_POINT,
        COULD_NOT_INSERT
    }
}
