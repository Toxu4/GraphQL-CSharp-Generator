using GraphQL.Types;

namespace GraphQlServer.Schema.Types
{
    public class DriveQuery : ObjectGraphType
    {
        public DriveQuery()
        {
            Field<ListGraphType<DriveType>>()
                .Name("list")
                .Resolve(context => System.IO.DriveInfo.GetDrives());
        }
    }
}