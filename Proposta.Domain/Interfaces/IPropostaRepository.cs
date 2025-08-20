using Proposta.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proposta.Domain.Services.Interfaces
{
    public interface IPropostaRepository
    {
        Task<List<PropostaEntity>> GetAllAsync();
        Task<PropostaEntity?> GetByIdAsync(int id);
        Task<List<PropostaEntity>> GetByStatusAsync(int? status);
        Task<PropostaEntity> InsertAsync(PropostaEntity proposta);
        Task<PropostaEntity?> UpdateAsync(PropostaEntity proposta);
        Task<bool> DeleteAsync(int id);

    }
}
