# GraphQL-CSharp-Generator

overwrite: true
schema: './schema.json'
documents: './Queries/*.graphql'
generates:
  ./Generated.cs:
    - ./gen-csharp.js:
        printTime: true
        namespace: 'GraphQlClasses'