using System.IO;
using GraphQlServer.Schema.Types;
using GraphQL.Types;

namespace GraphQlServer.Schema
{
    public class ComputerQuery : ObjectGraphType
    {
        public ComputerQuery()
        {
            Field<DriveQuery>("drives", resolve:context => new {});
            Field<FolderType>()
                .Name("folder")
                .Argument<StringGraphType, string>("name", "название")
                .Resolve(context => new DirectoryInfo(context.GetArgument<string>("name")));
        }
    }
}