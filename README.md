# GraphQL-CSharp-Generator

[![Build status](https://ci.appveyor.com/api/projects/status/2lbxr0qk6csiparf/branch/master?svg=true)](https://ci.appveyor.com/project/Toxu4/graphql-csharp-generator/branch/master)
![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/Toxu4.GraphQl.Client.svg)
![npm](https://img.shields.io/npm/v/graphql.csharp.generator.svg)

## Getting started

To get started with sample you need:

- nodejs v11
- .net cli


In your .net projects folder clone repository execute following commands:

```
git clone https://github.com/Toxu4/GraphQL-CSharp-Generator.git

cd GraphQL-CSharp-Generator\sample\GraphQlServer

dotnet run
```

Sample GraphQl sample server will start. You can surf api using playground:

```
http://localhost:5000/ui/playground
```

In new console window switch to your projects directory and execute following commands:

```
dotnet new console -n MyCoolGraphqlApp

cd .\MyCoolGraphqlApp

npm i -g graphql.csharp.generator

dotnet add package Toxu4.GraphQl.Client -v 1.0.15
dotnet add package Microsoft.Extensions.DependencyInjection
```

create and place into MyCoolGraphqlApp directory file computerQueries.graphql with following content:

```
query getDrives{
  drives{
    list{
  		name
      content{
        __typename
        ... on FolderType{
          fullName
        }
        ... on FileType{
          name
        }
      }
    }
  }
}
```

Generate code:

```
gql-gen-csharp -s http://localhost:5000/graphql -d ./*.graphql -o Generated.cs -n MyCoolGraphqlApp
```

Replace program.cs file content with:

```
using System;
using Microsoft.Extensions.DependencyInjection;
using Toxu4.GraphQl.Client;
using System.Threading.Tasks;

namespace MyCoolGraphqlApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var queries = 
                new ServiceCollection()
                    .AddGraphQlClient(settings => settings.Endpoint = "http://localhost:5000/graphql")
                    .AddGeneratedQueries()
                    .BuildServiceProvider()
                    .GetRequiredService<IComputerQueries>();

            var (result, _) = queries.GetDrives(new GetDrivesQuery()).GetAwaiter().GetResult();

            foreach (var drive in result.Drives.List){
                Console.WriteLine($"Drive: {drive.Name}");
                foreach(var content in drive.Content){
                    switch (content){
                        case GetDrivesQuery.Result.DrivesResult.ListResult.FolderTypeResult folder:
                            Console.WriteLine($"Folder: {folder.FullName}");
                            break;
                        case GetDrivesQuery.Result.DrivesResult.ListResult.FileTypeResult file:
                            Console.WriteLine($"File: {file.Name}");
                            break;                        
                    }
                }
            }
        }
    }
}
```

Run application

```
dotnet run
```

## limitations

There are some query limitations. 

- does not support interfaces
- does not support mutations
