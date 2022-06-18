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


        private static ClientFirebase _clientFirebase;

        public static ClientFirebase GetFirebase()
        {
            if(_clientFirebase == null)
            {
                _clientFirebase = new ClientFirebase();
            }
            return _clientFirebase;
        }


        public ClientFirebase()
        {
            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile("private_key.json")
            });
        }


        static Dictionary<string, string> fireBase = new Dictionary<string, string>();

        public static void addUserWithToken(String userId, String token)
        {
            if (fireBase.ContainsKey(token))
            {
                fireBase[token] = userId;
            }
            else
            {
                fireBase.Add(token, userId);
            }
        }


        public void SendMessage(string userId, MessagePost messagePost, string contactName)
        {
            var registrationToken = fireBase.FirstOrDefault(x => x.Value == contactName).Key;

            if(registrationToken != null)
            {
                var message = new Message()
                {
                    Data = new Dictionary<string, string>()
                {
                    { "userName", contactName },
                },
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


}

