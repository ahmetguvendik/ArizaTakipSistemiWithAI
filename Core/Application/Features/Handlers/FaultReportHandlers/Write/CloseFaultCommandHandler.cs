using Application.Features.Commands.FaultReportComamnds;
using Application.Repositories;
using Application.Services;
using Domain.Entities;
using MediatR;

namespace Application.Features.Handlers.FaultReportHandlers.Write;

public class CloseFaultCommandHandler : IRequestHandler<CloseFaultCommand>
{
    private readonly IRepository<FaultReport> _faultReportRepository;
    private readonly IEmailService  _emailService;
    
    public CloseFaultCommandHandler(IRepository<FaultReport> faultReportRepository, IEmailService emailService)
    {
        _faultReportRepository = faultReportRepository;
        _emailService = emailService;
    }
    public async Task Handle(CloseFaultCommand request, CancellationToken cancellationToken)
    {
        var value = await _faultReportRepository.GetByIdAsync(request.Id);
        value.ClosedById = request.ClosedById;
        value.MachineId = request.MachineId;
        value.ClosedTime = DateTime.Now;
        value.ClosedDescription = request.FaultDescription;
        value.Status = "Kapandı";
        string body = $@"
<div style='font-family:Arial,sans-serif; font-size:15px; color:#333;'>
    <p>Sayın {value.ReporterName},</p>

    <p>
        Tarafımıza iletmiş olduğunuz <strong>arıza bildirimi</strong> başarıyla çözümlenmiş ve ilgili kayıt <strong>kapatılmıştır</strong>.
    </p>

    <p>
        <strong>Arıza Başlığı:</strong> {value.Title}
        <strong>Arıza Açıklamanız:</strong> {value.Description}
    </p>

    <p>
        Eğer aynı konuda tekrar bir sorun yaşarsanız veya başka bir konuda desteğe ihtiyacınız olursa, bizimle her zaman iletişime geçebilirsiniz.
    </p>

    <br/>
    <p>Teşekkür eder, sağlıklı ve sorunsuz günler dileriz.</p>
    <p style='color:#4a90e2; font-weight:bold;'>Solfix Destek Ekibi</p>
</div>";

        await _emailService.SendClosedFaultEmailAsync(value.ReporterEmail,body);
        await _faultReportRepository.UpdateAsync(value);
        await _faultReportRepository.SaveChangesAsync();
        
    }
}

