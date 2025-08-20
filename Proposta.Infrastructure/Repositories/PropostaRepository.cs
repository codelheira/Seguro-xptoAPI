using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Proposta.Domain.Entities;
using Proposta.Domain.Services.Interfaces;
using Proposta.Infrastructure.Data;

namespace Proposta.Infrastructure.Repositories
{
    public class PropostaRepository : IPropostaRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<PropostaRepository> _logger;

        public PropostaRepository(AppDbContext context, ILogger<PropostaRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<PropostaEntity>> GetAllAsync()
        {
            try
            {
                return await _context.Set<PropostaEntity>().ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar todas as propostas.");
                return new List<PropostaEntity>();
            }
        }

        public async Task<PropostaEntity?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Set<PropostaEntity>().FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar proposta com ID {Id}.", id);
                return null;
            }
        }

        public async Task<List<PropostaEntity>> GetByStatusAsync(int? status)
        {
            try
            {
                return await _context.Set<PropostaEntity>()
                    .Where(p => p.Status == status)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar propostas com status {Status}.", status);
                return new List<PropostaEntity>();
            }
        }

        public async Task<PropostaEntity> InsertAsync(PropostaEntity proposta)
        {
            try
            {
                await _context.Set<PropostaEntity>().AddAsync(proposta);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Proposta inserida com sucesso. ID: {Id}", proposta.Id);
                return proposta;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao inserir proposta.");
                throw;
            }
        }

        public async Task<PropostaEntity?> UpdateAsync(PropostaEntity proposta)
        {
            try
            {
                var existing = await _context.Set<PropostaEntity>().FindAsync(proposta.Id);
                if (existing == null)
                {
                    _logger.LogWarning("Proposta com ID {Id} não encontrada para atualização.", proposta.Id);
                    return null;
                }

                _context.Entry(existing).CurrentValues.SetValues(proposta);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Proposta atualizada com sucesso. ID: {Id}", proposta.Id);
                return existing;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar proposta com ID {Id}.", proposta.Id);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var proposta = await _context.Set<PropostaEntity>().FindAsync(id);
                if (proposta == null)
                {
                    _logger.LogWarning("Proposta com ID {Id} não encontrada para exclusão.", id);
                    return false;
                }

                _context.Set<PropostaEntity>().Remove(proposta);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Proposta excluída com sucesso. ID: {Id}", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao excluir proposta com ID {Id}.", id);
                return false;
            }
        }
    }
}
