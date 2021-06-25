using BattleCards.Data;
using BattleCards.Data.Models;
using BattleCards.Models.User;
using BattleCards.Services;
using MyWebServer.Controllers;
using MyWebServer.Http;
using System.Linq;

namespace BattleCards.Controllers
{
    public class UsersController:Controller
    {
        private readonly ApplicationDbContext data;
        private readonly IValidator validator;
        private readonly IHashPassword passwordHasher;

        public UsersController(ApplicationDbContext data, IValidator validator, IHashPassword passwordHasher)
        {
            this.data = data;
            this.validator = validator;
            this.passwordHasher = passwordHasher;
        }

        public HttpResponse Login()
        {
            return View();
        }

        [HttpPost]
        public HttpResponse Login(UserLoginModel model)
        {
            var hashedPassword = passwordHasher.Hash(model.Password);

            var userId = data.Users
                .Where(u => u.Username == model.Username && u.Password == hashedPassword)
                .Select(u => u.Id)
                .FirstOrDefault();

            if (string.IsNullOrEmpty(userId))
            {
                //return Error("Username or Password not correct.");
                return Redirect("/Users/Login");
            }

            this.SignIn(userId);

            return Redirect("/Cards/All");
        }

        public HttpResponse Register()
        {
            return View();
        }

        [HttpPost]
        public HttpResponse Register(UserRegistrationModel model)
        {
            var errors = validator.ValidateUserRegistration(model);

            if (this.data.Users.Any(u => u.Username == model.Username))
            {
                errors.Add($"User with '{model.Username}' username already exists.");
            }

            if (this.data.Users.Any(u => u.Email == model.Email))
            {
                errors.Add($"User with '{model.Email}' e-mail already exists.");
            }

            if (errors.Count > 0)
            {
                //return Error(errors);
                return Redirect("/Users/Register");
            }

            var newUser = new User
            {
                Username = model.Username,
                Password = passwordHasher.Hash(model.Password),
                Email = model.Email,
            };

            data.Users.Add(newUser);
            data.SaveChanges();

            return Redirect("/Users/Login");
        }

        [Authorize]
        public HttpResponse Logout()
        {
            this.SignOut();

            return Redirect("/");
        }
    }
}
