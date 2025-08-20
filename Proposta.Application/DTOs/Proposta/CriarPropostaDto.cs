using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proposta.Application.DTOs.Proposta
{
    public class CriarPropostaDto
    {
        public string? Nome { get; set; }
        public string? Cpf { get; set; }
        public string? Email { get; set; }

        // Informações do Seguro
        public string? Tipo { get; set; }          // Ex: "Auto", "Vida", "Residencial"
        public decimal? ValorSegurado { get; set; }
        public decimal? Premio { get; set; }       // Valor a pagar
        public int PrazoMeses { get; set; }

        // Coberturas simples
        public bool? Assistencia24h { get; set; }
        public decimal? Franquia { get; set; }
    }
}
