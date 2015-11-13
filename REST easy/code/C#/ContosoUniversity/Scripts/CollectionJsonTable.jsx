var root = 'http://private-15052-contosouniversityapi.apiary-mock.com';

var CollectionJsonTableHeader = React.createClass({
  render: function() {
    var headerCells = _(this.props.data[0].data)
      .map(datum => <th>{datum.prompt}</th>)
      .value();

    var linkCell = this.props.containsLinks
      ? <th></th>
      : '';

    return (
      <tr>
        {headerCells}
        {linkCell}
      </tr>
    );
  }
});

var CollectionJsonTableLink = React.createClass({
  handleClick: function() {
    FetchCollection(this.props.link.href, this.props.link.rel);
    return false;
  },
  render: function() {
    return (
      <a href='#' rel={this.props.link.rel} onClick={this.handleClick}>{this.props.link.prompt}</a>
    );
  }
});

var CollectionJsonTableLinkCell = React.createClass({
  render: function() {
    var links = _(this.props.links)
      .map(link => <CollectionJsonTableLink link={link} />)
      .value();

    return (
      <td>{links}</td>
    );
  }
});

var CollectionJsonTableRow = React.createClass({
  render: function() {
    var dataCells = _(this.props.item.data)
      .map(datum => <td>{datum.value}</td>)
      .value();

    var linkCell = this.props.containsLinks
      ? <CollectionJsonTableLinkCell links={this.props.item.links} />
      : '';

    return (
      <tr>
        {dataCells}
        {linkCell}
      </tr>
    );
  }
});

var CollectionJsonTable = React.createClass({
    render: function() {
      if (!this.props.data.collection.items.length){
        return <p>No items found.</p>;
      }

      var containsLinks = _(this.props.data.collection.items)
        .some(item => item.links && item.links.length);

      var rows = _(this.props.data.collection.items)
        .map(item => <CollectionJsonTableRow
          item={item}
          containsLinks={containsLinks}
          />)
        .value();

      return (
        <table className="table">
          <CollectionJsonTableHeader
            data={this.props.data.collection.items}
            containsLinks={containsLinks} />
          {rows}
        </table>
      );
    }
});

var FetchCollection = function(relativeUrl, target) {
    $.getJSON(root + relativeUrl)
        .done(function(data) {
            React.render(
                React.createElement(CollectionJsonTable, {data : data}),
                document.getElementById(target)
            );
        })
        .fail(function (jqxhr, textStatus, error) {
            var err = textStatus + ", " + error;
            console.log("Request Failed: " + err);
        });
};
