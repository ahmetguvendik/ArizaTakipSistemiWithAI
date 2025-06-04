using Application.Features.Commands.FaultReportComamnds;
using Application.Features.Queries.FaultReportQueries;
using Application.Hubs;
using Application.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
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
