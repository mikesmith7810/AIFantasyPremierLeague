# Fantasy Premier League Predictor 🏆

[![.NET](https://img.shields.io/badge/.NET-8.0-blue.svg)](https://dotnet.microsoft.com/)
[![ML.NET](https://img.shields.io/badge/ML.NET-Machine%20Learning-orange.svg)](https://dotnet.microsoft.com/apps/machinelearning-ai/ml-dotnet)
[![MySQL](https://img.shields.io/badge/MySQL-Database-blue.svg)](https://www.mysql.com/)

## Overview

A machine learning-powered Fantasy Premier League prediction system built with ASP.NET Core 8.0 and ML.NET. This application analyzes historical player performance data to predict future Fantasy Premier League points, helping users make informed decisions for their fantasy teams.

### Key Features

- **Machine Learning Predictions**: Utilizes ML.NET framework with FastTree regression for accurate player performance forecasting
- **Historical Data Analysis**: Processes comprehensive player statistics including goals, assists, minutes played, and team performance metrics
- **Position-Based Filtering**: Separate predictions for Goalkeepers (GK), Defenders (DEF), Midfielders (MID), and Attackers (ATT)
- **RESTful API**: Clean, well-structured endpoints for data loading, model training, and predictions
- **Real-time Data Integration**: Fetches live data from the official Fantasy Premier League API

### Technology Stack

- **Backend**: ASP.NET Core 8.0
- **Machine Learning**: ML.NET
- **Database**: MySQL with Entity Framework Core
- **Containerization**: Docker & Docker Compose
- **Data Source**: Official Fantasy Premier League API

## Getting Started

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker](https://www.docker.com/get-started)
- [Docker Compose](https://docs.docker.com/compose/)

### Installation

1. **Clone the repository**

   ```bash
   git clone https://github.com/mikesmith7810/AIFantasyPremierLeague.git
   cd AIFantasyPremierLeague
   ```

2. **Start the MySQL database**

   ```bash
   docker-compose up -d
   ```

3. **Run the application**

   ```bash
   cd AIFantasyPremierLeague.API
   dotnet run
   ```

The API will be available at `http://localhost:5110`

## API Reference

### Data Loading Endpoints

Load foundational data required for predictions and analysis. **All data loading endpoints use POST methods.**

Note - these should be loaded in this order to allow for data dependancy.

| Endpoint | Method | Description | Response Time |
|----------|--------|-------------|---------------|
| `POST /fpldata/playersKnownData` | POST | Load basic player information (name, price, team, position) | ~30 seconds |
| `POST /fpldata/teamsKnownData` | POST | Load team data (name, short name, fixtures) | ~10 seconds |
| `POST /fpldata/teamFixtureData/{gameweek}` | POST | Load team fixture data and opposition strength | ~1 minute |
| `POST /fpldata/playersPerformanceData/{gameweek}` | POST | Load detailed player statistics for specific gameweek | ~2-3 minutes |
| `POST /fpldata/teamPerformanceData/{gameweek}` | POST | Load team performance history and points conceded data | ~1 minute |


### Machine Learning Endpoints

Train models and generate predictions for optimal fantasy team selection.

| Endpoint | Method | Description | Parameters | Response Time |
|----------|--------|-------------|------------|---------------|
| `POST /prediction/train` | POST | Train ML model using historical data | None | ~10 minutes |
| `GET /prediction/{playerid}/{gameweek}` | GET | Individual player prediction | `playerid`: Player ID (integer), `gameweek`: Target gameweek | ~1 second |
| `GET /prediction/all/{gameweek}/{position}` | GET | Position-based predictions | `gameweek`: Target gameweek, `position`: GK/DEF/MID/ATT | ~30 seconds |

### Data Retrieval Endpoints

Access stored player, performance, and team data with full CRUD operations.

#### Player Endpoints

| Endpoint | Method | Description | Parameters |
|----------|--------|-------------|------------|
| `GET /player` | GET | Retrieve all players with current season data | None |
| `GET /player/{id}` | GET | Get specific player by ID | `id`: Player ID (integer) |
| `POST /player` | POST | Add new player | Player object in request body |

#### Player Performance Endpoints

| Endpoint | Method | Description | Parameters |
|----------|--------|-------------|------------|
| `GET /playerPerformance` | GET | Get comprehensive player performance history | None |
| `GET /playerPerformance/{id}` | GET | Get specific player performance record | `id`: Performance ID (integer) |
| `POST /playerPerformance` | POST | Add new player performance record | PlayerPerformance object in request body |

#### Team Endpoints

| Endpoint | Method | Description | Parameters |
|----------|--------|-------------|------------|
| `GET /team` | GET | Access all team information and statistics | None |
| `GET /team/{id}` | GET | Get specific team by ID | `id`: Team ID (integer) |
| `POST /team` | POST | Add new team | Team object in request body |

## Data Flow

1. **Data Ingestion**: Historical player and team performance data is fetched from the FPL API
2. **Feature Engineering**: Player statistics are aggregated into meaningful features (5-game averages, home/away performance, etc.)
3. **Model Training**: ML.NET FastTree algorithm trains on processed features to predict fantasy points
4. **Prediction Generation**: Trained model generates point predictions for upcoming gameweeks
5. **Ranking**: Players are ranked by predicted points within their positions

## Model Features

The prediction model considers the following features:

- **Player Statistics**: Goals, assists, minutes played, bonus points, clean sheets
- **Defensive Metrics**: Goals conceded, yellow/red cards, saves (for goalkeepers)
- **Historical Performance**: 5-game rolling averages across all metrics
- **Opposition Strength**: Average points conceded by opposing teams
- **Home/Away Factor**: Venue-based performance adjustments
- **Position-Specific Weighting**: Tailored feature importance by player position

## Development Notes

This project serves as a learning exercise in C#/.NET ecosystem development, transitioning from Java background. The application demonstrates:

- Modern ASP.NET Core patterns and dependency injection
- Entity Framework Core with MySQL integration
- ML.NET machine learning workflows
- RESTful API design principles
- Docker containerization strategies

## Disclaimer

This application uses data from the official Fantasy Premier League API. If the API structure changes, functionality may be affected. The predictions are for entertainment purposes and should not be considered as guaranteed outcomes.
