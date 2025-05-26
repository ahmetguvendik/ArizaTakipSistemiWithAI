using Application.Features.Queries.FaultReportQueries;
using Application.Features.Results.FaultReportResults;
using Application.Repositories;
using MediatR;

namespace Application.Features.Handlers.FaultReportHandlers.Read;

public class GetFaultByMonthQueryHandler  : IRequestHandler<GetFaultByMonthQuery, List<GetFaultByMonthQueryResult>>
{
    private readonly IFaultReportRepository _faultReportRepository;

    public GetFaultByMonthQueryHandler(IFaultReportRepository faultReportRepository)
    {
         _faultReportRepository = faultReportRepository;
    }
    
    public async Task<List<GetFaultByMonthQueryResult>> Handle(GetFaultByMonthQuery request, CancellationToken cancellationToken)
    {
        var counts = await _faultReportRepository.GetFaultByMonthAsync();

        // Ay isimlerini getir
        var months = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.MonthNames
            .Where(m => !string.IsNullOrEmpty(m))
            .ToArray();

        var results = new List<GetFaultByMonthQueryResult>();

        for (int i = 0; i < 12; i++)
        {
            results.Add(new GetFaultByMonthQueryResult
            {
                MonthName = months[i],
                Count = counts.Count > i ? counts[i] : 0
            });
        }

        return results; 
    }
}