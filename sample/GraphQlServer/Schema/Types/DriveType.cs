using System.IO;
using System.Linq;
using GraphQL.Types;

namespace GraphQlServer.Schema.Types
{
    public class DriveType : ObjectGraphType<DriveInfo>
    {
        public DriveType()
        {
            Field(_ => _.Name);
            Field(_ => _.VolumeLabel);
            Field(_ => _.DriveFormat);
            Field(_ => _.IsReady);
            Field(_ => _.TotalSize);
            Field(_ => _.AvailableFreeSpace);
            Field(_ => _.TotalFreeSpace);

            Field<ListGraphType<FolderOrFileType>>()
                .Name("content")
                .Resolve(context => 
                    Directory
                        .GetDirectories(context.Source.Name)
                        .Select(d => new DirectoryInfo(d)).Cast<object>()
                        .Union(
                            Directory
                                .GetFiles(context.Source.Name)
                                .Select(f => new FileInfo(f))));
        }
    }
}

    