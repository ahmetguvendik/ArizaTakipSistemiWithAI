using Application.Features.Commands.AppUserCommands;
using Application.Features.Commands.TenantCommands;
using Application.Features.Results.AppUserResults;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace WebApi.Controller;

[Route("api/[controller]")]
[ApiController]
public class LoginController : Microsoft.AspNetCore.Mvc.Controller
{
    private readonly IMediator _mediator;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public LoginController(IMediator mediator, IHttpContextAccessor httpContextAccessor)
    {
        _mediator = mediator;
        _httpContextAccessor = httpContextAccessor;
    }
    
    [HttpPost]
    public async Task<ActionResult<LoginUserQueryResult>> Login([FromBody] LoginUserCommand command)
    {
        if (command == null)
        {
            return BadRequest("Invalid request");           
        }

        // Mediator ile LoginUserCommand gönderiliyor
        var result = await _mediator.Send(command);

        if (result == null)
        {
            return Unauthorized("Invalid credentials");
        }

        // Kullanıcı başarılı giriş yaptıysa döndürülüyor
        return Ok(result);
    }
    
    [HttpPost("SetTenantConnectionString")] 
    public IActionResult SetTenantConnectionString([FromBody] LoginTenantCommand request)
    {
        if (request == null || string.IsNullOrEmpty(request.ConnectionString))
        {
            Log.Warning("SetTenantConnectionString: Boş veya null ConnectionString alındı.");
            return BadRequest("Bağlantı dizesi gönderilmedi.");
        }

        // ConnectionString'i Backend'in Session'ına kaydet
        _httpContextAccessor.HttpContext?.Session?.SetString("DynamicConnectionString", request.ConnectionString);
        Log.Information($"Backend Session'a DynamicConnectionString başarıyla kaydedildi.");

        return Ok(new { Message = "Bağlantı dizesi sunucu oturumuna kaydedildi." });
    }

    
    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand command)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        try
        {
            await _mediator.Send(command);
            return Ok(new { Message = "Şifre başarıyla sıfırlandı." });
        }
        catch (System.Exception ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }
    
    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordCommand request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            await _mediator.Send(request);
            // Güvenlik için, kullanıcı yoksa bile başarılı mesaj döner
            return Ok(new { Message = "Şifre sıfırlama maili gönderildi (eğer kayıtlı bir kullanıcı varsa)." });    
        }
        catch (Exception ex)
        {
            // Hata varsa burada loglayabilir veya kullanıcıya özel mesaj dönebilirsin
            return BadRequest(new { Error = ex.Message });
        }
    }
}