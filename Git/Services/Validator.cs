using Git.Models.Repositories;
using Git.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Git.Services
{


    using static Data.DataConstants;

    public class Validator : IValidator
    {
       
        public ICollection<string> ValidateUser(RegisterUserFormModel model)
        {
            var errors = new List<string>();

            if (model.Username.Length < UserMinUsername || model.Username.Length > DefaultMaxLength)
            {
                errors.Add($"Username '{model.Username}' is not valid. It must be between {UserMinUsername} and {DefaultMaxLength} characters long.");
            }

            if (!Regex.IsMatch(model.Email, UserEmailRegularExpression))
            {
                errors.Add($"Email {model.Email} is not a valid e-mail address.");
            }

            if (model.Password.Length < UserMinPassword || model.Password.Length > DefaultMaxLength)
            {
                errors.Add($"The provided password is not valid. It must be between {UserMinPassword} and {DefaultMaxLength} characters long.");
            }

            if (model.Password.Any(x => x == ' '))//da nqma prazno mqsto
            {
                errors.Add($"The provided password cannot be only whitespaces");
            }

            if (model.Password != model.ConfirmPassword)
            {
                errors.Add($"Password and its confirmation are different.");
            }

            return errors;
        }

        public ICollection<string> ValidateRepository(CreateRepositoryFormModel model)
        {
            var errors = new List<string>();

            if (model.Name.Length < RepositoryMinName || model.Name.Length > RepositoryMaxName)
            {
                errors.Add($"Repository '{model.Name}' is not valid. It must be between {RepositoryMaxName} and {RepositoryMaxName} characters long.");
            }

            if (model.RepositoryType != RepositoryPrivateType && model.RepositoryType !=RepositoryPublicType)
            {
                errors.Add($"Repository type can be either public or private.");
            }

            return errors;
        }


    }
}
