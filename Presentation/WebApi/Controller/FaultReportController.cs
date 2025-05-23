using Application.Features.Commands.FaultReportComamnds;
using Application.Features.Queries.FaultReportQueries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using WebApi.Hubs;

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
    
    [HttpPost]
    public async Task<IActionResult> Post(CreateFaultReportCommand command)
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

        return Ok("Kapatildi");
    }
}
