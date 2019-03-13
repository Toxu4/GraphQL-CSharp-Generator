using System;
using System.IO;
using System.Linq;
using GraphQL.Types;

namespace GraphQlServer.Schema.Types
{
    public class FolderType : ObjectGraphType<DirectoryInfo>
    {
        public FolderType()
        {
            Field(_ => _.Name);
            Field(_ => _.FullName);
            Field(_ => _.CreationTime);
            
            Field<ListGraphType<FolderOrFileType>>()
                .Name("content")
                .Resolve(context => Directory
                    .GetDirectories(context.Source.FullName)
                    .Select(d => new DirectoryInfo(d)).Cast<object>()
                    .Union(
                        Directory
                            .GetFiles(context.Source.FullName)
                            .Select(f => new FileInfo(f))));            
        }
    }
}