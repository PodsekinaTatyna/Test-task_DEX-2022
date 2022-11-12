using FluentValidation;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Models;

namespace Services.Validators
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            var message = "Ошибка в поле {PropertyName}: значение {PropertyValue}";

            RuleFor(p => p.Id)
                .NotEmpty()
                .WithMessage(message);

            RuleFor(p => p.Name)
                .NotEmpty().WithMessage(message)
                .Must(s => s != null && s.All(c => Char.IsWhiteSpace(c) || Char.IsLetter(c))).WithMessage(message);
        }
    }
}
