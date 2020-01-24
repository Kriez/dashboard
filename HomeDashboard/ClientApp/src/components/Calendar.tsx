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
    private timerID: NodeJS.Timeout | undefined;

    public componentDidMount() {
        this.timerID = setInterval(() => this.ensureDataFetched(), 60000);
        this.ensureDataFetched();
    }

    public render() {
        return (
            <React.Fragment>
                {this.renderCalendarTable()}
                {this.renderPagination()}
            </React.Fragment>
        );
    }

    private ensureDataFetched() {
        this.props.requestCalendar();
    }

    private renderCalendarTable() {
        const startCalendarOptions = { weekday: 'long', month: 'long', day: 'numeric', hour: 'numeric', minute: 'numeric' };
        const endCalendarOptions = { hour: 'numeric', minute: 'numeric' };

        return (
            <div className="container">
                <div className="calendar dark">
                    <div className="calendar_header">
                        <h1 className="header_title">Calendar</h1>
                    </div>
                    <div className="calendar_events">
                        <p className="ce_title">Upcoming Events</p>


                        {this.props.calendars.map((calendarItem: CalendarStore.CalendarModel) =>
                            <div className="event_item">
                                <div className="ei_Dot" style={{ backgroundColor: "#" + calendarItem.color }}></div>
                                <div className="ei_Title">{new Intl.DateTimeFormat('sv', startCalendarOptions).format(new Date(calendarItem.start))} - {new Intl.DateTimeFormat('sv', endCalendarOptions).format(new Date(calendarItem.end))}</div>
                                <div className="ei_Copy">{calendarItem.title}</div>
                            </div>
                        )}
                        </div>
                    </div>

                </div>
        );
    }

    private renderPagination() {
        return (
            <div className="d-flex justify-content-between">
                {this.props.isLoading && <span>Loading...</span>}
            </div>
        );
    }
}

export default connect(
    (state: ApplicationState) => state.calendar, // Selects which state properties are merged into the component's props
    CalendarStore.actionCreators // Selects which action creators are merged into the component's props
)(Calendar as any);
