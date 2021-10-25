using System;
using System.Collections.Generic;
using System.Text;

namespace Calculi.Literal.Errors
{
    enum ErrorCode
    {
        EMPTY_EXPRESSION,
        MISMATCHING_PARENTHESIS,
        MISSING_TERM,
        MISSING_FACTOR,
        MISSING_EXPONENT,
        MISSING_NUMERATOR,
        MISSING_DENOMINATOR,
        DIVISION_BY_ZERO,
        INVALID_POINT,
        COULD_NOT_INSERT,
        REAL_NUMBER_MIXED_WITH_CONSTANT,
        MULTIPLE_CONSTANTS,
        UNKNOWN_ERROR
    }
}
