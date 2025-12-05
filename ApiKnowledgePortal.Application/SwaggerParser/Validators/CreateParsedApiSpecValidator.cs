using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiKnowledgePortal.Application.SwaggerParser.Commands;
using FluentValidation;

namespace ApiKnowledgePortal.Application.SwaggerParser.Validators
{
    public class CreateParsedApiSpecValidator : AbstractValidator<CreateParsedApiSpecCommand>
    {
        public CreateParsedApiSpecValidator()
        {
            RuleFor(x => x.ApiSpecId).NotEmpty();
            RuleFor(x => x.Path).NotEmpty();
            RuleFor(x => x.Method).NotEmpty();
            RuleFor(x => x.OperationId).NotEmpty();
            RuleFor(x => x.Summary).MaximumLength(1000);
        }
    }
}