# Fantasy Premier League Predictor

### Overview

I'm a Java developer so this is my first foray into using C#/.NET/ASP.NET as is meant as a project to further my skills in this area.

The application was seen as a bit of fun to try and help prevent me getting completely trounced by my 12 year old son in fantasy premier league. My team is quite woeful!

So - this application is designed to predict the players who will have the highest scores in the next game week. This uses the ML.NET framework to use machine learning based upon all the previous data from this season. (I might have to adapt it at some point to use previous seasons data).

Data is pulled from the API exposed by the Fantasy Premier League servers. If this data changes or the interface changes, there is a risk it may break this application.

The data is stored in a docker based mysql db.

For the prediction model, a scan is done of the data and an fplModel.zip file produced. This is then used by the ML.NET framework to create point predictions for the specified gameweek.

### Running

Start the service with 

cd AIFantasyPremierLeague.API
dotnet run

start the mysql docker instance with 
docker-compose up -d

### API Endpoints

I need to get some swagger docs or something running, but these are the main API endpoints. Be warned, some of them may take a few mins to load data in as there is alot of queries hitting the FPL API.

#### Data Loading


http://localhost:5110/fpldata/playersKnownData - Loads basic Player infomation - eg Name, Value, Team etc

http://localhost:5110/fpldata/teamsKnownData - Loads basic Team data such as name and shortName

http://localhost:5110/fpldata/playersPerformanceData/{gameweek} - Loads all the data for each player for the gameweek specified - this is for their stats for the week - eg goals scored, assits, yellow cards, mins played, etc.

http://localhost:5110/fpldata/teamPerformanceData/{gameweek} - Loads team fixtures and some bits not implemented yet.

#### Prediction

http://localhost:5110/prediction/train - Scans stored data and then produced model for future predictions (this can take up to 10 mins, but there is a progress indicator in the logs.)

http://localhost:5110/prediction/{playerid}/{gameweek} - Predicts the score for the playerid supplied in the gameweek supplied.

http://localhost:5110/prediction/all/{gameweek}/{GK/DEF/MID/ATT} - Predicts the scores for all players of the supplied position for gameweek supplied

#### Data

http://localhost:5110/player - Gets all Players

http://localhost:5110/playerPerformance - Gets all Player Performances

http://localhost:5110/team - Gets all Teams
