class InstructorsInfo extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            instructors: {},
            courses: {},
            students: {}
        };
        this._handleSelect = this._handleSelect.bind(this);
    }
    componentDidMount() {
        $.getJSON(this.props.href)
            .done(data => this.setState({ instructors: data }));
    }
    _handleSelect(e) {
        $.getJSON(e.href)
            .done(data => {
                var state = e.rel === "courses"
                    ? { students: {} }
                    : {};

                state[e.rel] = data;

                this.setState(state);
            });
    }
    render() {
        return (
            <div>
                <CollectionJsonTable data={this.state.instructors} onSelect={this._handleSelect} />
                <CollectionJsonTable data={this.state.courses} onSelect={this._handleSelect} />
                <CollectionJsonTable data={this.state.students} onSelect={this._handleSelect} />
            </div>
        );
    }
}

class CollectionJsonTable extends React.Component {
    render() {
        if (!this.props.data.collection) {
            return <div></div>;
        }
        if (!this.props.data.collection.items.length) {
            return <p>No items found.</p>;
        }

        const containsLinks = _(this.props.data.collection.items)
            .some(item => item.links && item.links.length);
        const rows = _(this.props.data.collection.items)
            .map((item, idx) => <CollectionJsonTableRow
                                    item={item}
                                    containsLinks={containsLinks}
                                    onSelect={this.props.onSelect}
                                    key={idx} />)
            .value();
        return (
            <table className="table">
                <CollectionJsonTableHeader data={this.props.data.collection.items} containsLinks={containsLinks} />
                <tbody>
                {rows}
                </tbody>
            </table>
        );
    }
}

class CollectionJsonTableHeader extends React.Component {
    render() {
        const headerCells = _(this.props.data[0].data)
            .map((datum, idx) => <th key={idx}>{datum.prompt}</th>)
            .value();
        if (this.props.containsLinks) {
            headerCells.push(<th key="links"></th>);
        }

        return (
            <thead>
            <tr>
                {headerCells}
            </tr>
            </thead>
        );
    }
}


class CollectionJsonTableRow extends React.Component {
    render() {
        const dataCells = _(this.props.item.data)
            .map((datum, idx) => <td key={idx}>{datum.value}</td>)
            .value();
        if (this.props.containsLinks) {
            dataCells.push(<CollectionJsonTableLinkCell key="links" links={this.props.item.links} onSelect={this.props.onSelect} />);
        }

        return (
            <tr>
                {dataCells}
            </tr>
        );
    }
}

class CollectionJsonTableLinkCell extends React.Component {
    render() {
        const links = _(this.props.links)
            .map((link, idx) => <CollectionJsonTableLink key={idx} link={link} onSelect={this.props.onSelect} />)
            .value();
        return (
            <td>{links}</td>
        );
    }
}


class CollectionJsonTableLink extends React.Component {
    constructor(props) {
        super(props);
        this._handleClick = this._handleClick.bind(this);
    }
    _handleClick(e) {
        e.preventDefault();
        this.props.onSelect({
                href: this.props.link.href,
                rel: this.props.link.rel
            }
        );
    }
    render() {
        return (
            <a href="#" rel={this.props.link.rel} onClick={this._handleClick}>
                {this.props.link.prompt}
            </a>
        );
    }
}

CollectionJsonTableLink.propTypes = {
    onSelect: React.PropTypes.func.isRequired
};

