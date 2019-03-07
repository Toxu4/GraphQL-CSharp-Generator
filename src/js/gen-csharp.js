const { transformDocumentsFiles } = require('graphql-codegen-core');

const Handlebars = require('Handlebars');
const fs = require('fs');

const mainTemplate = fs.readFileSync('./Templates/Main.handlebars');
const files = fs.readdirSync('./Templates/');

Handlebars.registerHelper('currentTime', function() {
  var now     = new Date();
  var year    = now.getFullYear();
  var month   = now.getMonth()+1;
  var day     = now.getDate();
  var hour    = now.getHours();
  var minute  = now.getMinutes();
  var second  = now.getSeconds();
  if(month.toString().length == 1) {
    month = '0'+month;
  }
  if(day.toString().length == 1) {
    day = '0'+day;
  }
  if(hour.toString().length == 1) {
    hour = '0'+hour;
  }
  if(minute.toString().length == 1) {
    minute = '0'+minute;
  }
  if(second.toString().length == 1) {
    second = '0'+second;
  }
  var dateTime = year+'/'+month+'/'+day+' '+hour+':'+minute+':'+second;
  return dateTime;
});

Handlebars.registerHelper('json',  function(data) {
  return data ? JSON.stringify(data) : "[undefined]";
});
        
files.forEach(f => {
  if (!f.endsWith('.handlebars')){
    return;
  }

  if (f != 'Main.handlebars'){
    const partial = fs.readFileSync('./Templates/' + f);
    Handlebars.registerPartial(f.split('.')[0], '' + partial);
  }
});

const compile = Handlebars.compile('' + mainTemplate);    

module.exports = {  
    plugin: (schema, documents, config) => {
    
      const transformedDocuments = transformDocumentsFiles(schema, documents);     

      transformedDocuments.config = config;
      
      const apiClasses = makeApiClasses(transformedDocuments);

      return compile(apiClasses);
    }
}

function makeApiClasses(documents){
    
  const queryClasses = documents.operations.map(operation => {
    let queryClass = createClassBlank(
      `${firstUpper(operation.name)}Query`, 
      {
        originalFile : operation.originalFile
      });
    
    queryClass.implementedInterfaces.push("IGraphQlQuery");

    queryClass.nestedClasses.push(getQueryResultClass(operation, documents));
    queryClass.properties.push(getQueryTextProperty(operation, documents));
    
    let variablesProperties = getVariablesProperties(operation, documents);
    variablesProperties.forEach(p => queryClass.properties.push(p));

    addConstructor(queryClass, operation, documents);
    
    return queryClass;
  });
  
  const querySetInterfaces = [];
  
  groupBy(queryClasses, qc => qc.originalFile)
    .forEach(qcg => {
      let fileName = qcg[0].originalFile.match(/(.*\/)?(.*?)\.graphql/i)[2];

      let interface = createInterfaceBlank(`I${firstUpper(fileName)}Processor`);
      
      qcg.forEach(qc => {
        let method = createMethodBlank(
          qc.name.match(/(.*)Query/)[1],
          {
            resultType: `Task<QueryResult<${qc.name}.Result>>`,
            parameters:
            [
              createParameterBlank(qc.name, "query", { isRequired: true })
            ]
          });
        
        interface.methods.push(method);
      });

      querySetInterfaces.push(interface);
    })
  
  return {
    config : documents.config,
    queryClasses,
    querySetInterfaces
  }
}

function getQueryResultClass(operation, documents){
  let queryResultClass =  createClassBlank("Result");
  
  fillResultClassMembers(queryResultClass, operation.selectionSet, documents);
  
  return queryResultClass;
}

function getQueryTextProperty(operation, documents){
  let queryParts = [operation.document];

  getUsedFragments(operation, documents)
    .forEach(f => queryParts.push(f.document));
  
  let queryTextProperty = createPropertyBlank(
    "string", 
    "QueryText", 
    {
      initialValue : `\n@"\n${queryParts.join('\n')}\n"` 
    });
  
  return queryTextProperty;
}

function getVariablesProperties(operation, documents){
  let res = 
      [
        createPropertyBlank(
          "IDictionary<string, object>",
          "Variables",
          {
            initialValue : 
              "new Dictionary<string, object>\n" +
              "{\n" +
                operation.variables
                  .map(o => {
                    return `\t{ "${o.name}", null }`
                  })
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
    ))
  });
  
  return res;
}

function addConstructor(queryClass, operation, documents) {
  let constructor = createMethodBlank(
    queryClass.name,
    {
      isConstructor : true
    })
  
  operation.variables.forEach(v =>{
    let parameter = createParameterBlank(
      getCsharpType(v), 
      v.name, 
      {
        isRequired: v.isRequired  
      });
    
    constructor.parameters.push(parameter);
    constructor.body.push(`${firstUpper(v.name)} = ${v.name};`)
  });
  
  constructor.parameters.sort((p1, p2) => {
    if (p1.isRequired && p2.isRequired){
      return 0;
    }
    if (p1.isRequired){
      return -1
    }
    return 1;
  })
  
  queryClass.methods.push(constructor);
}

function fillResultClassMembers(resultClass, selectionSet, documents){
  if (!selectionSet){
    return;
  }
  
  selectionSet.forEach(s => {
    if (s.isType || s.isUnion){
      let nestedClassName = `${firstUpper(s.name)}Result`;
      let nestedClass = createClassBlank(
        nestedClassName, 
        { 
          isAbstract : s.isUnion 
        });
      
      resultClass.nestedClasses.push(nestedClass)
      resultClass.properties.push(
        createPropertyBlank(
          nestedClassName,
          firstUpper(s.name),
          {
            isArray : s.isArray
          }
        ))

      if (!s.isUnion){
        fillResultClassMembers(nestedClass, s.selectionSet, documents);  
      }
      else 
      {
        fillUnionResultClassMembers(resultClass, nestedClass, s.selectionSet, documents);
      }
    }
    else if (s.isField){
      resultClass.properties.push(
        createPropertyBlank(
          getCsharpType(s),
          firstUpper(s.name)
        ))
    }
    else if (s.isFragmentSpread){
      let fragment = documents.fragments.find(f => f.name == s.fragmentName);

      fillResultClassMembers(resultClass, fragment.selectionSet, documents)
    }
  })
}

function fillUnionResultClassMembers(resultClass, unionClass, selectionSet, documents){
  if (!selectionSet){
    return;
  }

  selectionSet.forEach(s =>{
    let unionClassPart = createClassBlank(`${s.onType}Result`, { parentClass: unionClass.name });
    
    fillResultClassMembers(unionClassPart, s.selectionSet, documents);
    
    resultClass.nestedClasses.push(unionClassPart);
  })

  unionClass.properties.push(
    createPropertyBlank(
      "string",
      "__typename"));
}

function getUsedFragments(operation, documents) {
  let allFragments = 
    getSelectionSets(operation)
      .filter(s => s.isFragmentSpread)
      .map(s => s.fragmentName)
      .filter((f, i, a) => a.indexOf(f) === i);
  
  return documents
      .fragments
      .filter(f => allFragments.includes(f.name));
}

function getSelectionSets(root) 
{
  if (!root.selectionSet){
    return [];
  }
  
  return root
    .selectionSet
    .concat(
      root
        .selectionSet
        .flatMap(s => getSelectionSets(s)));
}

function createInterfaceBlank(name, options){
  options = options || {};

  return {
    accessModifier : options.accessModifier || "public",
    name: name,
    methods: []
  }
}

function createClassBlank(name, options){
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
    methods: [],

    originalFile: options.originalFile
  }
}

function createPropertyBlank(type, name, options){
  options = options || {};
  
  return {
    accessModifier : options.accessModifier || "public",
    type: type,
    name: name,
    getter: options.getter || "get;",
    setter: options.setter || "set;",
    initialValue: options.initialValue,
    isArray: options.isArray
  }
}

function createMethodBlank(name, options){
  options = options || {};

  return {
    accessModifier : options.accessModifier || "public",
    resultType: options.resultType || "void",
    name: name,
    isConstructor: options.isConstructor,
    parameters: options.parameters || [],    
    body: options.body || []
  }
}

function createParameterBlank(type, name, options){
  options = options || {};

  return {
    type: type,
    name: name,
    isRequired: options.isRequired,
    isOut: options.isOut,
    isArray: options.isArray
  }
}

function firstUpper(value){
  return value.replace(/(\w)(\w*)/g, function(g0,g1,g2){return g1.toUpperCase() + g2;});
}

function getCsharpType(v) {

  const typeMapping = {
    "String" : 'string',
    "Int" : 'int',
    "Float" : 'float',
    "Boolean" : 'bool',
    "ID" : 'string',
  }

  let res = typeMapping[v.type];

  if (v.isArray){
    res = `${res}[]`
  }
  else if (!v.isRequired && v.type != 'String'){
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
    } else {
      collection.push(item);
    }
  });
  return map;
}
