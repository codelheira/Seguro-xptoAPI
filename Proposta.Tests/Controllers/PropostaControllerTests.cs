using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Proposta.Application.DTOs.Proposta;
using Proposta.Domain.Entities;
using Proposta.Domain.Services.Interfaces;
using PropostaAPI.Controllers;
using Xunit;

namespace Proposta.Tests.Controllers
{
    public class PropostaControllerTests
    {
        private readonly Mock<IPropostaService> _serviceMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly Mock<ILogger<PropostaController>> _loggerMock = new();

        private PropostaController CreateController() =>
            new(_loggerMock.Object, _serviceMock.Object, _mapperMock.Object);

        [Fact]
        public async Task CriarProposta_DeveRetornarCreated()
        {
            var dto = new CriarPropostaDto { Nome = "Cliente Teste" };
            var entity = new PropostaEntity { Id = 1, Nome = "Cliente Teste" };
            var response = new PropostaResponseDto { Id = 1 };

            _mapperMock.Setup(m => m.Map<PropostaEntity>(dto)).Returns(entity);
            _serviceMock.Setup(s => s.CriarProposta(entity)).ReturnsAsync(entity);
            _mapperMock.Setup(m => m.Map<PropostaResponseDto>(entity)).Returns(response);

            var controller = CreateController();
            var result = await controller.CriarProposta(dto);

            result.Should().BeOfType<CreatedAtActionResult>();
        }
        [Fact]
        public async Task ObterPorId_DeveRetornarOk_SeEncontrado()
        {
            var proposta = new PropostaEntity { Id = 1, Nome = "Cliente Teste" };
            var dto = new PropostaResponseDto { Id = 1 };

            var propostas = new List<PropostaEntity>
            {
                new PropostaEntity { Id = 1, Nome = "Teste" },
                new PropostaEntity { Id = 2, Nome = "Outro" }
            };

            _serviceMock
                .Setup(s => s.GetPropostaById(1))
                .ReturnsAsync(propostas.First());

            _mapperMock.Setup(m => m.Map<PropostaResponseDto>(proposta)).Returns(dto);

            var result = await CreateController().ObterPorId(1);

            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task ObterPorId_DeveRetornarNotFound_SeNulo()
        {
            var propostas = new List<PropostaEntity>
            {
                new PropostaEntity { Id = 1, Nome = "Teste" },
                new PropostaEntity { Id = 2, Nome = "Outro" }
            };

            _serviceMock
                .Setup(s => s.GetProposta(null))
                .ReturnsAsync(propostas);

            var result = await CreateController().ObterPorId(1);

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Aprovar_DeveRetornarNoContent()
        {
            var result = await CreateController().Aprovar(1);
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task Rejeitar_DeveRetornarNoContent()
        {
            var result = await CreateController().Rejeitar(1);
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task Contratar_DeveRetornarNoContent()
        {
            var result = await CreateController().Contratar(1);
            result.Should().BeOfType<NoContentResult>();
        }
    }
}
