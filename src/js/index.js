#!/usr/bin/env node
console.log(__dirname)

let program = require('commander');

program
    .option('-s, --schema <schema>', 'path to json with schema, or schema url. e.g. -s ./schema.json')
    .option('-d, --documents <documents>', 'path with to documents with queries. e.g. -d ./Queries/*.graphql')
    .option('-o, --output <output>', 'output file name. e.g. -o ./Generated.cs')
    .option('-n, --namespace <namespace>', 'namespace of generated file. e.g. -n MyGraphQlClient')
    .parse(process.argv);

const fs = require('fs');
const Handlebars = require('Handlebars');

const configTemplate = fs.readFileSync(`${__dirname}/Templates/config.handlebars`);
const compile = Handlebars.compile('' + configTemplate);

if (!program.schema){
    console.error("schema expected");
    process.exit(1);    
}

if (!program.documents){
    console.error("documents expected");
    process.exit(1);    
}

if (!program.output){
    console.error("output expected");
    process.exit(1);    
}

if (!program.namespace){
    console.error("namespace expected");
    process.exit(1);    
}

var tempConfigFileName = require('tmp').tmpNameSync({ prefix: 'gen-csharp-config-', postfix: '.yml'});

program.pluginDir = __dirname;

let fd = fs.openSync(tempConfigFileName, 'a');
fs.appendFileSync(tempConfigFileName, compile(program), 'utf8');
fs.closeSync(fd);

const os = require('os');
const spawn = require("child_process").spawn;

let cmd = os.platform() === 'win32' ? 'node.exe' : 'node';

let gen = spawn(cmd, [`${__dirname}\\node_modules\\graphql-code-generator\\dist\\cli.js`,'--config', tempConfigFileName], {stdio: 'inherit'})
gen.on('error', function(err) {
    console.error(err);
    process.exit(1);
});
gen.on('exit', function() {
    fs.unlinkSync(tempConfigFileName);
});
