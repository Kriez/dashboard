import { Action, Reducer } from 'redux';
import { AppThunkAction } from '.';

// -----------------
// STATE - This defines the type of data maintained in the Redux store.

export interface HueState {
    isLoading: boolean;
    lastUpdated?: string;
    hues: HueSceneModel[];
}

export interface HueResponse {
    lastUpdated: string,
    hues: HueSceneModel[]
}

export interface HueSceneModel {
    id: string;
    name: string;
    lights: HueLightModel[];
}

export interface HueLightModel {
    id: string;
    name: string;
    isReachable: boolean;
    isOn: boolean;
    brightness: number;
    color: string;
}

// -----------------
// ACTIONS - These are serializable (hence replayable) descriptions of state transitions.
// They do not themselves have any side-effects; they just describe something that is going to happen.

interface RequestHueAction {
    type: 'REQUEST_HUE';
    lastUpdated: string;
}

interface ReceiveHueAction {
    type: 'RECEIVE_HUE';
    lastUpdated: string;
    hues: HueSceneModel[];
}

// Declare a 'discriminated union' type. This guarantees that all references to 'type' properties contain one of the
// declared type strings (and not any other arbitrary string).
type KnownAction = RequestHueAction | ReceiveHueAction;

// ----------------
// ACTION CREATORS - These are functions exposed to UI components that will trigger a state transition.
// They don't directly mutate state, but they can have external side-effects (such as loading data).

export const actionCreators = {
    requestHue: (lastUpdated: string): AppThunkAction<KnownAction> => (dispatch, getState) => {
        // Only load data if it's something we don't already have (and are not already loading)
        const appState = getState();
        if (appState && appState.hue && lastUpdated !== appState.hue.lastUpdated) {
            fetch(`hue`)
                .then(response => response.json() as Promise<HueResponse>)
                .then(data => {
                    dispatch({ type: 'RECEIVE_HUE', lastUpdated: data.lastUpdated, hues: data.hues });
                });

            dispatch({ type: 'REQUEST_HUE', lastUpdated: lastUpdated });
        }
    }
};

// ----------------
// REDUCER - For a given state and action, returns the new state. To support time travel, this must not mutate the old state.

const unloadedState: HueState = { hues: [], isLoading: false };

export const reducer: Reducer<HueState> = (state: HueState | undefined, incomingAction: Action): HueState => {
    if (state === undefined) {
        return unloadedState;
    }

    const action = incomingAction as KnownAction;
    switch (action.type) {
        case 'REQUEST_HUE':
            return {
                lastUpdated: action.lastUpdated,
                hues: state.hues,
                isLoading: true
            };
        case 'RECEIVE_HUE':
            // Only accept the incoming data if it matches the most recent request. This ensures we correctly
            // handle out-of-order responses.
            console.log(action.lastUpdated);
            console.log(state.lastUpdated);
            console.log("--------");
            if (action.lastUpdated !== state.lastUpdated) {
                console.log("Updating huestore")
                return {
                    lastUpdated: action.lastUpdated,
                    hues: action.hues,
                    isLoading: false
                };
            }
            break;
    }
    console.log(state);
    return state;
};
