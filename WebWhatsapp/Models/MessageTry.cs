using FirebaseAdmin.Messaging;

namespace WebWhatsappApi.Models
{
    public class MessageTry
    {

        public Dictionary<string, string> Data { get; set; }

        public string Token { get; set; }


        public Notification Notification { get; set; }
    }
}
