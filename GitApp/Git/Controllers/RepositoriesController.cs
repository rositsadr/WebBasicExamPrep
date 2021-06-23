using Git.Data;
using Git.Data.Models;
using Git.Models;
using Git.Services;
using MyWebServer.Controllers;
using MyWebServer.Http;
using System;
using System.Linq;
using static Git.Data.DataConstants;

namespace Git.Controllers
{
   public class RepositoriesController : Controller
    {
        private readonly IValidator validator;
        private readonly ApplicationDbContext data;

        public RepositoriesController(IValidator validator, ApplicationDbContext data)
        {
            this.validator = validator;
            this.data = data;
        }

        public HttpResponse All()
        {
            var repositories = data
                .Repositories
                .Where(r => r.IsPublic)
                .Select(r => new RepositoryListingViewModel
                {
                    Id = r.Id,
                    Name = r.Name,
                    Owner = r.Owner.Username,
                    CreatedOn = r.CreatedOn.ToString("r"),
                    Commits = r.Commits.Count,
                })
                .ToList();

            return View(repositories);
        }

        [Authorize]
        public HttpResponse Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public HttpResponse Create(CreateRepositoryModel model)
        {
            var errorMessage = this.validator.ValidateRepositoryName(model);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                return Error(errorMessage);
            }

            var repository = new Repository
            {
                Name = model.Name,
                IsPublic = IsPublic(model.RepositoryType),
                OwnerId = User.Id,
                CreatedOn = DateTime.UtcNow,
            };

            data.Repositories.Add(repository);
            data.SaveChanges();

            return Redirect("/Repositories/All");
        }
        private static bool IsPublic(string type)
        {
            if (type == PublicRepository)
            {
                return true;
            }

            return false;
        }
    }      
}
