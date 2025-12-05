using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiKnowledgePortal.Application.ApiSpec.Commands;
using FluentValidation;

namespace ApiKnowledgePortal.Application.ApiSpec.Validators
{
    public class CreateApiSpecValidator : AbstractValidator<CreateApiSpecCommand>
    {
        public CreateApiSpecValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Version).NotEmpty();
            RuleFor(x => x.Content).NotEmpty();
        }
    }
}