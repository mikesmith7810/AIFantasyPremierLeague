using AIFantasyPremierLeague.API.Models;

namespace AIFantasyPremierLeague.API.Services;
public interface IFPLDataService
{
    Task LoadPlayersKnownDataAsync();

    Task LoadPlayersPerformanceDataAsync(string gameWeek);
}