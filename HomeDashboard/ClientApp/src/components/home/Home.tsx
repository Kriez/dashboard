import * as React from 'react';
import { connect } from 'react-redux';
import Hue from './hue/Hue';
import Clock from './clock/Clock';
import Calendar from './calendar/Calendar';
import { createGlobalStyle } from 'styled-components'

import { ToastContainer, toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';

interface GlobalStyleProps {
    bodyBackgroundColor: string;
    containerMaxWidth: string;
}

const GlobalStyle = createGlobalStyle`
  body {
    background-color: ${(props: GlobalStyleProps) => (props.bodyBackgroundColor)};
  }

  .container {
    max-width: ${(props: GlobalStyleProps) => (props.containerMaxWidth)};
  }
`

class Home extends React.PureComponent {
    
    public render() {

        return <React.Fragment>
            <GlobalStyle bodyBackgroundColor='black' containerMaxWidth='100%'/>
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
    }
}

export default connect()(Home);