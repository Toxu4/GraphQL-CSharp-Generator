#!/usr/bin/env node

const args = parseCommandArgs();
const [tempConfigFileName, cleanupTempFile] = createTempConfig(args);

const cmd = require("os").platform() === "win32" ? "node.exe" : "node";

require("child_process")
    .spawn(
        cmd, 
        [`${__dirname}\\node_modules\\graphql-code-generator\\dist\\cli.js`, "--config", tempConfigFileName], 
        { stdio: "inherit" })
    .on("exit", function() {
        cleanupTempFile();
    });

function parseCommandArgs() {
    const args = require("commander");

    args
        .option("-s, --schema <schema>", "path to json with schema, or schema url. e.g. -s ./schema.json")
        .option("-d, --documents <documents>", "path with to documents with queries. e.g. -d ./Queries/*.graphql")
        .option("-o, --output <output>", "output file name. e.g. -o ./Generated.cs")
        .option("-n, --namespace <namespace>", "namespace of generated file. e.g. -n MyGraphQlClient")
        .parse(process.argv);

    if (["schema", "documents", "output", "namespace"].map(expect).some(exists => !exists)) {
        console.log();
        args.outputHelp();

        process.exit(1);
    }
    
    // o-o-ops
    args.pluginDir = __dirname;

    return args;

    function expect(key) {
        if (!args[key]) {            
            console.error(`${key} expected`);
            return false;
        }
        return true;
    }
}

function createTempConfig(args) {   
    var tempConfigFileName = require("tmp").tmpNameSync({ prefix: "gen-csharp-config-", postfix: ".yml" });

    const fs = require("fs");

    const configTemplate = fs.readFileSync(`${__dirname}/Templates/config.handlebars`);
    const compile = require("Handlebars").compile("" + configTemplate);
    
    const fd = fs.openSync(tempConfigFileName, "a");
    fs.appendFileSync(tempConfigFileName, compile(args), "utf8");
    fs.closeSync(fd);

    return [
        tempConfigFileName, 
        function() { 
            fs.unlinkSync(tempConfigFileName); 
        }
    ];
}