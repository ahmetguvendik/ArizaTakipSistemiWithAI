using Application.Features.Results.FaultReportResults;

namespace Application.Services;

public interface IHangfireService
{
    public Task SendDailyReportEmailAsync();
}