using AIFantasyPremierLeague.API.Models;

namespace AIFantasyPremierLeague.API.DataGatherers;

public interface IPerformanceCalculator
{
    Task<double> Calculate(string PlayerId, int NumberOfWeeks);
}