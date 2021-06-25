﻿namespace CarShop.Services
{
    using CarShop.Models.Cars;
    using CarShop.Models.Issues;
    using CarShop.Models.Users;
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    using static Data.DataConstants;

    public class Validator : IValidator
    {
        public ICollection<string> ValidateUser(RegisterUserFormModel model)
        {
            var errors = new List<string>();

            if (model.Username == null ||model.Username.Length < UserMinUsername || model.Username.Length > DefaultMaxLength)
            {
                errors.Add($"Username '{model.Username}' is not valid. It must be between {UserMinUsername} and {DefaultMaxLength} characters long.");
            }

            if (model.Email == null || !Regex.IsMatch(model.Email, UserEmailRegularExpression))
            {
                errors.Add($"Email {model.Email} is not a valid e-mail address.");
            }

            if (model.Password == null || model.Password.Length < UserMinPassword || model.Password.Length > DefaultMaxLength)
            {
                errors.Add($"The provided password is not valid. It must be between {UserMinPassword} and {DefaultMaxLength} characters long.");
            }

            if (model.Password != model.ConfirmPassword)
            {
                errors.Add($"Password and its confirmation are different.");
            }

            if (model.UserType != UserTypeMechanic && model.UserType != UserTypeClient)
            {
                errors.Add($"User should be either a '{UserTypeMechanic}' or '{UserTypeClient}'.");
            }

            return errors;
        }

        public ICollection<string> ValidateCar(AddCarFormModel model)
        {
            var errors = new List<string>();

            if (model.Model == null || model.Model.Length < CarModelMinLength || model.Model.Length > DefaultMaxLength)
            {
                errors.Add($"Model '{model.Model}' is not valid. It must be between {CarModelMinLength} and {DefaultMaxLength} characters long.");
            }

            if (model.Year < CarYearMinValue || model.Year > CarYearMaxValue)
            {
                errors.Add($"Year '{model.Year}' is not valid. It must be between {CarYearMinValue} and {CarYearMaxValue}.");
            }

            if (model.Image == null || !Uri.IsWellFormedUriString(model.Image, UriKind.Absolute))
            {
                errors.Add($"Image {model.Image} is not a valid URL.");
            }

            if (model.PlateNumber == null || !Regex.IsMatch(model.PlateNumber, CarPlateNumberRegularExpression))
            {
                errors.Add($"Plate number {model.PlateNumber} is not valid. It should be in format 'AA0000AA'.");
            }

            return errors;
        }

        public ICollection<string> ValidateIssue(AddingIssueModel model)
        {
            var errors = new List<string>();

            if (model.Description == null || model.Description.Length < IssueDescriptionMinLength)
            {
                errors.Add($"Description should be longer then {IssueDescriptionMinLength} symbols.");
            }

            if (model.CarId == null)
            {
                errors.Add("Car name can not be empty");
            }

            return errors;
        }
    }
}
