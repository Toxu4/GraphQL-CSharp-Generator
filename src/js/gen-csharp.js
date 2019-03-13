const { transformDocumentsFiles } = require("graphql-codegen-core");

const Handlebars = require("Handlebars");
const fs = require("fs");

const mainTemplate = fs.readFileSync(`${__dirname}/Templates/Main.handlebars`);
const files = fs.readdirSync(`${__dirname}/Templates/`);

Handlebars.registerHelper("currentTime", function() {
  var now     = new Date();
  var year    = now.getFullYear();
  var month   = now.getMonth()+1;
  var day     = now.getDate();
  var hour    = now.getHours();
  var minute  = now.getMinutes();
  var second  = now.getSeconds();
  if (month.toString().length == 1) {
    month = "0"+month;
  }
  if (day.toString().length == 1) {
    day = "0"+day;
  }
  if (hour.toString().length == 1) {
    hour = "0"+hour;
  }
  if (minute.toString().length == 1) {
    minute = "0"+minute;
  }
  if (second.toString().length == 1) {
    second = "0"+second;
  }
  var dateTime = year+"/"+month+"/"+day+" "+hour+":"+minute+":"+second;
  return dateTime;
});

Handlebars.registerHelper("json",  function(data) {
  return data ? JSON.stringify(data) : "[undefined]";
});
        
files.forEach(f => {
  if (!f.endsWith(".handlebars")) {
    return;
  }

  if (f != "Main.handlebars") {
    const partial = fs.readFileSync(`${__dirname}/Templates/${f}`);
    Handlebars.registerPartial(f.split(".")[0], "" + partial);
  }
});

const compile = Handlebars.compile("" + mainTemplate);    

module.exports = {  
    plugin: (schema, documents, config) => {
    
      const transformedDocuments = transformDocumentsFiles(schema, documents);     

      transformedDocuments.config = config;
      
      const apiClasses = makeApiClasses(transformedDocuments);

      return compile(apiClasses);
    }
};

function makeApiClasses(documents) {
    
  const queryClasses = documents.operations.map(operation => {
    const queryClass = createClassBlank(
      `${firstUpper(operation.name)}Query`, 
      {
        originalFile : operation.originalFile
      });
    
    queryClass.implementedInterfaces.push("IGraphQlQuery");

    queryClass.nestedClasses.push(getQueryResultClass(operation, documents));
    queryClass.properties.push(getQueryTextProperty(operation, documents));
    
    getVariablesProperties(operation)
      .forEach(p => queryClass.properties.push(p));

    addConstructor(queryClass, operation);
    
    return queryClass;
  });
  
  const querySetInterfaces = [];
  const querySetImplementations = [];
  const registrationExtensionsClass = createClassBlank(
      "RegistrationExtensions", 
      {
          accessModifier: "public static",
          methods: [ createMethodBlank(
              "AddGeneratedQueries",
              {
                  accessModifier: "public static",
                  isExtension: true,
                  resultType: "IServiceCollection",
                  parameters: [createParameterBlank("IServiceCollection", "serviceCollection", { isRequired: true })]
              })]
      });
  
  groupBy(queryClasses, qc => qc.originalFile)
    .forEach(qcg => {
      const fileName = qcg[0].originalFile.match(/(.*\/)?(.*?)\.graphql/i)[2];

      const interface = createInterfaceBlank(`I${firstUpper(fileName)}`);
      const implementation = createClassBlank(`${firstUpper(fileName)}`, { accessModifier: "internal" });

      registrationExtensionsClass.methods[0].body.push(`serviceCollection.AddSingleton<${interface.name}, ${implementation.name}>();`)
      
      implementation.implementedInterfaces.push(interface.name);
      implementation.fields. push(createFieldBlank("IGraphQlQueryExecutor", "_graphQlQueryExecutor"));
      
      const ctor = createMethodBlank(
          implementation.name,
          {
              isConstructor : true,
              parameters: [createParameterBlank("IGraphQlQueryExecutor", "graphQlQueryExecutor")],
              body: ["_graphQlQueryExecutor = graphQlQueryExecutor;"]
          });
      
      implementation.methods.push(ctor);
      
      qcg.forEach(qc => {
        const method = createMethodBlank(
          qc.name.match(/(.*)Query/)[1],
          {
            resultType: `Task<QueryResult<${qc.name}.Result>>`,
            parameters:
            [
              createParameterBlank(qc.name, "query", { isRequired: true })
            ],
            body: [`return _graphQlQueryExecutor.Run<${qc.name},QueryResult<${qc.name}.Result>>(query);`]
          });
        
        interface.methods.push(method);
        implementation.methods.push(method);
      });

      querySetInterfaces.push(interface);
      querySetImplementations.push(implementation);
    });

  registrationExtensionsClass.methods[0].body.push("return serviceCollection;");  
  
  return {
    config : documents.config,
    queryClasses,
    querySetInterfaces,
    querySetImplementations,
    registrationExtensionsClass
  };
}

function getQueryResultClass(operation, documents) {
  const queryResultClass =  createClassBlank("Result");
  
  fillResultClassMembers(queryResultClass, operation.selectionSet, documents);
  
  return queryResultClass;
}

function getQueryTextProperty(operation, documents) {
  const queryParts = [operation.document];

  getUsedFragments(operation, documents)
    .forEach(f => queryParts.push(f.document));
  
  const queryTextProperty = createPropertyBlank(
    "string", 
    "QueryText", 
    {
      initialValue : `\n@"\n${queryParts.join("\n")}\n"` 
    });
  
  return queryTextProperty;
}

function getVariablesProperties(operation) {
  const res = 
      [
        createPropertyBlank(
          "IDictionary<string, object>",
          "Variables",
          {
            initialValue : 
              "new Dictionary<string, object>\n" +
              "{\n" +
                operation.variables
                  .map(o => `\t{ "${o.name}", null }`)
                  .join(",\n") +              
              "\n}"
          })
      ];
  
  operation.variables.forEach(v =>{
    res.push(createPropertyBlank(
      getCsharpType(v),
      firstUpper(v.name),
      {
        getter: `get => (${getCsharpType(v)})Variables["${v.name}"];`,
        setter: `set => Variables["${v.name}"] = value;`
      }
    ));
  });
  
  return res;
}

function addConstructor(queryClass, operation) {
  const ctor = createMethodBlank(
    queryClass.name,
    {
      isConstructor : true
    });
  
  operation.variables.forEach(v =>{
    const parameter = createParameterBlank(
      getCsharpType(v), 
      v.name, 
      {
        isRequired: v.isRequired  
      });
    
    ctor.parameters.push(parameter);
    ctor.body.push(`${firstUpper(v.name)} = ${v.name};`);
  });
  
  ctor.parameters.sort((p1, p2) => {
    if (p1.isRequired && p2.isRequired) {
      return 0;
    }

    return p1.isRequired ? -1 : 1;
  });
  
  queryClass.methods.push(ctor);
}

function fillResultClassMembers(resultClass, selectionSet, documents) {
  if (!selectionSet) {
    return;
  }
  
  selectionSet.forEach(s => {
    if (s.isType || s.isUnion) {
      const nestedClassName = `${firstUpper(s.name)}Result`;
      const nestedClass = createClassBlank(
        nestedClassName, 
        { 
          isAbstract : s.isUnion 
        });
      
      resultClass.nestedClasses.push(nestedClass);
      resultClass.properties.push(
        createPropertyBlank(
          nestedClassName,
          firstUpper(s.name),
          {
            isArray : s.isArray
          }
        ));

      if (!s.isUnion) {
        fillResultClassMembers(nestedClass, s.selectionSet, documents);  
      }
      else {
        fillUnionResultClassMembers(nestedClass, s.selectionSet, documents);
      }
    }
    else if (s.isField) {
      resultClass.properties.push(
        createPropertyBlank(
          getCsharpType(s),
          firstUpper(s.name)
        ));
    }
    else if (s.isFragmentSpread) {
      const fragment = documents.fragments.find(f => f.name == s.fragmentName);

      fillResultClassMembers(resultClass, fragment.selectionSet, documents);
    }
  });
}

function fillUnionResultClassMembers(unionClass, selectionSet, documents) {
  if (!selectionSet) {
    return;
  }

  selectionSet.forEach(s =>{
    const unionClassPart = createClassBlank(`${s.onType}Result`, { parentClass: unionClass.name });
    
    fillResultClassMembers(unionClassPart, s.selectionSet, documents);
    
      unionClass.nestedClasses.push(unionClassPart);
  });

  unionClass.properties.push(
    createPropertyBlank(
      "string",
      "__typename"));
}

function getUsedFragments(operation, documents) {
  const allFragments = 
    getSelectionSets(operation)
      .filter(s => s.isFragmentSpread)
      .map(s => s.fragmentName)
      .filter((f, i, a) => a.indexOf(f) === i);
  
  return documents
      .fragments
      .filter(f => allFragments.includes(f.name));
}

function getSelectionSets(root) {
  if (!root.selectionSet) {
    return [];
  }
  
  return root
    .selectionSet
    .concat(
      root
        .selectionSet
        .flatMap(s => getSelectionSets(s)));
}

function createInterfaceBlank(name, options) {
  options = options || {};

  return {
    accessModifier : options.accessModifier || "public",
    name: name,
    methods: []
  };
}

function createClassBlank(name, options) {
  options = options || {};
  
  return {
    accessModifier : options.accessModifier || "public",
    isAbstract : options.isAbstract,
    name: name,
    parentClass: options.parentClass,
    
    implementedInterfaces: [],

    nestedClasses: [],
    nestedInterfaces: [],
    fields: [],
    properties: [],
    methods: options.methods || [],

    originalFile: options.originalFile
  };
}

function createFieldBlank(type, name, options) {
    options = options || {};

    return {
        accessModifier : options.accessModifier || "private readonly",
        type: type,
        name: name,
        initialValue: options.initialValue,
        isArray: options.isArray
    };
}

function createPropertyBlank(type, name, options) {
  options = options || {};
  
  return {
    accessModifier : options.accessModifier || "public",
    type: type,
    name: name,
    getter: options.getter || "get;",
    setter: options.setter || "set;",
    initialValue: options.initialValue,
    isArray: options.isArray
  };
}

function createMethodBlank(name, options) {
  options = options || {};

  return {
    accessModifier : options.accessModifier || "public",
    resultType: options.resultType || "void",
    name: name,
    isConstructor: options.isConstructor,
    isExtension: options.isExtension,  
    parameters: options.parameters || [],    
    body: options.body || []
  };
}

function createParameterBlank(type, name, options) {
  options = options || {};

  return {
    type: type,
    name: name,
    isRequired: options.isRequired,
    isOut: options.isOut,
    isArray: options.isArray
  };
}

function firstUpper(value) {
  return value.replace(/(\w)(\w*)/g, function(g0, g1, g2) {
return g1.toUpperCase() + g2;
});
}

function getCsharpType(v) {

  const typeMapping = {
    "String" : "string",
    "Int" : "int",
    "Float" : "float",
    "Boolean" : "bool",
    "ID" : "string",
  };

  let res = typeMapping[v.type];

  if (v.isArray) {
    res = `${res}[]`;
  }
  else if (!v.isRequired && v.type != "String") {
    res = `${res}?`;
  }

  return res;
}

function groupBy(list, keyGetter) {
  const map = new Map();
  list.forEach((item) => {
    const key = keyGetter(item);
    const collection = map.get(key);
    if (!collection) {
      map.set(key, [item]);
    }
    else {
      collection.push(item);
    }
  });

  return map;
}
