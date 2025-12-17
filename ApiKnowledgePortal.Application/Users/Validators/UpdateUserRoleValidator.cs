using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiKnowledgePortal.Application.Users.Commands;
using FluentValidation;

namespace ApiKnowledgePortal.Application.Users.Validators
{
    public class UpdateUserRoleValidator : AbstractValidator<UpdateUserRoleCommand>
    {
        public UpdateUserRoleValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.NewRole).IsInEnum();
        }
    }
}