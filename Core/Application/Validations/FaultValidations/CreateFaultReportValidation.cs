using Application.Features.Commands.FaultReportComamnds;
using FluentValidation;

namespace Application.Validations.FaultValidations;

public class CreateFaultReportValidation: AbstractValidator<CreateFaultReportCommand>
{
    public CreateFaultReportValidation()
    {
        RuleFor(x => x.ReporterName).NotEmpty().WithMessage("Isminizi Giriniz");
        RuleFor(x=>x.ReporterEmail).NotEmpty().WithMessage("Emailinizi Giriniz");
        RuleFor(x=>x.ReporterPhone).NotEmpty().WithMessage("Telefon Numaranizi Giriniz");
        RuleFor(x => x.Description).MaximumLength(100).WithMessage("En Fazla 50 Karakter Giriniz");  
    }
}