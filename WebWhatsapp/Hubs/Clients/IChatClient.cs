using WebWhatsappApi.Models;

namespace WebWhatsappApi.Hubs.Clients
{
    public interface IChatClient
    {
        Task ReceiveMessage(Transfer message);
    }
}
