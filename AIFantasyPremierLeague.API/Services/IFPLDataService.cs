namespace AIFantasyPremierLeague.API.Services;
public interface IFPLDataService
{
    Task LoadPlayersKnownDataAsync();

    Task LoadTeamsKnownDataAsync();

    Task LoadPlayersPerformanceDataAsync(int gameWeek);
}