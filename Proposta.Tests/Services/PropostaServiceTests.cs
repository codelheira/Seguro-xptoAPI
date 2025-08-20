using FluentAssertions;
using MassTransit;
using Microsoft.Extensions.Logging;
using Moq;
using Proposta.Application.Services;
using Proposta.Domain.Entities;
using Proposta.Domain.Enums;
using Proposta.Domain.Services.Interfaces;
using Xunit;

namespace Proposta.Tests.Services
{
    public class PropostaServiceTests
    {
        private readonly Mock<IPropostaRepository> _repoMock = new();
        private readonly Mock<IPublishEndpoint> _publishMock = new();
        private readonly Mock<ILogger<PropostaService>> _loggerMock = new();

        private PropostaService CreateService() =>
            new(_repoMock.Object, _loggerMock.Object, _publishMock.Object);

        [Fact]
        public async Task AprovarProposta_DevePublicarEvento_SeStatusEmAnalise()
        {
            var proposta = new PropostaEntity { Id = 1, Status = (int)StatusPropostaEnum.Análise };
            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(proposta);

            var service = CreateService();
            await service.AprovarProposta(1);

            proposta.Status.Should().Be((int)StatusPropostaEnum.Aprovada);
            _repoMock
            .Setup(s => s.UpdateAsync(proposta))
            .ReturnsAsync(proposta);
        }

        [Fact]
        public async Task RejeitarProposta_DeveAtualizarStatus_SeValida()
        {
            var proposta = new PropostaEntity { Id = 1, Status = (int)StatusPropostaEnum.Análise };
            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(proposta);

            await CreateService().RejeitarProposta(1);

            proposta.Status.Should().Be((int)StatusPropostaEnum.Rejeitada);
            _repoMock.Verify(r => r.UpdateAsync(proposta), Times.Once);
        }

        [Fact]
        public async Task ContratarProposta_DeveAtualizarStatusEVigencia_SeAprovada()
        {
            var proposta = new PropostaEntity { Id = 1, Status = (int)StatusPropostaEnum.Aprovada };
            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(proposta);

            await CreateService().ContratarProposta(1);

            proposta.Status.Should().Be((int)StatusPropostaEnum.Contratada);
            proposta.InicioVigencia.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(5));
            _repoMock.Verify(r => r.UpdateAsync(proposta), Times.Once);
        }

        [Fact]
        public async Task GetProposta_DeveRetornarLista()
        {
            var propostas = new List<PropostaEntity> { new() { Id = 1 } };
            _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(propostas);

            var result = await CreateService().GetProposta(null);

            result.Should().BeEquivalentTo(propostas);
        }

    }
}
