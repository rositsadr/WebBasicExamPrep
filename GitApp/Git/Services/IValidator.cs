using Git.Models;
using System.Collections.Generic;

namespace Git.Services
{
    public interface IValidator
    {
        ICollection<string> ValidateUserRegistration(RegisterUserModel model);

        string ValidateRepositoryName(CreateRepositoryModel model);

        string ValidateDescriptionLength(CreateCommitModel model);
    }
}
