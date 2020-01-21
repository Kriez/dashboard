import { Action, Reducer } from 'redux';
import { AppThunkAction } from '.';

// -----------------
// STATE - This defines the type of data maintained in the Redux store.

export interface CalendarState {
    isLoading: boolean;
    lastUpdated?: string;
    calendars: CalendarModel[];
}

export interface CalendarResponse {
    lastUpdated: string,
    calendars: CalendarModel[]
}

export interface CalendarModel {
    title: string;
    start: string;
    end: string;
}

// -----------------
// ACTIONS - These are serializable (hence replayable) descriptions of state transitions.
// They do not themselves have any side-effects; they just describe something that is going to happen.

interface RequestCalendarsAction {
    type: 'REQUEST_CALENDAR';
}

interface ReceiveCalendarsAction {
    type: 'RECEIVE_CALENDAR';
    lastUpdated: string;
    calendars: CalendarModel[];
}

// Declare a 'discriminated union' type. This guarantees that all references to 'type' properties contain one of the
// declared type strings (and not any other arbitrary string).
type KnownAction = RequestCalendarsAction | ReceiveCalendarsAction;

// ----------------
// ACTION CREATORS - These are functions exposed to UI components that will trigger a state transition.
// They don't directly mutate state, but they can have external side-effects (such as loading data).

export const actionCreators = {

    requestCalendar: (): AppThunkAction<KnownAction> => (dispatch, getState) => {

        // Only load data if it's something we don't already have (and are not already loading)
        const appState = getState();

        if (appState && appState.calendar) {
            fetch(`calendar`)
                .then(response => response.json() as Promise<CalendarResponse>)
                .then(data => {
                    dispatch({ type: 'RECEIVE_CALENDAR', lastUpdated: data.lastUpdated, calendars: data.calendars });
                });

            dispatch({ type: 'REQUEST_CALENDAR'});
        }
    }
};

// ----------------
// REDUCER - For a given state and action, returns the new state. To support time travel, this must not mutate the old state.

const unloadedState: CalendarState = { calendars: [], isLoading: false };

export const reducer: Reducer<CalendarState> = (state: CalendarState | undefined, incomingAction: Action): CalendarState => {
    if (state === undefined) {
        return unloadedState;
    }

    const action = incomingAction as KnownAction;
    switch (action.type) {
        case 'REQUEST_CALENDAR':
            return {
                lastUpdated: state.lastUpdated,
                calendars: state.calendars,
                isLoading: true
            };
        case 'RECEIVE_CALENDAR':
            // Only accept the incoming data if it matches the most recent request. This ensures we correctly
            // handle out-of-order responses.
            if (action.lastUpdated !== state.lastUpdated) {
                return {
                    lastUpdated: action.lastUpdated,
                    calendars: action.calendars,
                    isLoading: false
                };
            }
            break;
    }

    return state;
};
