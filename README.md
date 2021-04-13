# MileStone1 Desktop Application: 

__Introduction:__

This project is a WPF applictaion which represents a flight simulation. 
With this application we can simulate a flight by using a designated simulator, detect anomalies and receive information regarding the flight and it's features.


__Application open screen:__
![openScreen](https://i.postimg.cc/W3s0FJ8f/open-window.jpg)






__Application features:__

•	Progress Bar – after loading the flight data, by using the progress bar we can skip to any time segment of the flight.
We can move the bar pointer backwards and forwards and see the flight data and simulator accordingly.

•	Playback Control -  by enabling playback control we can choose the desired play speed at different levels of speed.
Also, we can stop and resume the simulator.
The time passed until the current moment  of simulation is shown.

•	Joystick – represents the current state of the plane's steering.
The joystick is used to represent the current of the data of aileron and elevator as they are given.
Also, the throttle and rudder current state  are shown via slider bars according to the transmitted data.

•	More Flight Data – to achieve more knowledge regarding the flight we represent during the simulation and updated information regarding these following features:
1)	Altimeter 
2)	Airspeed
3)	Heading
4)	Pitch – represented via the left  gauge.
5)	Roll – represented via the middle  gauge.
6)	Yaw – represented via the right gauge.


•	Data Representation Using Graphs – to investigate a specific flight feature and its data out of all given features, we can simply choose it from a list box and see an updated graph regarding the chosen feature. 
In the graph section we can observe 3 graphs:
1)	The chosen feature's data (the upper right graph)
2)	The most correlative feature to the chosen feature (the upper left graph).
3)	The regression line graph which includes regression line, anomalies, and regression points. 

•	Anomalies Detection – when anomaly is occurred, we need to detect and report it.
We can detect anomalies by using different algorithm of detection anomalies, we can use those algorithms after the application is used. 
This application allows the user to choose different algorithms of detection anomalies: 
1)	Regression based.
2)	Inner circle based.

Also, we can choose an algorithm  and investigate the flight by using this chosen algorithm, afterward  we cand load a different algorithm and investigate the flight by using this second chosen  algorithm.
All the algorithms are implemented as a plug-in, meaning that all algorithms are DLL files which are loaded dynamically at the beginning of the simulation.

__Features Implementation:__
![UI picture](https://i.postimg.cc/LX76dcJB/app.jpg)



__Project Structure:__

![folders picture](https://i.postimg.cc/mrVKHn6P/files-windw.jpg)

1)	AnomalyDetectors –
responsible of the DLL loading and syncing to the project.
2)	Controls – 
responsible of the view of each component (graphs, gauges, info boxes, joystick).
Each logical part (such as the graphs part, data information , joystick) has a view model of its own.
This folder contains the XAMLs of each component as described above and it's code behind. 
3)	Dlls – 
contains the dlls of the project.
4)	Fonts - contains the fonts files.
5)	Model –
the main and only model of the project, implements the MVVM design pattern and its responsible of the business logic of the project.
Contains the logic parts of the application such as parsing the files given, transmitting information, save and use information that is being used at the view models.
6)	Tools – 
This folder contains helper classes such as Transmitter class which is responsible of transmitting data (used in the model) and such as mostCorrelativeFinder which is responsible of finding the most correlative features.
7)	ViewModel-
this folder contains the view models of each logical part as mentioned above. Each class connects the model to the view.  
8)	Windows –
contains the Dashboard window which includes each logical part.

__Installation requirements:__

•	Frameworks:
1)	Microsoft.NETCore.App
2)	Microsoft.WindowsDesktop.App.WPF

•	Packages:
1)	CircularGauge (1.0.0)
2)	MahApps.Metro (2.4.4)
3)	Microsoft.Toolkit.Uwp.UI.Controls (7.0.1)
4)	OxyPlot.WindowsForms (2.0.0)
5)	Oxyplot.Wpf (2.0.0)

•	Applications:
1)	FlightGear – version 2018.3.5

__Installation Instructions:__


1) Add playback_small.xml to data/protocol folder in FlightGear installation path.

2)	Update the FlightGear's settings: 

--generic=socket,in,10,127.0.0.1,5400,tcp,playback_small

--fdm=null

__Example:__
![fg picture](https://i.postimg.cc/rFXccB6T/fg.jpg)




3)	Open the application and upload an data file to investigate.

4)	Launch  our app!
