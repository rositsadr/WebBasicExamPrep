using Git.Models;
using System.Collections.Generic;
using static Git.Data.DataConstants;

namespace Git.Services
{
    public class Validator : IValidator
    {
        public string ValidateDescriptionLength(CreateCommitModel model)
        {
           if (model.Description.Length<UserMinLength)
            {
                return $"Description is too short. Minimum {UserMinLength} symbol.";
            }

            return string.Empty;
        }

        public string ValidateRepositoryName(CreateRepositoryModel model)
        {
            if(model.Name.Length < RepositoryMinLength || model.Name.Length > RepositoryMaxLength)
            {
                return $"The repository name should be between {RepositoryMinLength} and {RepositoryMaxLength} symbols.";
            }

            return string.Empty;
        }

        public ICollection<string> ValidateUserRegistration(RegisterUserModel model)
        {
            var errors = new List<string>();

            if (model.Username.Length<UserMinLength || model.Username.Length>UserMaxLength)
            {
                errors.Add($"Username mast be between {UserMinLength} and {UserMaxLength} symbols.");
            }

            if(model.Password.Length<PasswordMinLebgth || model.Password.Length>UserMaxLength)
            {
                errors.Add($"Password mast be between {PasswordMinLebgth} and {UserMaxLength} symbols.");
            }

            return errors;
        }
    }
}
