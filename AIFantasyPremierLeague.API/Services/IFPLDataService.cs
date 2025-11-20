namespace AIFantasyPremierLeague.API.Services;
public interface IFPLDataService
{
    Task LoadPlayersKnownDataAsync();

    Task LoadTeamsKnownDataAsync();

    Task LoadTeamHistoryData(int gameWeek);

    Task LoadTeamFixtureData(int gameWeek);

    Task LoadPlayersPerformanceDataAsync(int gameWeek);
}