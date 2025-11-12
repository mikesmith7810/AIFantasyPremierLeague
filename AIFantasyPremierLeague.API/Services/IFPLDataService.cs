using AIFantasyPremierLeague.API.Models;

namespace AIFantasyPremierLeague.API.Services;
public interface IFPLDataService
{
    Task GetPlayersKnownDataAsync();
}