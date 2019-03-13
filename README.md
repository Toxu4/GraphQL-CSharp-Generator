# GraphQL-CSharp-Generator

[![Build status](https://ci.appveyor.com/api/projects/status/2lbxr0qk6csiparf/branch/master?svg=true)](https://ci.appveyor.com/project/Toxu4/graphql-csharp-generator/branch/master)
![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/Toxu4.GraphQl.Client.svg)

Generates c# classes to access GraphQl API. 

To use this tool you need to install npm package:
```
npm install -g graphql.csharp.generator
```

[WIP] and add nuget package into your c# project

```
dotnet add package Toxu4.GraphQl.Client
```

use cli to generate c# code:

```
gql-gen-csharp -s .\schema.json -d .\Queries\*.graphql -o .\Generated.cs -n MyProject.GraphQl
```


## limitations

There are some query limitations. 

- does not support interfaces
- does not support mutations
