using MyWebServer.Controllers;
using MyWebServer.Http;

namespace BattleCards.Controllers
{

    public class HomeController : Controller
    { 
        public HttpResponse Index()
        {
            if(string.IsNullOrEmpty(User.Id))
            {
                return this.View();
            }

            return Redirect("/Cards/All");
        }
    }
}