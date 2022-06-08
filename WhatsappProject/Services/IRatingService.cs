using WhatsappServer.Models;

namespace WhatsappServer.Services
{
    public interface IRatingService
    {
        public void addItem(Rating rating);

        public List<Rating> search(string query);

        public void editItem(Rating rating, int ID, string UserName);


        public Rating? getRating(int ID);


        public List<Rating> getAllRatings();


        public void removeRating(int ID);

    }
}
