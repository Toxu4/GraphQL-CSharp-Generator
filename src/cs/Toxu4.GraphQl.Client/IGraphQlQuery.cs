using System.Collections.Generic;

namespace Toxu4.GraphQl.Client
{
    public interface IGraphQlQuery
    {
        string QueryText { get; }
        IDictionary<string, object> Variables { get; }
    }
}
