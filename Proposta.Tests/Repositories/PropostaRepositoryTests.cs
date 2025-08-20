using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Proposta.Domain.Entities;
using Proposta.Infrastructure.Data;
using Proposta.Infrastructure.Repositories;
using Proposta.Tests.Fixtures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Proposta.Tests.Repositories
{
    public class PropostaRepositoryTests : IClassFixture<DbTestFixture>
    {
        private readonly AppDbContext _context;
        private readonly PropostaRepository _repository;

        public PropostaRepositoryTests(DbTestFixture fixture)
        {
            _context = fixture.Context;
            var logger = new Mock<ILogger<PropostaRepository>>().Object;
            _repository = new PropostaRepository(_context, logger);
        }

        [Fact]
        public async Task InsertAsync_DeveSalvarProposta()
        {
            var proposta = new PropostaEntity { Nome = "Teste", Status = 0 };
            var result = await _repository.InsertAsync(proposta);

            result.Should().NotBeNull();
            result.Id.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task GetByIdAsync_DeveRetornarProposta_SeExistente()
        {
            var proposta = new PropostaEntity { Nome = "Teste", Status = 0 };
            await _repository.InsertAsync(proposta);

            var result = await _repository.GetByIdAsync(proposta.Id);

            result.Should().NotBeNull();
            result!.Nome.Should().Be("Teste");
        }

        [Fact]
        public async Task GetByStatusAsync_DeveRetornarFiltradas()
        {
            await _repository.InsertAsync(new PropostaEntity { Nome = "A", Status = 1 });
            await _repository.InsertAsync(new PropostaEntity { Nome = "B", Status = 2 });

            var result = await _repository.GetByStatusAsync(1);

            result.Should().HaveCount(1);
            result[0].Nome.Should().Be("A");
        }

        [Fact]
        public async Task UpdateAsync_DeveAtualizarProposta()
        {
            var proposta = await _repository.InsertAsync(new PropostaEntity { Nome = "Original", Status = 0 });

            proposta.Nome = "Atualizado";
            var result = await _repository.UpdateAsync(proposta);

            result!.Nome.Should().Be("Atualizado");
        }

        [Fact]
        public async Task DeleteAsync_DeveRemoverProposta()
        {
            var proposta = await _repository.InsertAsync(new PropostaEntity { Nome = "Excluir", Status = 0 });

            var result = await _repository.DeleteAsync(proposta.Id);

            result.Should().BeTrue();
            var check = await _repository.GetByIdAsync(proposta.Id);
            check.Should().BeNull();
        }
    }

}
