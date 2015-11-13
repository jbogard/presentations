/*
* react-dom v0.1.0
* https://github.com/EtienneLem/react-dom
*
* Copyright 2014, Etienne Lemay http://heliom.ca
* Released under the MIT license
*/

(function (root, factory) {
  // AMD
  if (typeof define === 'function' && define.amd) {
    define(['React'], factory)

  // Node.js or CommonJS
  } else if (typeof exports !== 'undefined') {
    module.exports = factory(require('react'))

  // Browser globals
  } else {
    root.DOM = factory(root.React)
  }
}(this, function (React) {
  var DOM, key, value, ref, bridge

  DOM = {}
  ref = React.DOM

  proxy = function(key, value) {
    // Only proxy `React.DOM.<element>` functions
    if (typeof value !== 'function') { return }

    DOM[key] = function(opts, children) {
      var extractObjectsAsKeys

      // Make the `opts` argument optional
      if (children === void 0) {
        children = opts
        opts = null
      }

      // Nest `data` attributes
      if (opts && opts.data) {
        extractObjectsAsKeys = function(data, root) {
          var dataKey, dataValue
          if (root == null) { root = '' }

          for (dataKey in data) {
            dataValue = data[dataKey]

            // Instance of Array
            if (dataValue instanceof Array) {
              var i, arrayValue
              for (i = 0; i < dataValue.length; i++) {
                arrayValue = dataValue[i]
                opts['data' + (root + '-' + dataKey + '-' + arrayValue)] = true
              }

              continue

            // Instance of Object
            } else if (dataValue instanceof Object) {
              extractObjectsAsKeys(dataValue, (root + dataKey) + '-')
              continue
            }

            opts['data-' + (root + dataKey)] = dataValue
          }
        }

        extractObjectsAsKeys(opts.data)
        delete opts.data
      }

      // Make `class` -> `className`
      if (opts && opts.class) {
        opts.className = opts.class
        delete opts.class
      }

      // Relay to original `React.DOM.<element>`
      return ref[key](opts, children)
    }
  }

  // Scope proxy
  for (key in ref) {
    value = ref[key]
    proxy(key, value)
  }

  return DOM
}))
