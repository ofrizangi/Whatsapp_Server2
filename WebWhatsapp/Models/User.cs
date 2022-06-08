using System.ComponentModel.DataAnnotations;

namespace WebWhatsappApi
{
    public class User
    {
        [Key]
        [Required]
        [MaxLength(20)]
        public string UserName { get; set; } = "";


        [Required]
        [DataType(DataType.Password)]
        //[RegularExpression("^(?=.*?[A - Z])(?=.*?[a - z])(?=.*?[0 - 9]).{8,}$")]
        public string Password { get; set; }

        [MaxLength(20)]
        public string NickName { get; set; }

        public string Image { get; set; } = "https://media.istockphoto.com/vectors/user-icon-flat-style-isolated-on-white-background-vector-id1084418050?k=20&m=1084418050&s=612x612&w=0&h=pm3Ov7GL8rnKKqe98FEfoya6A6UK-z4Iv60LPbj38GE=";

        public IEnumerable<Contact> Contacts { get; set; }


    }
}
