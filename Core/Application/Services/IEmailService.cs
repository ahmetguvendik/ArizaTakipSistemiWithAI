namespace Application.Services;

public interface IEmailService
{
    public Task SendFaultEmailAsync(string emailAdress, string body);
    public Task SendSupervisorToTeknisyenEmailAsync(string emailAdress, string body);
    public Task SendClosedFaultEmailAsync(string emailAdress, string body); 

}