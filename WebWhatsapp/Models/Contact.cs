using System.ComponentModel.DataAnnotations;


namespace WebWhatsappApi
{
    public class Contact
    {

        [Key]
        public int Id { get; set; }


        public string ContactUserName { get; set; }


        public string ContactNickName { get; set; }

        public string Server { get; set; }

        public User User { get; set; }


        public IEnumerable<Messages> Messages { get; set; }


    }
}
