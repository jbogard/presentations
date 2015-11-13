exec = require('child_process').exec
colors = require('colors')
fs = require('fs')

# Utils
String::repeat = (num) ->
  new Array(num + 1).join(this)

# Helpers
execute = (command, callback) ->
  start = (new Date).getTime()
  executing = "Executing: #{command}"
  separators = '-'.repeat(executing.length)

  console.log "\n#{separators.yellow}"
  console.log executing.yellow
  console.log separators.yellow

  # Child process
  child = exec command, (error, stdout, stderr) ->
    throw stderr if stderr

    # Time taken to `exec`
    end = (new Date).getTime()
    console.log stdout + "=> Done: #{(end - start) / 1000}s".green

    # Callback
    callback?()

  # Continuous stdout log
  child.stdout.on 'data', (data) ->
    console.log data.toString().trim()

# Tasks
task 'minify', 'Minify react-dom', ->
  execute './node_modules/.bin/uglifyjs --comments /react-dom/ --output react-dom.min.js react-dom.js', ->
    fs.appendFile('./react-dom.min.js', '\n')
