using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using System.Collections.Generic;
using WebWhatsappApi.Models;

namespace WebWhatsappApi.Firebase
{
    public class ClientFirebase
    {

        public static void SendMessage()
        {
            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile("private_key.json")
            });



            var registrationToken = "f7vL1g_rzus:APA91bGkl1D5lCbDXKLzpPBa68uP5Bk4IUW6Kq67YDpfTWLnIytJ0NNG18ISEXHYlucBwG4AkGAnLiWJd_VLI_pOIpOpJZ0R4Elzjvijm5fmPXFRB_L4Y90bFfc16YILtfkYnW_TWISF";


            var message = new Message()
            {
                Data = new Dictionary<string, string>()
                {
                    { "myData", "1337" },
                },
                Token = registrationToken,
                Notification = new Notification()
                {
                    Title = "Test from code",
                    Body = "Here is your test!"
                }
            };

            // Send a message to the device corresponding to the provided
            // registration token.
            string response = FirebaseMessaging.DefaultInstance.SendAsync(message).Result;
            // Response is a message ID string.
            Console.WriteLine("Successfully sent message: " + response);
        }



    }


}

