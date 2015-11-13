# DOM
DOM is a `React.DOM` wrapper with — subjectively — more sanity and awesome helpers.

## API
#### Optional `options`
```js
DOM.div('foo')
// => React.DOM.div(null, 'foo')
```

#### Allow `class` attribute
```js
DOM.div({ class: 'foozle' }, 'foo')
// => React.DOM.div({ className: 'foozle' }, 'foo')
```

### The `data` attribute
#### Nest objects
```js
DOM.div({ data: { state: 'selected' }}, 'foo')
// => React.DOM.div({ 'data-state': 'selected' }, 'foo')
```

#### Handle arrays
```js
DOM.div({ data: { state: ['selected', 'opened'] } }, 'foo')
// => React.DOM.div({ 'data-state-selected': true, 'data-state-opened': true }, 'foo')
```

## CoffeeScript bliss
I’ll be honest, I don’t really like [JSX](http://facebook.github.io/react/docs/jsx-in-depth.html). It makes you think you’re writing HTML while it is in fact XML. It is getting converted into JavaScript anyway and I prefer not confusing any of my collegues into thinking this is HTML. Since most (read: all) of the projects that I work on are in CoffeeScript, I find the syntax bearable enough.

```coffee
Hello = React.createClass
  componentWillMount: -> console.log('componentWillMount')
  componentDidMount:  -> console.log('componentDidMount')
  # …

  render: ->
    DOM.div class: 'container', [
      DOM.span
        data:
          foo: bar: 'baz'
          much: much: 'fun'
        class: 'test'
      , "Hello #{@props.name}"

      DOM.span ', goodbye.'
    ]

console.log React.renderComponentToString Hello(name: 'World')
# <div class="container">
#   <span data-foo-bar="baz" data-much-much="fun" class="test">Hello World</span>
#   <span>, goodbye.</span>
# </div>
```

Now, I know that not everybody works with CoffeeScript, so [here’s the snippet in JavaScript](https://gist.github.com/EtienneLem/3c52167b2ccb6e180132). You can also [see the examples](/examples/index.html), which are in JavaScript.

## Usage
react-dom is available via `npm install react-dom` or `bower install react-dom`. AMD, CommonJS and broswer globals are supported.

## Tests
Run the `npm test` task.
