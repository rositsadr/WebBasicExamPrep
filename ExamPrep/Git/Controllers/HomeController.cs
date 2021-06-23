using MyWebServer.Controllers;
using MyWebServer.Http;

namespace Git.Controllers
{

    public class HomeController : Controller
    {
        [HttpGet]
        public HttpResponse Index()
        {
            if (this.User.IsAuthenticated)
            {
                return this.Redirect("/Repositories/All");
            }

            return this.View();
        }
    }
}
