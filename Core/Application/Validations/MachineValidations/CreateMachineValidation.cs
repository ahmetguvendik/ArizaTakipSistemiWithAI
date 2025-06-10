using Application.Features.Commands.MachineCommands;
using FluentValidation;

namespace Application.Validations.MachineValidations;

public class CreateMachineValidation  : AbstractValidator<CreateMachineCommand>
{
    public CreateMachineValidation()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Isim Bos Birakilmaz");
        RuleFor(x=>x.SerialNumber).NotEmpty().WithMessage("Seri No Bos Birakilmaz");
        RuleFor(x=>x.DepartmentId).NotEmpty().WithMessage("Departman Bos Birakilmaz");
    }
}