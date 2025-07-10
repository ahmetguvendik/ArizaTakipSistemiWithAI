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
        try
        {
            var valus = await _mediator.Send(new GetFaultReportQuery());
            return Ok(valus);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error in Get FaultReport");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("export-to-excel")]
    public async Task<IActionResult> ExportToExcel()
    {
        try
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
            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Arizalar_{DateTime.Now}.xlsx");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error in ExportToExcel");
            return StatusCode(500, "Internal server error");
        }
    }
    
    [HttpGet("export-to-excel-departmant/{departmanId}")]
    public async Task<IActionResult> ExportToExcelDepartmant(string departmanId)
    {
        try
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
            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Arizalar_{DateTime.Now}.xlsx");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error in ExportToExcelDepartmant");
            return StatusCode(500, "Internal server error");
        }
    }
    
    
    [HttpGet("{id}")]       
    public async Task<IActionResult> GetById(string id)
    {
        try
        {
            var valus = await _mediator.Send(new GetFaultReportByIdQuery(id));
            return Ok(valus);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error in GetById");
            return StatusCode(500, "Internal server error");
        }
    }
    
    [HttpGet("GetByDepartmanId/{id}")]         
    public async Task<IActionResult> GetByDepartmanId(string id)
    {
        try
        {
            var valus = await _mediator.Send(new GetFaultReportByDepartmanIdQuery(id));
            return Ok(valus);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error in GetByDepartmanId");
            return StatusCode(500, "Internal server error");
        }
    }
    
    [HttpGet("GetFaultByDepartman")]         
    public async Task<IActionResult> GetFaultByDepartman()
    {
        try
        {
            var valus = await _mediator.Send(new GetFaultByDepartmanQuery());   
            return Ok(valus);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error in GetFaultByDepartman");
            return StatusCode(500, "Internal server error");
        }
    }
    
    
    [HttpPost]
    public async Task<IActionResult> Post([FromForm]CreateFaultReportCommand command)
    {
        try
        {
            await _mediator.Send(command);
            await _faultHubContext.Clients.All.SendAsync("ReceiveUpdate", "Yeni Ariza Geldi");  
             return Ok("Eklendi");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error in Post (CreateFaultReport)");
            return StatusCode(500, "Internal server error");
        }
    }
    
    [HttpPut]
    public async Task<IActionResult> Post(AssignTechnicianCommand command)
    {
        try
        {
            await _mediator.Send(command);
            await _faultHubContext.Clients.All.SendAsync("ReceiveUpdate", "Arıza Atandi");  
            return Ok("Atandı");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error in Post (AssignTechnician)");
            return StatusCode(500, "Internal server error");
        }
    }
    
    [HttpPut("CloseFault")]
    public async Task<IActionResult> ClosedFault(CloseFaultCommand command)     
    {
        try
        {
            await _mediator.Send(command);
            await _faultHubContext.Clients.All.SendAsync("ReceiveUpdate", "Arıza Kapatildi");
            return Ok("Kapatildi");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error in ClosedFault");
            return StatusCode(500, "Internal server error");
        }
    }
    
    [HttpGet("GetFaultByMonth")]         
    public async Task<IActionResult> GetFaultByMonth()  
    {
        try
        {
            var valus = await _mediator.Send(new GetFaultByMonthQuery());
            return Ok(valus);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error in GetFaultByMonth");
            return StatusCode(500, "Internal server error");
        }
    }
    
}
