using MassTransit;
using Microsoft.Extensions.Logging;
using Proposta.Domain.Entities;
using Proposta.Domain.Enums;
using Proposta.Domain.Services.Interfaces;
namespace Proposta.Application.Services
{
    public class PropostaService : IPropostaService
    {
        private readonly IPropostaRepository _repository;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ILogger<PropostaService> _logger;
        private record PropostaAprovadaRecord(int Id);

        public PropostaService(IPropostaRepository repository, ILogger<PropostaService> logger, IPublishEndpoint publishEndpoint)
        {
            _repository = repository;
            _publishEndpoint = publishEndpoint;
            _logger = logger;
        }

        public async Task<PropostaEntity> GetPropostaById(int id)
        {
            _logger.LogInformation("Buscando propostas com id: {id}", id);
            var propostas = await _repository.GetByIdAsync(id);
            _logger.LogInformation("Propostas encontrada!");
            return propostas ?? throw new Exception($"Proposta com o id {id} não encontrada!");
        }

        public async Task<List<PropostaEntity>> GetProposta(int? status)
        {
            _logger.LogInformation("Buscando propostas com status: {Status}", status);
            var propostas = await _repository.GetAllAsync();
            _logger.LogInformation("Total de propostas encontradas: {Quantidade}", propostas.Count);
            return propostas;
        }

        public async Task AprovarProposta(int id)
        {
            _logger.LogInformation("Tentando aprovar proposta com ID: {Id}", id);
            var existente = await _repository.GetByIdAsync(id);

            if (existente == null)
            {
                _logger.LogWarning("Proposta com ID {Id} não encontrada", id);
                return;
            }

            if (existente.Status != (int)StatusPropostaEnum.Análise)
            {
                _logger.LogWarning("Proposta com ID {Id} não está em análise. Status atual: {Status}", id, existente.Status);
                return;
            }

            existente.Status = (int)StatusPropostaEnum.Aprovada;
            await _repository.UpdateAsync(existente);
            _logger.LogInformation("Proposta com ID {Id} aprovada com sucesso", id);

            await _publishEndpoint.Publish(new PropostaAprovadaRecord(existente.Id));
            _logger.LogInformation("Proposta com ID {Id} publicada na fila para contratação", id);
        }

        public async Task RejeitarProposta(int id)
        {
            _logger.LogInformation("Tentando rejeitar proposta com ID: {Id}", id);
            var existente = await _repository.GetByIdAsync(id);

            if (existente == null)
            {
                _logger.LogWarning("Proposta com ID {Id} não encontrada", id);
                return;
            }

            if (existente.Status != (int)StatusPropostaEnum.Análise)
            {
                _logger.LogWarning("Proposta com ID {Id} não está em análise. Status atual: {Status}", id, existente.Status);
                return;
            }

            existente.Status = (int)StatusPropostaEnum.Rejeitada;
            await _repository.UpdateAsync(existente);
            _logger.LogInformation("Proposta com ID {Id} rejeitada com sucesso", id);
        }

        public async Task<PropostaEntity> CriarProposta(PropostaEntity proposta)
        {
            _logger.LogInformation("Criando nova proposta para cliente: {Cliente}", proposta.Cpf);
            proposta.Status = (int)StatusPropostaEnum.Análise;
            var criada = await _repository.InsertAsync(proposta);
            _logger.LogInformation("Proposta criada com ID: {Id}", criada.Id);
            return criada;
        }

        public async Task ContratarProposta(int id)
        {
            _logger.LogInformation("Tentando contratar proposta com ID: {Id}", id);
            var existente = await _repository.GetByIdAsync(id);

            if (existente == null)
            {
                _logger.LogWarning("Proposta com ID {Id} não encontrada", id);
                return;
            }

            if (existente.Status != (int)StatusPropostaEnum.Aprovada)
            {
                _logger.LogWarning("Proposta com ID {Id} não está aprovada. Status atual: {Status}", id, existente.Status);
                return;
            }

            existente.Status = (int)StatusPropostaEnum.Contratada;
            existente.InicioVigencia = DateTime.Now;

            await _repository.UpdateAsync(existente);
            _logger.LogInformation("Proposta com ID {Id} contratada com início de vigência em {Data}", id, existente.InicioVigencia);
        }
    }
}
