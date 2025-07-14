using Application.Features.Commands.FaultReportComamnds;
using Application.Repositories;
using Application.Services;
using Domain.Entities;
using MediatR;

namespace Application.Features.Handlers.FaultReportHandlers.Write;

public class AssignTechnicianCommandHandler : IRequestHandler<AssignTechnicianCommand>
{
    private readonly IRepository<FaultReport>  _faultReportRepository;
    private readonly IEmailService _emailService;

    public AssignTechnicianCommandHandler(IRepository<FaultReport> faultReportRepository, IEmailService emailService)
    {
        _faultReportRepository = faultReportRepository;
        _emailService = emailService;
    }
    public async Task Handle(AssignTechnicianCommand request, CancellationToken cancellationToken)
    {
        var value = await _faultReportRepository.GetByIdAsync(request.Id);
        value.AssignedToId = request.AssignnedToId;
        value.Id = request.Id;
        value.Status = "Atandı";
        string body = $@"
<div style='font-family:Arial,sans-serif; font-size:15px; color:#333;'>
    <p>Sayın {value.ReporterName},</p>

    <p>
        Tarafımıza iletmiş olduğunuz <strong>arıza bildirimi</strong> sistemimize başarıyla kaydedilmiştir.
    </p>

    <p>
        Bildiriminiz, ilgili birim tarafından değerlendirilmiş ve çözüm süreci için teknik personelimize atanmıştır.
    </p>

    <p>
        <strong>Arıza Başlığı:</strong> {value.Title}<br/>
    </p>

    <p>
        Teknik ekibimiz en kısa sürede sizinle iletişime geçerek gerekli müdahaleyi gerçekleştirecektir.
        Süreçle ilgili herhangi bir sorunuz olursa bizimle iletişime geçebilirsiniz.
    </p>

    <br/>
    <p>İlginiz için teşekkür eder, sağlıklı ve sorunsuz günler dileriz.</p>
    <p style='color:#4a90e2; font-weight:bold;'>Solfix Destek Ekibi</p>
</div>";

        await _emailService.SendSupervisorToTeknisyenEmailAsync(value.ReporterEmail,body);
        value.AssignedById = request.AssignnedById;
        value.AssignedTime = DateTime.Now;  
        await _faultReportRepository.UpdateAsync(value);
        await _faultReportRepository.SaveChangesAsync();
    }
}