using AutoMapper;
using Proposta.Application.DTOs.Proposta;
using Proposta.Domain.Entities;

namespace Proposta.API.Profiles
{
    public class PropostaProfile : Profile
    {
        public PropostaProfile()
        {
            CreateMap<CriarPropostaDto, PropostaEntity>();
            CreateMap<PropostaEntity, PropostaResponseDto>();
        }
    }   
}
