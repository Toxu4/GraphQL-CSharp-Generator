using GraphQL.Types;

namespace GraphQlServer.Schema.Types
{
    public class FolderOrFileType : UnionGraphType
    {
        public FolderOrFileType()
        {
            Type<FolderType>();
            Type<FileType>();
        }
    }
}