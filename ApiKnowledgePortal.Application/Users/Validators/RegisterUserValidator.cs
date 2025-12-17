using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiKnowledgePortal.Application.Users.Commands;
using FluentValidation;

namespace ApiKnowledgePortal.Application.Users.Validators
{
    public class RegisterUserValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.LastName).NotEmpty();
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
        }
    }
}
