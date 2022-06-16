using System.ComponentModel.DataAnnotations;

namespace WebWhatsappApi
{
    public class Messages
    {

        [Key]
        public int Id { get; set; }


        [Required]
        public string Content { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Time { get; set; }

        public Boolean Sent { get; set; }


        public Contact Contact { get; set; }
    }
}
