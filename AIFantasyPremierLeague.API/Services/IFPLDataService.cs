namespace AIFantasyPremierLeague.API.Services;
public interface IFPLDataService
{
    Task LoadPlayersKnownDataAsync();

    Task LoadPlayersPerformanceDataAsync(int gameWeek);
}