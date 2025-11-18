using AIFantasyPremierLeague.API.Models;

namespace AIFantasyPremierLeague.API.DataGatherers;

public interface IPlayerCalculator
{
    Task<double> Calculate(string PlayerId, int NumberOfWeeks);
}