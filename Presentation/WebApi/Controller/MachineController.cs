using Application.Features.Commands.MachineCommands;
using Application.Features.Queries.FaultReportQueries;
using Application.Features.Queries.MachineQueries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;

namespace WebApi.Controller;

[Route("api/[controller]")]
[ApiController]
public class MachineController : Microsoft.AspNetCore.Mvc.Controller
{
    private readonly IMediator _mediator;

    public MachineController(IMediator mediator)
    {
         _mediator = mediator;
    }
    
    [HttpGet("GetMachineByDepartmanId/{id}")]         
    public async Task<IActionResult> GetMachineByDepartmanId(string id)
    {
        var valus = await _mediator.Send(new GetMachineByDepartmanIdQuery(id));
        return Ok(valus);
    }

    [HttpPost]
    [Authorize(Roles = "Teknisyen")]
    public async Task<IActionResult> CreateMachine(CreateMachineCommand command)
    {
        await _mediator.Send(command);
        return Ok("Eklendi");
    }
    
    [HttpGet("export-to-excel-department/{departmanId}")]
    public async Task<IActionResult> ExportToExcelDepartment(string departmanId)    
    {
        ExcelPackage.License.SetNonCommercialPersonal("Ahmet Guvendik");
        
        var reports = await _mediator.Send(new GetMachineByDepartmanIdQuery(departmanId));

        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Makineler");

        // Başlık satırı
        worksheet.Cells[1, 1].Value = "Makine Adi";
        worksheet.Cells[1, 2].Value = "Makine Seri No";

        
        int row = 2;
        foreach (var item in reports)
        {
            worksheet.Cells[row, 1].Value = item.Name;
            worksheet.Cells[row, 2].Value = item.SeriNo;
            row++;
        }

        var fileBytes = package.GetAsByteArray();
        return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Makinalar.xlsx");
    }
}