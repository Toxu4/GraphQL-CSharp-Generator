using System.Threading;
using System.Threading.Tasks;

namespace Toxu4.GraphQl.Client
{
    public interface IGraphQlQueryExecutor
    {
        Task<TResult> Run<TQuery, TResult>(TQuery query) where TQuery : IGraphQlQuery;
    }
}
