﻿using FluentValidation;
using Models;

namespace Services.Validators
{
    public class AnnouncementValidator : AbstractValidator<Announcement>
    {
        public AnnouncementValidator()
        {
            var message = "Ошибка в поле {PropertyName}: значение {PropertyValue}";

            RuleFor(p => p.Id)
                .NotEmpty().WithMessage(message);

            RuleFor(p => p.Number)
                .NotEmpty().WithMessage(message)
                 .GreaterThan(0).WithMessage(message);

            RuleFor(p => p.UserId)
                .NotEmpty().WithMessage(message);

            RuleFor(p => p.Text)
                .NotEmpty().WithMessage(message);

            RuleFor(p => p.Rating)
                .NotEmpty().WithMessage(message)
                .GreaterThan(0).WithMessage(message)
                .LessThanOrEqualTo(10).WithMessage(message);

            RuleFor(p => p.Created)
                .NotEmpty().WithMessage(message)
                .LessThanOrEqualTo(DateTime.Now.Date).WithMessage(message);

            RuleFor(p => p.ExpirationDate)
                .NotEmpty().WithMessage(message);

        }
    }
}
