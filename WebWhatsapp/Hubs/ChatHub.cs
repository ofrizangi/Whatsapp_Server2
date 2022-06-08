using WebWhatsappApi.Hubs.Clients;
using WebWhatsappApi.Models;

using Microsoft.AspNetCore.SignalR;
using System.Collections;

namespace WebWhatsappApi.Hubs

{
    public class ChatHub : Hub<IChatClient>
    {
        private static Hashtable htUsers_ConIds = new Hashtable(20);
        public void registerConId(string userID)
        {
            var key = "user: " + userID;
            if (htUsers_ConIds.ContainsKey(key))
                htUsers_ConIds[key] = Context.ConnectionId;
            else
                htUsers_ConIds.Add(key, Context.ConnectionId);
        }

        public async Task SendMessage(Transfer message)
        {
            var key = "user: " + message.To;
            var a = (string)htUsers_ConIds[key];
            if ((string)htUsers_ConIds[key] != null){
                await Clients.Client((string)htUsers_ConIds[key]).ReceiveMessage(message);

            }

            //await Clients.All.ReceiveMessage(message);
        }
    }
}



