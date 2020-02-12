import * as React from 'react';
import { connect } from 'react-redux';
import { RouteComponentProps } from 'react-router';
import { Link } from 'react-router-dom';
import { ApplicationState } from '../../../store';
import * as HueStore from '../../../store/HueStore';

import { CircularProgressbar, CircularProgressbarWithChildren,  buildStyles } from 'react-circular-progressbar';
import 'react-circular-progressbar/dist/styles.css';

import './Hue.css';

type HueProps =
    HueStore.HueState // ... state we've requested from the Redux store
    & typeof HueStore.actionCreators // ... plus action creators we've requested
    & RouteComponentProps<{ lastUpdated: string }>; // ... plus incoming routing parameters

class Hue extends React.PureComponent<HueProps> {
    private timerID: NodeJS.Timeout | undefined;

    public componentDidMount() {
        this.timerID = global.setInterval(() => this.ensureDataFetched(), 5000);
        this.ensureDataFetched();
    }

    public render() {
        console.log(this.props);
        return (
            <React.Fragment>
                {this.renderHuesTable()}
                {this.renderPagination()}
            </React.Fragment>
        );
    }

    private ensureDataFetched() {
        this.props.requestHue();
    }

    private renderHuesTable() {
        return (<div className="hueComponent">
            {this.props.hues.map((hue: HueStore.HueSceneModel) =>
                <div className="card" id={hue.id} style={{ color: 'white' }} >
                    <div className="card-body">
                        <h5 className="card-title">{hue.name}</h5>
                        {hue.lights.map((light: HueStore.HueLightModel) =>
                            <div>
                                <p style={{ marginBottom: '0px', marginTop: '10px' }} className="text-left">{light.name}</p>
                                <div className="progress" style={{ height: '10px', backgroundColor: '#202020' }}>
                                    <div className={"progress-bar"} style={{ width: (light.brightness * 100 / 254) + '%', backgroundColor: "#" + light.color }} role="progressbar" aria-valuenow={light.brightness} aria-valuemin={0} aria-valuemax={254}></div>
                                </div>
                            </div>
                        )}
                    </div>
                </div>
            )}
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
    (state: ApplicationState) => state.hue, // Selects which state properties are merged into the component's props
    HueStore.actionCreators // Selects which action creators are merged into the component's props
)(Hue as any);
