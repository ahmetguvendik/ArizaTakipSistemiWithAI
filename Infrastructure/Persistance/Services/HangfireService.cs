using System.ComponentModel;
using Application.Services;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using OfficeOpenXml; // EPPlus
using System.IO;
using Application.Features.Results.FaultReportResults;
using Application.Repositories;

namespace Persistance.Services
{
    public class HangfireService : IHangfireService
    {
        private readonly IFaultReportRepository _faultReportRepository;

        public HangfireService(IFaultReportRepository faultReportRepository)
        {
             _faultReportRepository = faultReportRepository;
        }
        public async Task SendDailyReportEmailAsync()
        {
            // Excel paketini oluştur
            ExcelPackage.License.SetNonCommercialPersonal("Ahmet Guvendik");
            using var package = new ExcelPackage();
            
            var reports = await _faultReportRepository.GetAllAsync(); 
            var values =  reports.Select(x => new GetFaultReportResult
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                ReporterName = x.ReporterName,
                ReporterPhone = x.ReporterPhone,
                ReporterEmail = x.ReporterEmail,
                CreatedAt = x.CreatedAt,
                AssignedTime = x.AssignedTime,
                ClosedTime = x.ClosedTime,  
                Status = x.Status,
                AssignedByName = x.AssignedBy != null
                    ? x.AssignedBy.NameSurname
                    : "Atanmamış",
                ClosedDescription = x.ClosedDescription,    
                AssignedToName = x.AssignedTo != null
                    ? x.AssignedTo.NameSurname
                    : null,
                MachineName = x.Machine != null
                    ? x.Machine.Name    
                    : "Bilinmiyor",
                AssignedToId = x.AssignedTo?.Id ?? null,
                DepartmanName = x.AssignedTo?.Department?.Name ?? null,
                DepartmanId = x.AssignedTo?.Department?.Id ?? null,
                MachineId = x.Machine?.Id ?? null,
                ClosedByName = x.ClosedBy?.NameSurname?? null,
                ClosedById = x.ClosedBy?.Id ?? null,
                AssignedById = x.AssignedBy?.Id ?? null,
            }).ToList();
            
            var worksheet = package.Workbook.Worksheets.Add("Arizalar");

            worksheet.Cells[1, 1].Value = "İsim";
            worksheet.Cells[1, 2].Value = "Email";
            worksheet.Cells[1, 3].Value = "Başlık";
            worksheet.Cells[1, 4].Value = "Açıklama";
            worksheet.Cells[1, 5].Value = "Tarih";
            worksheet.Cells[1, 6].Value = "Departman";
            worksheet.Cells[1, 7].Value = "Durum";

            int row = 2;
            foreach (var item in values)
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

            // Excel dosyasını byte[] olarak al
            var fileBytes = package.GetAsByteArray();

            // Mail mesajını oluştur
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("ahmetguvendik011348@gmail.com"));
            email.To.Add(MailboxAddress.Parse("ahmetguvendik01@gmail.com"));
            email.Subject = "Günlük Arıza Raporları Hakkında";

            // Mail gövdesi ve ek dosya için multipart oluştur
            var builder = new BodyBuilder();
            builder.TextBody = "Günlük arıza raporu ekte gönderilmiştir.";

            // Excel dosyasını ekle
            builder.Attachments.Add($"Arizalar_{DateTime.Now}.xlsx", fileBytes, new ContentType("application", "vnd.openxmlformats-officedocument.spreadsheetml.sheet"));

            email.Body = builder.ToMessageBody();

            // SMTP ile gönder
            using var smtp = new SmtpClient();
            await smtp.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync("solfixhelpdesk@gmail.com", "zuwu jwvv ovka hxal");
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
        
    }


}
