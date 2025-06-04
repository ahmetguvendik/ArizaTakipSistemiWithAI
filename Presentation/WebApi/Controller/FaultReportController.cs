using Application.Features.Commands.FaultReportComamnds;
using Application.Features.Queries.FaultReportQueries;
using Application.Hubs;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;


namespace WebApi.Controller;

[Route("api/[controller]")]
[ApiController]
public class FaultReportController  : Microsoft.AspNetCore.Mvc.Controller
{
    private readonly IMediator _mediator;
    private readonly IHubContext<FaultHub> _faultHubContext;
    

    public FaultReportController(IMediator mediator,IHubContext<FaultHub> faultHubContext)
    {
         _mediator = mediator;
         _faultHubContext = faultHubContext;    
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var valus = await _mediator.Send(new GetFaultReportQuery());
        return Ok(valus);
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
        if (command.FaultFire == null || command.FaultFire.Length == 0)
            return BadRequest("Dosya yüklenmedi.");

        // Dosya yolunu belirle
        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
        Directory.CreateDirectory(uploadsFolder); // Klasör yoksa oluştur

        var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(command.FaultFire.FileName);
        var filePath = Path.Combine(uploadsFolder, uniqueFileName); 

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await command.FaultFire.CopyToAsync(stream); // Dosyayı yükle
        }

        // Dosya yolunu Command'a ekle
        command.FaultFirePath = "/uploads/" + uniqueFileName;

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

        return Ok("Kapatildi");
    }
    
    [HttpGet("GetFaultByMonth")]         
    public async Task<IActionResult> GetFaultByMonth()  
    {
        var valus = await _mediator.Send(new GetFaultByMonthQuery());
        return Ok(valus);
    }
    
}
