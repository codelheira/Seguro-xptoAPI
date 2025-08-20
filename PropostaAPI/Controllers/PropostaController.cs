using AutoMapper;
using MassTransit;
using MassTransit.Transports;
using Microsoft.AspNetCore.Mvc;
using Proposta.Application.DTOs.Proposta;
using Proposta.Domain.Entities;
using Proposta.Domain.Services.Interfaces;
using RabbitMQ.Client;
using System.Text;

namespace PropostaAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PropostaController : ControllerBase
    {
        private readonly IPropostaService _propostaService;
        private readonly IMapper _mapper;
        private readonly ILogger<PropostaController> _logger;

        public PropostaController(
            ILogger<PropostaController> logger,
            IPropostaService propostaService,
            IMapper mapper)
        {
            _logger = logger;
            _propostaService = propostaService;
            _mapper = mapper;
        }

        /// <summary>
        /// Criar uma nova proposta.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CriarProposta([FromBody] CriarPropostaDto dto)
        {
            try
            {
                _logger.LogInformation("Recebida solicitação para criar proposta.");

                var proposta = _mapper.Map<PropostaEntity>(dto);
                var criada = await _propostaService.CriarProposta(proposta);
                var resposta = _mapper.Map<PropostaResponseDto>(criada);

                _logger.LogInformation("Proposta criada com sucesso. ID: {Id}", resposta.Id);
                return CreatedAtAction(nameof(ObterPorId), new { id = resposta.Id }, resposta);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar proposta.");
                return StatusCode(500, "Erro interno ao criar proposta.");
            }
        }

        /// <summary>
        /// Buscar todas as propostas.
        /// </summary>
        /// <param name="statusId">Filtro por Status (opcional).</param>
        /// <returns></returns>
        [HttpGet()]
        public async Task<IActionResult> GetAll([FromQuery] int? statusId)
        {
            try
            {
                if (statusId == null)
                    _logger.LogInformation("Buscando todas as propostas");
                else
                    _logger.LogInformation("Buscando proposta com StatusID: {Id}", statusId);

                var proposta = await _propostaService.GetProposta(statusId);
                if (proposta == null)
                {
                    _logger.LogWarning("Proposta não encontrada.");
                    return NotFound();
                }

                var resposta = _mapper.Map<List<PropostaResponseDto>>(proposta);
                return Ok(resposta);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar proposta.");
                return StatusCode(500, "Erro interno ao buscar proposta.");
            }
        }

        /// <summary>
        /// Buscar proposta pelo Id
        /// </summary>
        /// <param name="id">ID da proposta</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(int id)
        {
            try
            {
                _logger.LogInformation("Buscando proposta com ID: {Id}", id);

                var proposta = await _propostaService.GetPropostaById(id);
                if (proposta == null)
                {
                    _logger.LogWarning("Proposta com ID {Id} não encontrada.", id);
                    return NotFound();
                }

                var resposta = _mapper.Map<PropostaResponseDto>(proposta);
                return Ok(resposta);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar proposta com ID {Id}.", id);
                return StatusCode(500, "Erro interno ao buscar proposta.");
            }
        }

        /// <summary>
        /// Aprovar proposta (necessasrio que esteja em analise "StatusId = 0")
        /// </summary>
        /// <param name="id">ID da proposta</param>
        /// <returns></returns>
        [HttpPatch("{id}/aprovar")]
        public async Task<IActionResult> Aprovar(int id)
        {
            try
            {
                _logger.LogInformation("Solicitação para aprovar proposta ID: {Id}", id);
                await _propostaService.AprovarProposta(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao aprovar proposta ID: {Id}", id);
                return StatusCode(500, "Erro interno ao aprovar proposta.");
            }
        }

        /// <summary>
        /// Rejeitar proposta (necessasrio que esteja em analise "StatusId = 0")
        /// </summary>
        /// <param name="id">ID da proposta</param>
        /// <returns></returns>
        [HttpPatch("{id}/rejeitar")]
        public async Task<IActionResult> Rejeitar(int id)
        {
            try
            {
                _logger.LogInformation("Solicitação para rejeitar proposta ID: {Id}", id);
                await _propostaService.RejeitarProposta(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao rejeitar proposta ID: {Id}", id);
                return StatusCode(500, "Erro interno ao rejeitar proposta.");
            }
        }

        /// <summary>
        /// Contratar proposta (necessasrio que esteja em aprovada "StatusId = 1")
        /// </summary>
        /// <param name="id">ID da proposta</param>
        /// <returns></returns>
        [HttpPatch("{id}/contratar")]
        public async Task<IActionResult> Contratar(int id)
        {
            try
            {
                _logger.LogInformation("Solicitação para contratar proposta ID: {Id}", id);
                await _propostaService.ContratarProposta(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao contratar proposta ID: {Id}", id);
                return StatusCode(500, "Erro interno ao contratar proposta.");
            }
        }
    }
}
