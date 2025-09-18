using System.Collections.Generic;
using System.Threading.Tasks;
using ApostasCompulsivas.Models;

namespace ApostasCompulsivas.Services
{
    /// <summary>
    /// Interface para operações de manipulação de arquivos
    /// </summary>
    public interface IFileService
    {
        Task SalvarHistoricoTxtAsync(List<Historico> historicos, string nomeArquivo = "historico.txt");
        Task<List<Historico>> CarregarHistoricoTxtAsync(string nomeArquivo = "historico.txt");
        Task SalvarUsuariosJsonAsync(List<Usuario> usuarios, string nomeArquivo = "usuarios.json");
        Task<List<Usuario>> CarregarUsuariosJsonAsync(string nomeArquivo = "usuarios.json");
        Task SalvarApostasJsonAsync(List<Aposta> apostas, string nomeArquivo = "apostas.json");
        Task<List<Aposta>> CarregarApostasJsonAsync(string nomeArquivo = "apostas.json");
        Task SalvarRelatorioCompletoAsync(List<Usuario> usuarios, List<Aposta> apostas, List<Historico> historicos, string nomeArquivo = "relatorio_completo.json");
        Task<bool> ArquivoExisteAsync(string nomeArquivo);
        Task<string[]> ListarArquivosAsync(string extensao = "*");
    }
}
