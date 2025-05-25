namespace Application.Repositories;

public interface IStatisticsRepository
{
    int GetTotalFaultCount();
    int GetNewFaultCount();
    int GetAssignedFaultCount();
    int GetClosedFaultCount();
    public double GetAverageAssignmentTimeInMinutes();
    public double GetAverageClosedTimeInMinutes();

}