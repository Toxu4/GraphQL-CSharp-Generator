# GraphQL-CSharp-Generator

Generates c# classes to access GraphQl API. 

To use this tool you need to install npm package:
```
npm install -g graphql.csharp.generator
```

[WIP] and add nuget package into your c# project

[WIP] 
```
dotnet add package Toxu4.GraphQl.Client
```

use cli to generate c# code:

```
gql-gen-csharp -s .\schema.json -d .\Queries\*.graphql -o .\Generated.cs -n MyProject.GraphQl
```
