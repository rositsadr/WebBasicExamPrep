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
        private readonly IValidator validator;

        public IssuesController(IUserService userService, CarShopDbContext data, IValidator validator)
        {
            this.userService = userService;
            this.data = data;
            this.validator = validator;
        }

        [Authorize]
        public HttpResponse CarIssues(string carId)
        {
            if (!this.userService.IsMechanic(this.User.Id) && !this.userService.OwnsCar(this.User.Id,carId))
            {
                return Unauthorized();
            }

            var carWithIssues = this.data
                .Cars
                .Where(c => c.Id == carId)
                .Select(c => new CarIssuesViewModel
                {
                    Id = c.Id,
                    Model = c.Model,
                    Year = c.Year,
                    IsMechanic = this.userService.IsMechanic(this.User.Id),
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
            if (!this.userService.IsMechanic(this.User.Id) && !this.userService.OwnsCar(this.User.Id, model.CarId))
            {
                return Unauthorized();
            }

            var errors = validator.ValidateIssue(model);

            if (errors.Any())
            {
                return Error(errors);
            }

            var issue = new Issue
            {
                CarId = model.CarId,
                Description = model.Description,
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

            var issue = data.Issues.Find(issueId);

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

            if (!this.userService.IsMechanic(this.User.Id) && !this.userService.OwnsCar(this.User.Id, carId))
            {
                return Unauthorized();
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
