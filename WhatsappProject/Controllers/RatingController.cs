using Microsoft.AspNetCore.Mvc;
using WhatsappServer.Models;
using WhatsappServer.Services;

namespace WhatsappServer.Controllers
{
    public class RatingController : Controller
    {
        private IRatingService ratingService;


        public RatingController()
        {
            ratingService = new RatingService();
        }


        public IActionResult RatingList()
        {
            List<Rating> ratings = ratingService.getAllRatings();
            return View("RatingList", ratings);
        }
       
        [HttpPost]
        public async Task<IActionResult> RatingList(string query)
         {

            
            List<Rating>? ratings = ratingService.search(query);
          return View("RatingList", ratings);

        }


        public async Task<IActionResult> Search(string query)
        {

            List<Rating>? ratings = ratingService.search(query);
            return PartialView(ratings);

        }
        


        public IActionResult RatingItem(int ID)
        {
            Rating? rating = ratingService.getRating(ID);
            return View("RatingItem", rating);
        }

        public IActionResult EditRating(int ID)
        {
            Rating? rating = ratingService.getRating(ID);
            return View("EditRating", rating);
        }

        public IActionResult RemoveRating(int ID)
        { 
            //return Content($"Hello {rating.UserName}");
            ratingService.removeRating(ID);
            return Redirect("/Rating/RatingList");
        }


        // get the view of the list
        public IActionResult AddRating()
        {
            return View();
        }

        //  add a new rating - post method
        [HttpPost]
        public IActionResult AddRating(Rating rating)
        {
            ratingService.addItem(rating);
            return Redirect("ratinglist");

        }

        [HttpPost]
        public IActionResult EditRating(Rating rating, int ID, string UserName)
         {
            ratingService.editItem(rating, ID, UserName);
            return Redirect("/Rating/RatingList");
        }
    }
}
