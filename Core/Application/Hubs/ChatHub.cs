using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Application.Hubs;

public class ChatHub : Hub
{
    // Bu metot istemci tarafından doğrudan çağrılacak.
    public string GetConnectionId()
    {
        Console.WriteLine($"Hub: GetConnectionId çağrıldı. ID: {Context.ConnectionId}");
        return Context.ConnectionId;
    }

    // OnConnectedAsync metodunu değiştirmeye gerek yok, GetConnectionId'yi doğrudan kullanıyoruz.
    // Ancak hata ayıklamak için burada bir log bırakabilirsiniz:
    public override async Task OnConnectedAsync()
    {
        Console.WriteLine($"Hub: İstemci bağlandı. ID: {Context.ConnectionId}");
        await base.OnConnectedAsync();
    }
}