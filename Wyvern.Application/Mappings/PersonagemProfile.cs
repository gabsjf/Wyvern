using AutoMapper;
using Wyvern.Application.DTOs.Personagem;
using Wyvern.Domain.Entities;

namespace Wyvern.Application.Mappings
{
    public class PersonagemProfile : Profile
    {
        public PersonagemProfile()
        {
            CreateMap<Personagem, PersonagemResponseDto>().ReverseMap();
            CreateMap<Personagem, PersonagemCreateDto>().ReverseMap();
            CreateMap<Personagem, PersonagemUpdateDto>().ReverseMap();

            CreateMap<PersonagemCombate, PersonagemCombateResponseDto>().ReverseMap();
            CreateMap<PersonagemCombate, PersonagemCombateUpdateDto>().ReverseMap();

            CreateMap<PersonagemPlayer, PersonagemPlayerResponseDto>().ReverseMap();
            CreateMap<PersonagemPlayer, PersonagemPlayerUpdateDto>().ReverseMap();

            CreateMap<PersonagemItem, PersonagemItemAddDto>().ReverseMap();
            CreateMap<PersonagemMagia, PersonagemMagiaAddDto>().ReverseMap();
            CreateMap<PersonagemPericia, PersonagemPericiaAddDto>().ReverseMap();
            CreateMap<PersonagemPericia, PersonagemPericiaUpdateDto>().ReverseMap();
        }
    }
}
