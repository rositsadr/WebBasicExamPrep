using BattleCards.Data;
using BattleCards.Data.Models;
using BattleCards.Models.Card;
using BattleCards.Services;
using Microsoft.EntityFrameworkCore;
using MyWebServer.Controllers;
using MyWebServer.Http;
using System.Linq;

namespace BattleCards.Controllers
{
    public class CardsController:Controller
    {
        private readonly ApplicationDbContext data;
        private readonly IValidator validator;

        public CardsController(ApplicationDbContext data, IValidator validator)
        {
            this.data = data;
            this.validator = validator;
        }

        [Authorize]
        public HttpResponse All()
        {
            var cards = data.Cards
                .Select(c => new AllCardModel
                {
                    Id=c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    ImageUrl = c.ImageUrl,
                    Keyword = c.Keyword,
                    Attack = c.Attack,
                    Health = c.Health
                })
                .ToList();

            return View(cards);
        }

        [Authorize]
        public HttpResponse Add()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public HttpResponse Add(AddCardModel model)
        {
            var errors = validator.ValidateCardAdding(model);

            if(errors.Count>0)
            {
                //return Error(errors);
                return Redirect("/Cards/Add");
            }

            var card = new Card
            {
                Name = model.Name,
                ImageUrl = model.Image,
                Description = model.Description,
                Keyword = model.Keyword,
                Attack = model.Attack,
                Health = model.Health
            };

            data.Cards.Add(card);
            data.SaveChanges();

            return Redirect("/Cards/All");
        }

        [Authorize]
        public HttpResponse Collection()
        {
            var myCards = data.Cards
                .Where(c => c.CardUsers
                    .Select(cu => cu.UserId)
                    .Contains(User.Id))
                .ToList();

            return View(myCards);
        }

        [Authorize]
        public HttpResponse AddToCollection(string cardId)
        {
            var user = data.Users
                .Include(x=>x.UserCards)
                .Where(u => u.Id == this.User.Id)
                .FirstOrDefault();

            if(!user.UserCards.Any(c=>c.CardId == cardId))
            {
                var userCard = new UserCard
                {
                    UserId = this.User.Id,
                    CardId = cardId
                };

                data.UsersCards.Add(userCard);
                data.SaveChanges();
            }

            return Redirect("/Cards/All");
        }

        [Authorize]
        public HttpResponse RemoveFromCollection(string cardId)
        {
            var userCrad = data.UsersCards
                .Where(uc => uc.UserId == User.Id && uc.CardId == cardId)
                .FirstOrDefault();

            if(userCrad != null)
            {
                data.UsersCards.Remove(userCrad);
                data.SaveChanges();
            }

            return Redirect("/Cards/Collection");
        }
    }
}
