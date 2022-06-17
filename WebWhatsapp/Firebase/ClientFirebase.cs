using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using System.Collections.Generic;
using WebWhatsappApi.Models;
using WebWhatsappApi.Service;

namespace WebWhatsappApi.Firebase
{
    public class ClientFirebase
    {


        static Dictionary<string, string> fireBase = new Dictionary<string, string>();

        public static void addUserWithToken(String userId, String token)
        {
            fireBase.Add(token, userId);
        }


        public static void SendMessage(string userId, MessagePost messagePost, string contactName)
        {
            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile("private_key.json")
            });
            

            var registrationToken = fireBase.FirstOrDefault(x => x.Value == contactName).Key;


            var message = new Message()
            {

                Token = registrationToken,
                Notification = new Notification()
                {
                    Title = userId,
                    Body = messagePost.Content
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

