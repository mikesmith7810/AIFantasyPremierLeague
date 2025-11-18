namespace AIFantasyPremierLeague.API.Services;
public interface IFPLDataService
{
    Task LoadPlayersKnownDataAsync();

    Task LoadTeamsKnownDataAsync();

    Task LoadTeamFixtureHistoryData(int gameWeek);

    Task LoadPlayersPerformanceDataAsync(int gameWeek);
}