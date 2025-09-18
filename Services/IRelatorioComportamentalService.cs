using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApostasCompulsivas.Models;

namespace ApostasCompulsivas.Services
{
    /// <summary>
    /// Interface para serviço de relatórios comportamentais
    /// </summary>
    public interface IRelatorioComportamentalService
    {
        Task<RelatorioComportamental> GerarRelatorioAsync(int usuarioId, DateTime dataInicio, DateTime dataFim);
        Task<RelatorioComportamental> GerarRelatorioMensalAsync(int usuarioId, int mes, int ano);
        Task<RelatorioComportamental> GerarRelatorioSemanalAsync(int usuarioId, DateTime dataInicio);
        Task<List<RelatorioComportamental>> ListarRelatoriosAsync(int usuarioId);
        Task<RelatorioComportamental> ObterRelatorioPorIdAsync(int id);
        Task<string> GerarAnaliseComportamentalAsync(int usuarioId);
        Task<string> GerarRecomendacoesAsync(int usuarioId);
        Task<decimal> CalcularSimulacaoInvestimentoAsync(int usuarioId, decimal valor, int meses);
        Task<List<string>> GerarAlertasConscientizacaoAsync(int usuarioId);
        Task<RelatorioComportamental> SalvarRelatorioAsync(RelatorioComportamental relatorio);
    }
}
