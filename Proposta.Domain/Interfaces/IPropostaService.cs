using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proposta.Domain.Entities;
namespace Proposta.Domain.Services.Interfaces
{
    public interface IPropostaService
    {
        public Task<PropostaEntity> GetPropostaById(int id);
        public Task<List<PropostaEntity>> GetProposta(int? status);
        public Task AprovarProposta(int id);
        public Task RejeitarProposta(int id);
        public Task<PropostaEntity> CriarProposta(PropostaEntity proposta);
        public Task ContratarProposta(int id);
    }
}
