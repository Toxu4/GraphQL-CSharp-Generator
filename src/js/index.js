const os = require('os');
const spawn = require("child_process").spawn;

let cmd = os.platform() === 'win32' ? 'node.exe' : 'node';

let config = process.argv[2] || './default.yml'

let gen = spawn(cmd, ['.\\node_modules\\graphql-code-generator\\dist\\cli.js','--config', config], {stdio: 'inherit'})
gen.on('error', function(err) {
    console.error(err);
    process.exit(1);
});
