import 'bootstrap/dist/css/bootstrap.css';

import * as React from 'react';
import * as ReactDOM from 'react-dom';
import { Provider } from 'react-redux';
import { ConnectedRouter } from 'connected-react-router';
import { createBrowserHistory } from 'history';
import configureStore from './store/configureStore';
import App from './App';
import registerServiceWorker from './registerServiceWorker';

import Hue from './components/Hue';
import Calendar from './components/Calendar';
import Counter from './components/Counter';
import Clock from './components/Clock';

import { ToastContainer, toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';

import './css/app.css';


// Create browser history to use in the Redux store
const baseUrl = document.getElementsByTagName('base')[0].getAttribute('href') as string;
const history = createBrowserHistory({ basename: baseUrl });

// Get the application-wide store instance, prepopulating with state from the server where available.
const store = configureStore(history);

ReactDOM.render(
    <Provider store={store}>
        <ConnectedRouter history={history}>
            <React.Fragment>
                <div className="row">
                    <div className="panel col-lg-4">
                        <Hue />
                    </div>
                    <div className="panel col-lg-4">
                        <Clock />
                    </div>
                    <div className="panel col-lg-4">
                        <Calendar />
                    </div>
                    <ToastContainer
                        position="top-center"
                        autoClose={3000}
                        hideProgressBar={false}
                        newestOnTop={false}
                        closeOnClick={false}
                        rtl={false}
                        draggable={false}
                        pauseOnHover={false}
                    />
                </div>
            </React.Fragment>
        </ConnectedRouter>
    </Provider>,
    document.getElementById('root'));

registerServiceWorker();
