using Proposta.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proposta.Domain.Entities
{
    [Table("Propostas")]
    public class PropostaEntity
    {
        public int Id { get; set; }
        // Informações do Segurado
        public string? Nome { get; set; }
        public string? Cpf { get; set; }
        public string? Email { get; set; }
        public int Status {  get; set; }

        // Informações do Seguro
        public string? Tipo { get; set; }          // Ex: "Auto", "Vida", "Residencial"
        public decimal? ValorSegurado { get; set; }
        public decimal? Premio { get; set; }       // Valor a pagar
        public DateTime? InicioVigencia { get; set; }
        public int PrazoMeses { get; set; }

        // Coberturas simples
        public bool? Assistencia24h { get; set; }
        public decimal? Franquia { get; set; }
    }
}
