import * as React from 'react';
import { connect } from 'react-redux';
import { RouteComponentProps } from 'react-router';
import { Link } from 'react-router-dom';
import { ApplicationState } from '../store';
import * as HueStore from '../store/HueStore';

import { CircularProgressbar, CircularProgressbarWithChildren,  buildStyles } from 'react-circular-progressbar';
import 'react-circular-progressbar/dist/styles.css';


type HueProps =
    HueStore.HueState // ... state we've requested from the Redux store
    & typeof HueStore.actionCreators // ... plus action creators we've requested
    & RouteComponentProps<{ lastUpdated: string }>; // ... plus incoming routing parameters

class Hue extends React.PureComponent<HueProps> {
    private timerID: NodeJS.Timeout | undefined;

    public componentDidMount() {
        this.timerID = setInterval(() => this.ensureDataFetched(), 5000);
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
        console.log("Derp");
        this.props.requestHue();
    }

    private renderHuesTable() {
        return (<div>
            {this.props.hues.map((hue: HueStore.HueSceneModel) =>
                <div className="card" id={hue.id} >
                    <div className="card-body">
                        <h5 className="card-title">{hue.name}</h5>
                        <div className="row">
                        {hue.lights.map((light: HueStore.HueLightModel) =>
                            <div col-lg-4>
                                <div style={{ width: 80, height: 80 }}>
                                    <CircularProgressbarWithChildren styles={buildStyles({ backgroundColor: "black", trailColor: "transparent", pathColor: '#' + light.color })} value={200} minValue={0} maxValue={254} >
                                        <img style={{ width: 50, marginTop: -5 }} src="https://localhost:44372/dinner.png" alt="doge" />
                                    </CircularProgressbarWithChildren>
                                   
                                    </div>
                            </div>
                            )}
                            </div>
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
