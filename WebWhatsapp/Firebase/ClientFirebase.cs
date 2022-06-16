using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using System.Collections.Generic;
using WebWhatsappApi.Models;

namespace WebWhatsappApi.Firebase
{
    public class ClientFirebase
    {


        static Dictionary<string, string> fireBase = new Dictionary<string, string>();

        public static void addUserWithToken(String userId, String token)
        {
            fireBase.Add(userId, token);
        }
        

        //AddToDB(string userId, MessagePost message, string contactName)
        public static void SendMessage()
        {
            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile("private_key.json")
            });
            

            var registrationToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJCYXoiLCJqdGkiOiI3YzRhZGY4MS03MWZkLTRjOTgtYmY3OC01NWE2MDU3YTMzMTYiLCJpYXQiOiIxNi8wNi8yMDIyIDE0OjI5OjU1IiwiVXNlcklkIjoic2l2YW4iLCJleHAiOjE2NTUzOTI3OTUsImlzcyI6IkZvbyIsImF1ZCI6IkJhciJ9.GN_xc28uhwBU2dpqwnm-5Z_Bn92QMh7_o-B1TvYu5LE";

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

