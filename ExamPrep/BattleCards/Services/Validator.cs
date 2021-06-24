using BattleCards.Models.User;
using System.Collections.Generic;
using static BattleCards.Data.DatabaseConstants;

namespace BattleCards.Services
{
    public class Validator : IValidator
    {
        public ICollection<string> ValidateUserRegistration(UserRegistrationModel model)
        {
            var errors = new List<string>();

            if (model.Username.Length < UsernameMinLength || model.Username.Length > UsernameMaxLength)
            {
                errors.Add($"Username must be between {UsernameMinLength} and {UsernameMaxLength} symbols long.");
            }

            if (model.Password.Length < PasswordMinLength || model.Password.Length > PasswordMaxLength)
            {
                errors.Add($"Passwordmust be between {PasswordMinLength} and {PasswordMaxLength} symbols long.");
            }

            if(model.Password != model.ConfirmPassword)
            {
                errors.Add($"Password and confirmpassword are not the same.");
            }

            return errors;
        }
    }
}
