using GraphQL;

namespace GraphQlServer.Schema
{
    public class ComputerSchema :GraphQL.Types.Schema
    {
        public ComputerSchema(ComputerQuery query, IDependencyResolver resolver)
        {
            Query = query;
            DependencyResolver = resolver;
        }
    }
}