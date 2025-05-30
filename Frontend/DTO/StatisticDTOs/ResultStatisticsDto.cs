namespace DTO.StatisticDTOs;

public class ResultStatisticsDto
{
    public int GetTotalFaultCount { get; set; }
    public int GetNewFaultCount { get; set; }
    public int GetAssignedFaultCount { get; set; }
    public int GetClosedFaultCount { get; set; }
    public double GetTimeFaultAssignedToTeknosyenCountTime { get; set; }
    public double GetAverageClosedTimeInMinutes { get; set; }   
    
}