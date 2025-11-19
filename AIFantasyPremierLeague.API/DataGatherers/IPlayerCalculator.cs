using AIFantasyPremierLeague.API.Models;

namespace AIFantasyPremierLeague.API.DataGatherers;

public interface IPlayerCalculator
{
    Task<double> Calculate(int PlayerId, int NumberOfWeeks);
    Task<double> CalculateForGameWeek(int PlayerId, int NumberOfWeeks, int GameWeek);
}