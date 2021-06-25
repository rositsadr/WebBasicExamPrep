namespace CarShop.Controllers
{
    using CarShop.Data;
    using CarShop.Data.Models;
    using CarShop.Models.Issues;
    using CarShop.Services;
    using MyWebServer.Controllers;
    using MyWebServer.Http;
    using System.Linq;
    using static Data.DataConstants;

    public class IssuesController : Controller
    {
        private readonly IUserService userService;
        private readonly CarShopDbContext data;

        public IssuesController(IUserService userService, CarShopDbContext data)
        {
            this.userService = userService;
            this.data = data;
        }

        [Authorize]
        public HttpResponse CarIssues(string carId)
        {
            if (!this.userService.IsMechanic(this.User.Id))
            {
                var userOwnsCar = this.data.Cars
                    .Any(c => c.Id == carId && c.OwnerId == this.User.Id);

                if (!userOwnsCar)
                {
                    return Error("You do not have access to this car.");
                }
            }

            var carWithIssues = this.data
                .Cars
                .Where(c => c.Id == carId)
                .Select(c => new CarIssuesViewModel
                {
                    Id = c.Id,
                    Model = c.Model,
                    Year = c.Year,
                    Issues = c.Issues.Select(i => new IssueListingViewModel
                    {
                        Id = i.Id,
                        Description = i.Description,
                        IsFixed = i.IsFixed
                    })
                })
                .FirstOrDefault();

            if (carWithIssues == null)
            {
                return Error($"Car with ID '{carId}' does not exist.");
            }

            return View(carWithIssues);
        }

        [Authorize]
        public HttpResponse Add(string carId)
        {
            if (!data.Cars.Any(c=>c.Id == carId))
            {
                return BadRequest();
            }

            return View(new string[] { carId });
        }

        [Authorize]
        [HttpPost]
        public HttpResponse Add(AddingIssueModel model)
        {
            if (model.Description.Length<IssueDescriptionMinLength)
            {
                return Error($"Description should be longer then {IssueDescriptionMinLength} symbols.");
            }

            var issue = new Issue
            {
                CarId = model.CarId,
                Description = model.Description,
                IsFixed = false,
            };

            data.Issues.Add(issue);
            data.SaveChanges();

            return Redirect($"/Issues/CarIssues?carId={model.CarId}");
        }

        public HttpResponse Fix(string issueId, string carId)
        {
            if (!this.userService.IsMechanic(this.User.Id))
            {
                return Unauthorized();
            }

            var carExists = data.Cars.Any(c => c.Id == carId);

            if (!carExists)
            {
                return BadRequest();
            }

            var issue = data.Issues
                .Where(i => i.Id == issueId && i.CarId == carId)
                .FirstOrDefault();

            if (issue == null)
            {
                return Error("There is no such issue for this car");
            }

            if(issue.IsFixed == false)
            {
                issue.IsFixed = true;

                data.SaveChanges();
            }

            return Redirect($"/Issues/CarIssues?carId={carId}");
        }

        public HttpResponse Delete(string issueId, string carId)
        {
            var carExists = data.Cars.Any(c => c.Id == carId);

            if(!carExists)
            {
                return BadRequest();
            }

            var issue = data.Issues
                .Where(i => i.Id == issueId && i.CarId == carId)
                .FirstOrDefault();

            if(issue==null)
            {
                return Error($"There is no such issue for this car.");
            }

            data.Issues.Remove(issue);
            data.SaveChanges();

            return Redirect($"/Issues/CarIssues?carId={carId}");
        }
    }
}
