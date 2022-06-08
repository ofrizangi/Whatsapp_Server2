namespace WhatsappServer.Models
{
    public class RaitngAverage
    {

        public int Id { get; set; }


        public List<Rating> AllRatings { get; set; }

        public double AverageRating { get; set; }   
    }
}
