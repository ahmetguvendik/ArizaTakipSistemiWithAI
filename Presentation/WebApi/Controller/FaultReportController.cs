using Application.Features.Commands.FaultReportComamnds;
using Application.Features.Queries.FaultReportQueries;
using Application.Hubs;
using Application.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using OfficeOpenXml;
using Serilog;


namespace WebApi.Controller;

[Route("api/[controller]")]
[ApiController]
public class FaultReportController  : Microsoft.AspNetCore.Mvc.Controller
{
    private readonly IMediator _mediator;
    private readonly IHubContext<FaultHub> _faultHubContext;
    private readonly IEmailService  _emailService;
    
    

    public FaultReportController(IMediator mediator,IHubContext<FaultHub> faultHubContext,IEmailService emailService)
    {
         _mediator = mediator;
         _faultHubContext = faultHubContext;    
         _emailService = emailService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var valus = await _mediator.Send(new GetFaultReportQuery());
        return Ok(valus);
    }

    [HttpGet("export-to-excel")]
    public async Task<IActionResult> ExportToExcel()
    {
        ExcelPackage.License.SetNonCommercialPersonal("Ahmet Guvendik");
        
        var reports = await _mediator.Send(new GetFaultReportQuery());

        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Arizalar");

        // Başlık satırı
        worksheet.Cells[1, 1].Value = "İsim";
        worksheet.Cells[1, 2].Value = "Email";
        worksheet.Cells[1, 3].Value = "Başlık";
        worksheet.Cells[1, 4].Value = "Açıklama";
        worksheet.Cells[1, 5].Value = "Tarih";
        worksheet.Cells[1, 6].Value = "Departman";
        worksheet.Cells[1, 7].Value = "Durum";

        int row = 2;
        foreach (var item in reports)
        {
            worksheet.Cells[row, 1].Value = item.ReporterName;
            worksheet.Cells[row, 2].Value = item.ReporterEmail;
            worksheet.Cells[row, 3].Value = item.Title;
            worksheet.Cells[row, 4].Value = item.Description;
            worksheet.Cells[row, 5].Value = item.CreatedAt.ToString("dd.MM.yyyy HH:mm");
            worksheet.Cells[row, 6].Value = item.DepartmanName;
            worksheet.Cells[row, 7].Value = item.Status;
            row++;
        }

        var fileBytes = package.GetAsByteArray();
        return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Arizalar.xlsx");
    }
    
    [HttpGet("export-to-excel-departmant/{departmanId}")]
    public async Task<IActionResult> ExportToExcelDepartmant(string departmanId)
    {
        ExcelPackage.License.SetNonCommercialPersonal("Ahmet Guvendik");
        var reports = await _mediator.Send(new GetFaultReportByDepartmanIdQuery(departmanId));

        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Arizalar");

        // Başlık satırı
        worksheet.Cells[1, 1].Value = "İsim";
        worksheet.Cells[1, 2].Value = "Email";
        worksheet.Cells[1, 3].Value = "Başlık";
        worksheet.Cells[1, 4].Value = "Açıklama";
        worksheet.Cells[1, 5].Value = "Tarih";
        worksheet.Cells[1, 6].Value = "Departman";
        worksheet.Cells[1, 7].Value = "Durum";

        int row = 2;
        foreach (var item in reports)
        {
            worksheet.Cells[row, 1].Value = item.ReporterName;
            worksheet.Cells[row, 2].Value = item.ReporterEmail;
            worksheet.Cells[row, 3].Value = item.Title;
            worksheet.Cells[row, 4].Value = item.Description;
            worksheet.Cells[row, 5].Value = item.CreatedAt.ToString("dd.MM.yyyy HH:mm");
            worksheet.Cells[row, 6].Value = item.DepartmanName;
            worksheet.Cells[row, 7].Value = item.Status;
            row++;
        }

        var fileBytes = package.GetAsByteArray();
        return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Arizalar.xlsx");
    }
    
    
    [HttpGet("{id}")]       
    public async Task<IActionResult> GetById(string id)
    {
        var valus = await _mediator.Send(new GetFaultReportByIdQuery(id));
        return Ok(valus);
    }
    
    [HttpGet("GetByDepartmanId/{id}")]         
    public async Task<IActionResult> GetByDepartmanId(string id)
    {
        var valus = await _mediator.Send(new GetFaultReportByDepartmanIdQuery(id));
        return Ok(valus);
    }
    
    [HttpGet("GetFaultByDepartman")]         
    public async Task<IActionResult> GetFaultByDepartman()
    {
        var valus = await _mediator.Send(new GetFaultByDepartmanQuery());   
        return Ok(valus);
    }
    
    
    [HttpPost]
    public async Task<IActionResult> Post([FromForm]CreateFaultReportCommand command)
    {
        
            await _mediator.Send(command);
            await _faultHubContext.Clients.All.SendAsync("ReceiveUpdate", "Yeni Ariza Geldi");  
             return Ok("Eklendi");
    }
    [HttpPut]
    public async Task<IActionResult> Post(AssignTechnicianCommand command)
    {
        await _mediator.Send(command);
        await _faultHubContext.Clients.All.SendAsync("ReceiveUpdate", "Arıza Atandi");  

        return Ok("Atandı");
    }
    
    [HttpPut("CloseFault")]
    public async Task<IActionResult> ClosedFault(CloseFaultCommand command)     
    {
        await _mediator.Send(command);
        await _faultHubContext.Clients.All.SendAsync("ReceiveUpdate", "Arıza Kapatildi");
    
        Log.Information("Ariza ID: "+command.Id +"Ariza Kapatan: "+command.ClosedById);
        return Ok("Kapatildi");
    }
    
    [HttpGet("GetFaultByMonth")]         
    public async Task<IActionResult> GetFaultByMonth()  
    {
        var valus = await _mediator.Send(new GetFaultByMonthQuery());
        return Ok(valus);
    }
    
}
