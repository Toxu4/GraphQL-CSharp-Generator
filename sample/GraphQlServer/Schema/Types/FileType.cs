using System.IO;
using GraphQL.Types;

namespace GraphQlServer.Schema.Types
{
    public class FileType : ObjectGraphType<FileInfo>
    {
        public FileType()
        {
            Field(_ => _.Name);
            Field(_ => _.Length);
            Field(_ => _.IsReadOnly);
            Field(_ => _.CreationTime);
        }
    }
}