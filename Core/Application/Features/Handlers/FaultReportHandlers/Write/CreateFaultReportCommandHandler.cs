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
            var extension = Path.GetExtension(request.FaultFire.FileName).ToLowerInvariant();
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".pdf" };

            if (!allowedExtensions.Contains(extension))
            {
                throw new Exception("Sadece JPG ve PDF dosyalarına izin verilmektedir.");
            }

            if ((extension == ".jpg" || extension == ".jpeg") && request.FaultFire.Length > 5 * 1024 * 1024)
            {
                throw new Exception("JPG dosyaları en fazla 5MB olabilir.");
            }

            var uploadsFolder = Path.Combine(_hostEnvironment.ContentRootPath, "wwwroot", "uploads");
            Directory.CreateDirectory(uploadsFolder);
            fileName = Guid.NewGuid().ToString() + extension;
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await request.FaultFire.CopyToAsync(stream);
            }
        }

        var faultReport = new FaultReport
        {
            Id = Guid.NewGuid().ToString(),
            Title = request.Title,
            Description = request.Description,
            Status = "Yeni",
            CreatedAt = DateTime.Now,
            ReporterName = request.ReporterName,
            ReporterEmail = request.ReporterEmail,
            ReporterPhone = request.ReporterPhone,
            FaultFirePath = fileName != null ? "/uploads/" + fileName : "Gorsel veya Belge Yok"
        };

        await _faultReportRepository.CreateAsync(faultReport);
        await _faultReportRepository.SaveChangesAsync();
    }

}