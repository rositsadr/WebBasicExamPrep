using Git.Data;
using Git.Data.Models;
using Git.Models;
using Git.Services;
using Microsoft.EntityFrameworkCore;
using MyWebServer.Controllers;
using MyWebServer.Http;
using System.Linq;

namespace Git.Controllers
{
    public class CommitsController : Controller
    {
        private readonly ApplicationDbContext data;
        private readonly IValidator validator;

        public CommitsController(ApplicationDbContext data, IValidator validator)
        {
            this.data = data;
            this.validator = validator;
        }

        [Authorize]
        public HttpResponse All()
        {
            var commits = data.Commits
                .Where(c => c.CreatorId == this.User.Id)
                .Select(c => new CommitListingModel
                {
                    Id = c.Id,
                    Repository = c.Repository.Name,
                    Description = c.Description,
                    CreatedOn = c.CreatedOn.ToString("r"),
                })
                .ToList();

            return View(commits);
        }

        [Authorize]
        public HttpResponse Create(string id)
        {
            var repository = data.Repositories
                .Where(r => r.Id == id)
                .Select(r => new CommitToRepositorieViewModel
                {
                    Id = r.Id,
                    Name = r.Name
                })
                .FirstOrDefault();

            if (repository == null)
            {
                return BadRequest();
            }

            return View(repository);
        }

        [HttpPost]
        [Authorize]
        public HttpResponse Create(CreateCommitModel model)
        {
            if (!data.Repositories.Any(r=>r.Id == model.Id))
            {
                return BadRequest();
            }

            var error = validator.ValidateDescriptionLength(model);
            if (!string.IsNullOrEmpty(error))
            {
                return Error(error);
            }

            var commit = new Commit
            {
                Description = model.Description,
                RepositoryId=model.Id,
                CreatorId = this.User.Id
            };
            data.Commits.Add(commit);
            data.SaveChanges();

            return Redirect("/Repositories/All");
        }

        [Authorize]
        public HttpResponse Delete(string id)
        {
            var commitToDelete = data.Commits
            .Where(c => c.Id == id)
            .FirstOrDefault();

            if (commitToDelete==null || commitToDelete.CreatorId!=User.Id)
            {
                return BadRequest();
            }

            if (!UsersCommit(id,commitToDelete))
            {
                return Error("You are not the creator of the commit.You are not autorized to delete it.");
            }

            data.Commits.Remove(commitToDelete);
            data.SaveChanges();

            return Redirect("/Commits/All");
        }

        private bool UsersCommit(string id, Commit commit )
        {
            var user = data.Users
                .Include("Repositories")
                .Where(u => u.Id == this.User.Id)
                .FirstOrDefault();

            if (user.Repositories.Any(r=>r.Id==commit.RepositoryId))
            { 
                return true; 
            }

            return false;
        }
    }
}
