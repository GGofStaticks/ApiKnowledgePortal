using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiKnowledgePortal.Application.SwaggerParser.Dtos;
using ApiKnowledgePortal.Domain.ParsedApiSpecs;
using AutoMapper;

namespace ApiKnowledgePortal.Application.SwaggerParser.Mapping
{
    public class ParsedApiSpecProfile : Profile
    {
        public ParsedApiSpecProfile()
        {
            CreateMap<ParsedApiSpec, ParsedApiSpecDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ApiSpecId, opt => opt.MapFrom(src => src.ApiSpecId))
                .ForMember(dest => dest.Path, opt => opt.MapFrom(src => src.Path))
                .ForMember(dest => dest.Method, opt => opt.MapFrom(src => src.Method))
                .ForMember(dest => dest.OperationId, opt => opt.MapFrom(src => src.OperationId))
                .ForMember(dest => dest.Summary, opt => opt.MapFrom(src => src.Summary))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt));
        }
    }
}