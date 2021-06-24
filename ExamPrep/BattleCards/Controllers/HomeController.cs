using MyWebServer.Controllers;
using MyWebServer.Http;

namespace BattleCards.Controllers
{

    public class HomeController : Controller
    { 
        public HttpResponse Index()
        {
            return this.View();
        }
    }
}