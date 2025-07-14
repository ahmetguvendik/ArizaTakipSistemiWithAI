using Application.Features.Commands.FaultReportComamnds;
using Application.Repositories;
using Application.Services;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Hosting;

namespace Application.Features.Handlers.FaultReportHandlers.Write;

public class CreateFaultReportCommandHandler : IRequestHandler<CreateFaultReportCommand>
{
    private readonly IRepository<FaultReport>  _faultReportRepository;
    private readonly IWebHostEnvironment  _hostEnvironment;
    private readonly IEmailService _emailService;
    

    public CreateFaultReportCommandHandler(IRepository<FaultReport> faultReportRepository, IWebHostEnvironment hostEnvironment, IEmailService emailService)
    {
         _faultReportRepository = faultReportRepository;
         _hostEnvironment = hostEnvironment;
         _emailService = emailService;
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
        string body = $@"
<div style='font-family:Arial,sans-serif; font-size:15px; color:#333;'>
    <p>Sayın {request.ReporterName},</p>

    <p>
        Oluşturmuş olduğunuz <strong>arıza bildirimi</strong> sistemimize başarıyla kaydedilmiştir.
    </p>

    <p>
        Bildiriminiz ilgili supervizor tarafindan ilgilencek ve size haber verilecektir
    </p>

    <p>
        <strong>Arıza Başlığı:</strong> {request.Title}<br/>
        <strong>Oluşturma Tarihi:</strong> {request.CreatedAt:dd.MM.yyyy HH:mm}
    </p>

    <p>
        Teknik ekibimiz en kısa sürede müdahale sağlayacaktır. Süreçle ilgili herhangi bir sorunuz ya da eklemek istediğiniz bir bilgi olursa bizimle iletişime geçebilirsiniz.
    </p>

    <br/>
    <p>İlginiz için teşekkür eder, işlerinizde kolaylıklar dileriz.</p>
    <p style='color:#4a90e2; font-weight:bold;'>Solfix Destek Ekibi</p>
</div>";


        await _emailService.SendFaultEmailAsync(request.ReporterEmail,body);

        await _faultReportRepository.CreateAsync(faultReport);
        await _faultReportRepository.SaveChangesAsync();
    }

}