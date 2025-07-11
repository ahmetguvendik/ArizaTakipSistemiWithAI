using Application.Features.Commands.AppUserCommands;
using Application.Features.Results.AppUserResults;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Microsoft.AspNetCore.Identity;
using Application.Services;
using Domain.Entities;

namespace WebApi.Controller;

[Route("api/[controller]")]
[ApiController]
public class LoginController : Microsoft.AspNetCore.Mvc.Controller
{
    private readonly IMediator _mediator;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ITokenHandler _tokenHandler;
    private readonly UserManager<AppUser> _userManager;

    public LoginController(IMediator mediator, IHttpContextAccessor httpContextAccessor, ITokenHandler tokenHandler, UserManager<AppUser> userManager)
    {
        _mediator = mediator;
        _httpContextAccessor = httpContextAccessor;
        _tokenHandler = tokenHandler;
        _userManager = userManager;
    }
    
    [HttpPost]
    public async Task<ActionResult<LoginUserQueryResult>> Login([FromBody] LoginUserCommand command)
    {
        if (command == null)
        {
            return BadRequest("Invalid request");           
        }

        var result = await _mediator.Send(command);

        if (result == null || string.IsNullOrEmpty(result.Role))
        {
            return Unauthorized("Invalid credentials");
        }

        return Ok(result);
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
    
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()   
    {
        await _mediator.Send(new LogoutUserCommand());
        return Ok(new { message = "Çıkış başarılı" });
    }

    [HttpPost("jwt-login")]
    public async Task<IActionResult> JwtLogin([FromBody] LoginUserCommand command)
    {
        if (command == null)
            return BadRequest("Invalid request");

        var result = await _mediator.Send(command);

        if (result == null || string.IsNullOrEmpty(result.Role))
            return Unauthorized("Invalid credentials");

        // Kullanıcıyı bul
        var user = await _userManager.FindByNameAsync(command.Username);
        if (user == null)
            return Unauthorized("User not found");

        // JWT token oluştur
        var token = _tokenHandler.CreateAccessToken(user, result.Role);

        return Ok(new
        {
            token = token.AccessToken,
            expiration = token.Expiration
        });
    }
}