using System;
using System.Collections.Generic;
using System.Text;

namespace Calculi.Shared
{
    internal interface IConverter<TSource, TDestination>
    {
        TDestination Convert(TSource source_object);
    }
}
