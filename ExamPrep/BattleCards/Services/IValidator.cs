using BattleCards.Models.User;
using System.Collections.Generic;

namespace BattleCards.Services
{
    public interface IValidator
    {
        ICollection<string> ValidateUserRegistration(UserRegistrationModel model);
    }
}
