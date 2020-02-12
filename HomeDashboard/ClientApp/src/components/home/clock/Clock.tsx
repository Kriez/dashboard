import React, { Component } from 'react'; // let's also import Component
import './Clock.css';

type ClockState = {
	time: Date
}

class Clock extends Component<{}, ClockState>   {
	private timerId: NodeJS.Timeout | undefined;

	public componentDidMount() {
		this.setState({
			time: new Date()
		});

		this.timerId = global.setInterval(() => this.updateClock(), 1000);

	}

	public updateClock() {

		this.setState({
			time: new Date()
		});

		//var date = new Date(),
		//	hours = date.getHours(),
		//	minutes = date.getMinutes(),
		//	seconds = date.getSeconds();

		////make clock a 12 hour clock instead of 24 hour clock
		//hours = (hours > 12) ? (hours - 12) : hours;
		//hours = (hours === 0) ? 12 : hours;

		////invokes function to make sure number has at least two digits
		//hours = this.addZero(hours);
		//minutes = this.addZero(minutes);
		//seconds = this.addZero(seconds);

		////changes the html to match results
		//document.getElementsByClassName('hours')[0].innerHTML = hours;
		//document.getElementsByClassName('minutes')[0].innerHTML = minutes;
		//document.getElementsByClassName('seconds')[0].innerHTML = seconds;
	}

	private addZero(val: number) {
		return (val <= 9) ? ("0" + val) : val;
	}


	public render() {

		if (this.state === null) {
			return <div></div>
		}
			
		let hours = this.addZero(this.state.time.getHours());
		let minutes = this.addZero(this.state.time.getMinutes());
		let seconds = this.addZero(this.state.time.getSeconds());
		let currentDay = this.state.time.getDay();

		console.log(currentDay);
		return (
			<React.Fragment>
				<div className="clockComponent">
					<div className="days">

						<div className="day">
							<p className={(currentDay == 1 ? 'light-on' : 'light-off')}>mån</p>
						</div>

						<div className="day">
							<p className={(currentDay == 2 ? 'light-on' : 'light-off')}>tis</p>
						</div>

						<div className="day">
							<p className={(currentDay == 3 ? 'light-on' : 'light-off')}>ons</p>
						</div>

						<div className="day">
							<p className={(currentDay == 4 ? 'light-on' : 'light-off')}>tor</p>
						</div>

						<div className="day">
							<p className={(currentDay == 5 ? 'light-on' : 'light-off')}>fre</p>
						</div>

						<div className="day">
							<p className={(currentDay == 6 ? 'light-on' : 'light-off')}>lör</p>
						</div>

						<div className="day">
							<p className={(currentDay == 7 ? 'light-on' : 'light-off')}>sön</p>
						</div>

					</div>
					<div className="clock">
						<div className="numbers">
							<p className="hours">{hours}</p>
							<p className="placeholder">88</p>
						</div>

						<div className="colon">
							<p>:</p>
						</div>

						<div className="numbers">
							<p className="minutes">{minutes}</p>
							<p className="placeholder">88</p>
						</div>

						<div className="colon">
							<p>:</p>
						</div>

						<div className="numbers">
							<p className="seconds">{seconds}</p>
							<p className="placeholder">88</p>
						</div>
					</div>
				</div>
			</React.Fragment>
		);
	}
};

export default Clock;
