using Application.Features.Commands.TenantCommands;
using Application.Features.Results.TenantResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace WebApi.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class TenantController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TenantController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginTenant([FromBody] LoginTenantCommand command)
        {
            try
            {
                LoginTenantUserQueryResult result = await _mediator.Send(command);

                // Burada session set etmek istersen, session middleware'i eklemen ve konfigüre etmen lazım.
                // HttpContext.Session.SetString("TenantCS", result.ConnectionString);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = "Login failed",
                    error = ex.Message
                });
            }
        }
        
   
    }
}