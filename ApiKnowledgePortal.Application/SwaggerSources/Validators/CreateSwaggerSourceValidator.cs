using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiKnowledgePortal.Application.SwaggerSources.Commands;
using FluentValidation;

namespace ApiKnowledgePortal.Application.SwaggerSources.Validators
{
    public class CreateSwaggerSourceValidator : AbstractValidator<CreateSwaggerSourceCommand>
    {
        public CreateSwaggerSourceValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Url).NotEmpty();
        }
    }
}
