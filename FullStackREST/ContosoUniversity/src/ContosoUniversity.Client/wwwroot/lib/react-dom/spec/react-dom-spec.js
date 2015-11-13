React = require('react')
DOM = require('../react-dom')

describe('react-dom', function() {
  beforeEach(function() { spyOn(React.DOM, 'div') })

  it('acts as a proxy', function() {
    DOM.div({}, 'foo')
    expect(React.DOM.div).toHaveBeenCalled()
  })

  it('has optional `opts` argument', function() {
    DOM.div('foo')
    expect(React.DOM.div).toHaveBeenCalledWith(null, 'foo')
  })

  it('replaces `class` with `className`', function() {
    DOM.div({ class: 'foozle' }, 'foo')
    expect(React.DOM.div).toHaveBeenCalledWith({ className: 'foozle' }, 'foo')
  })

  describe('`data` attribute', function() {
    it('nests `data` attributes', function() {
      DOM.div({ data: { a: 'b', c: 'd', e: { f: 'g', h: 'i' } } }, 'foo')
      expect(React.DOM.div).toHaveBeenCalledWith({
        'data-a': 'b',
        'data-c': 'd',
        'data-e-f': 'g',
        'data-e-h': 'i'
      }, 'foo')
    })

    it('handles array', function() {
      DOM.div({ data: { state: ['selected', 'opened'] } }, 'foo')
      expect(React.DOM.div).toHaveBeenCalledWith({
        'data-state-selected': true,
        'data-state-opened': true
      }, 'foo')
    })
  })
})
