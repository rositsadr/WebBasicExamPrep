using Git.Data;
using Git.Data.Models;
using Git.Models;
using Git.Services;
using MyWebServer.Controllers;
using MyWebServer.Http;
using System.Linq;

namespace Git.Controllers
{
    public class UsersController : Controller
    {
        private readonly IValidator validator;
        private readonly ApplicationDbContext data;
        private readonly IPasswordHasher hasher;

        public UsersController(IValidator validator, ApplicationDbContext data, IPasswordHasher hasher)
        {
            this.validator = validator;
            this.data = data;
            this.hasher = hasher;
        }
        public HttpResponse Login()
        {
           return View();
        }

        [HttpPost]
        public HttpResponse Login(LoginUserModel model)
        {
            var userId = data.Users
                .Where(u => u.Username == model.Username && u.Password == hasher.HashedPassword(model.Password))
                .Select(u => u.Id)
                .FirstOrDefault();


            if (userId==null)
            {
                return Error("Username and password combination is not valid!");
            }

            this.SignIn(userId);

            return Redirect("/Repositories/All");
        }

        public HttpResponse Register()
        {
            return View();
        }

        [HttpPost]
        public HttpResponse Register(RegisterUserModel model)
        {
            var errors = this.validator.ValidateUserRegistration(model);

            if (data.Users.Any(u=>u.Username == model.Username))
            {
                errors.Add("Username alreay exists!");
            }

            if (errors.Any())
            {
                return Error(errors);
            }

            var user = new User
            {
                Username = model.Username,
                Password = this.hasher.HashedPassword(model.Password),
                Email = model.Email,
            };

            data.Users.Add(user);
            data.SaveChanges();

            return Redirect("/Users/Login");
        }

        [Authorize]
        public HttpResponse Logout()
        {
            this.SignOut();

            return Redirect("/Home/Index");
        }
    }
}
