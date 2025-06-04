using Application.Features.Commands.FaultReportComamnds;
using Application.Repositories;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Hosting;

namespace Application.Features.Handlers.FaultReportHandlers.Write;

public class CreateFaultReportCommandHandler : IRequestHandler<CreateFaultReportCommand>
{
    private readonly IRepository<FaultReport>  _faultReportRepository;
    private readonly IHostingEnvironment  _hostEnvironment;
    

    public CreateFaultReportCommandHandler(IRepository<FaultReport> faultReportRepository, IHostingEnvironment hostEnvironment)
    {
         _faultReportRepository = faultReportRepository;
         _hostEnvironment = hostEnvironment;
    }
    
    public async Task Handle(CreateFaultReportCommand request, CancellationToken cancellationToken)
    {

        string fileName = null;
        if (request.FaultFire != null && request.FaultFire.Length > 0)
        {
            var uploadsFolder = Path.Combine(_hostEnvironment.ContentRootPath, "wwwroot", "uploads");
            Directory.CreateDirectory(uploadsFolder);   
            fileName  = Guid.NewGuid().ToString() + Path.GetExtension(request.FaultFire.FileName);
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await request.FaultFire.CopyToAsync(stream);
            }
            
            
        }

        
        var  faultReport = new FaultReport();
        faultReport.Id = Guid.NewGuid().ToString();
        faultReport.Title = request.Title;
        faultReport.Description = request.Description;
        faultReport.Status = "Yeni";
        faultReport.CreatedAt = DateTime.Now;
        faultReport.ReporterName  = request.ReporterName;
        faultReport.ReporterEmail  = request.ReporterEmail;
        faultReport.ReporterPhone  = request.ReporterPhone;
        faultReport.FaultFirePath = fileName != null ? "/uploads/" + fileName : "Gorsel veya Belge Yok";   
        await _faultReportRepository.CreateAsync(faultReport);
        await _faultReportRepository.SaveChangesAsync();
       
    }
}