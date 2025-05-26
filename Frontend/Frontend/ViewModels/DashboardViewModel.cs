namespace Frontend.ViewModels;

public class DashboardViewModel
{
        public List<DTO.FaultReportDtos.GetFaultByMonthDto> FaultsByMonth { get; set; }
        public List<DTO.FaultReportDtos.GetFaultByDepartmentDto> FaultsByDepartment { get; set; }

}