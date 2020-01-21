import * as React from 'react';
import { connect } from 'react-redux';
import { RouteComponentProps } from 'react-router';
import { Link } from 'react-router-dom';
import { ApplicationState } from '../store';
import * as CalendarStore from '../store/CalendarStore';

type CalendarProps =
    CalendarStore.CalendarState // ... state we've requested from the Redux store
    & typeof CalendarStore.actionCreators // ... plus action creators we've requested
    & RouteComponentProps<{ lastUpdated: string }>; // ... plus incoming routing parameters

class Calendar extends React.PureComponent<CalendarProps> {
    public componentDidMount() {
        this.ensureDataFetched();
    }

    // This method is called when the route parameters change
    public componentDidUpdate() {
       // this.ensureDataFetched();
    }

    public render() {
        return (
            <React.Fragment>
                <h1 id="tabelLabel">Weather forecast</h1>
                <p>This component demonstrates fetching data from the server and working with URL parameters.</p>
                {this.renderCalendarTable()}
                {this.renderPagination()}
            </React.Fragment>
        );
    }

    private ensureDataFetched() {
        const lastUpdated = this.props.match ? this.props.match.params.lastUpdated : "";
        this.props.requestCalendar(lastUpdated);
    }

    private renderCalendarTable() {
        return (
            <table className='table table-striped' aria-labelledby="tabelLabel">
                <thead>
                    <tr>
                        <th>Date</th>
                        <th>Temp. (C)</th>
                        <th>Temp. (F)</th>
                        <th>Summary</th>
                    </tr>
                </thead>
                <tbody>
                    {this.props.calendars.map((forecast: CalendarStore.CalendarModel) =>
                        <tr key={forecast.date}>
                            <td>{forecast.date}</td>
                            <td>{forecast.temperatureC}</td>
                            <td>{forecast.temperatureF}</td>
                            <td>{forecast.summary}</td>
                        </tr>
                    )}
                </tbody>
            </table>
        );
    }

    private renderPagination() {
        const prevStartDateIndex = this.props.lastUpdated;
        const nextStartDateIndex = this.props.lastUpdated;

        return (
            <div className="d-flex justify-content-between">
                <Link className='btn btn-outline-secondary btn-sm' to={`/fetch-data/${prevStartDateIndex}`}>Previous</Link>
                {this.props.isLoading && <span>Loading...</span>}
                <Link className='btn btn-outline-secondary btn-sm' to={`/fetch-data/${nextStartDateIndex}`}>Next</Link>
            </div>
        );
    }
}

export default connect(
    (state: ApplicationState) => state.calendar, // Selects which state properties are merged into the component's props
    CalendarStore.actionCreators // Selects which action creators are merged into the component's props
)(Calendar as any);
