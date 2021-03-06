using BattleCards.Models.Card;
using BattleCards.Models.User;
using System.Collections.Generic;

namespace BattleCards.Services
{
    public interface IValidator
    {
        ICollection<string> ValidateUserRegistration(UserRegistrationModel model);

        ICollection<string> ValidateCardAdding(AddCardModel model);
    }
}
