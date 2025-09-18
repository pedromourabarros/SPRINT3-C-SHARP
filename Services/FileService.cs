using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ApostasCompulsivas.Models;

namespace ApostasCompulsivas.Services
{
    /// <summary>
    /// Serviço responsável pela manipulação de arquivos .txt e .json
    /// </summary>
    public class FileService : IFileService
    {
        private readonly string _diretorioArquivos;

        public FileService()
        {
            _diretorioArquivos = Path.Combine(Directory.GetCurrentDirectory(), "Arquivos");
            if (!Directory.Exists(_diretorioArquivos))
            {
                Directory.CreateDirectory(_diretorioArquivos);
            }
        }

        public async Task SalvarHistoricoTxtAsync(List<Historico> historicos, string nomeArquivo = "historico.txt")
        {
            var caminhoArquivo = Path.Combine(_diretorioArquivos, nomeArquivo);
            var conteudo = new StringBuilder();

            conteudo.AppendLine("=== HISTÓRICO DE OPERAÇÕES ===");
            conteudo.AppendLine($"Data/Hora: {DateTime.Now:dd/MM/yyyy HH:mm:ss}");
            conteudo.AppendLine($"Total de registros: {historicos.Count}");
            conteudo.AppendLine();

            foreach (var historico in historicos.OrderBy(h => h.DataOperacao))
            {
                conteudo.AppendLine($"ID: {historico.Id}");
                conteudo.AppendLine($"Usuário: {historico.UsuarioId}");
                conteudo.AppendLine($"Operação: {historico.TipoOperacao}");
                conteudo.AppendLine($"Valor: R$ {historico.Valor:F2}");
                conteudo.AppendLine($"Descrição: {historico.Descricao}");
                conteudo.AppendLine($"Data: {historico.DataOperacao:dd/MM/yyyy HH:mm:ss}");
                conteudo.AppendLine($"Saldo: {historico.SaldoAnterior:F2} → {historico.SaldoPosterior:F2}");
                conteudo.AppendLine(new string('-', 50));
            }

            await File.WriteAllTextAsync(caminhoArquivo, conteudo.ToString(), Encoding.UTF8);
        }

        public async Task<List<Historico>> CarregarHistoricoTxtAsync(string nomeArquivo = "historico.txt")
        {
            var caminhoArquivo = Path.Combine(_diretorioArquivos, nomeArquivo);
            
            if (!File.Exists(caminhoArquivo))
                return new List<Historico>();

            var conteudo = await File.ReadAllTextAsync(caminhoArquivo, Encoding.UTF8);
            var historicos = new List<Historico>();

            // Parse simples do arquivo TXT (implementação básica)
            var linhas = conteudo.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            var i = 0;
            
            while (i < linhas.Length)
            {
                if (linhas[i].StartsWith("ID:"))
                {
                    try
                    {
                        var historico = new Historico
                        {
                            Id = int.Parse(linhas[i].Split(':')[1].Trim()),
                            UsuarioId = int.Parse(linhas[i + 1].Split(':')[1].Trim()),
                            TipoOperacao = linhas[i + 2].Split(':')[1].Trim(),
                            Valor = decimal.Parse(linhas[i + 3].Split('$')[1].Trim()),
                            Descricao = linhas[i + 4].Split(':')[1].Trim(),
                            DataOperacao = DateTime.Parse(linhas[i + 5].Split(':')[1].Trim()),
                            SaldoAnterior = decimal.Parse(linhas[i + 6].Split('→')[0].Split(':')[1].Trim()),
                            SaldoPosterior = decimal.Parse(linhas[i + 6].Split('→')[1].Trim())
                        };
                        historicos.Add(historico);
                    }
                    catch
                    {
                        // Ignorar linhas malformadas
                    }
                }
                i++;
            }

            return historicos;
        }

        public async Task SalvarUsuariosJsonAsync(List<Usuario> usuarios, string nomeArquivo = "usuarios.json")
        {
            var caminhoArquivo = Path.Combine(_diretorioArquivos, nomeArquivo);
            
            // Mascarar dados sensíveis para LGPD
            var usuariosMascarados = usuarios.Select(u => 
            {
                var usuarioCopy = new Usuario(u.Nome, u.Email, u.Saldo)
                {
                    Id = u.Id,
                    Telefone = u.Telefone,
                    DataCadastro = u.DataCadastro,
                    Ativo = u.Ativo,
                    NivelRisco = u.NivelRisco,
                    PontuacaoRisco = u.PontuacaoRisco,
                    UltimaAvaliacao = u.UltimaAvaliacao,
                    RecebeAlertas = u.RecebeAlertas,
                    AceitaApoio = u.AceitaApoio,
                    DataUltimaAposta = u.DataUltimaAposta,
                    TotalApostasHoje = u.TotalApostasHoje,
                    ValorApostadoHoje = u.ValorApostadoHoje,
                    DiasConsecutivosApostando = u.DiasConsecutivosApostando,
                    ConsentimentoAceito = u.ConsentimentoAceito,
                    DataConsentimento = u.DataConsentimento
                };
                
                // Aplicar mascaramento
                DataMaskingService.MascararDadosUsuario(usuarioCopy);
                return usuarioCopy;
            }).ToList();
            
            var json = JsonConvert.SerializeObject(usuariosMascarados, Formatting.Indented);
            await File.WriteAllTextAsync(caminhoArquivo, json, Encoding.UTF8);
        }

        public async Task<List<Usuario>> CarregarUsuariosJsonAsync(string nomeArquivo = "usuarios.json")
        {
            var caminhoArquivo = Path.Combine(_diretorioArquivos, nomeArquivo);
            
            if (!File.Exists(caminhoArquivo))
                return new List<Usuario>();

            var json = await File.ReadAllTextAsync(caminhoArquivo, Encoding.UTF8);
            return JsonConvert.DeserializeObject<List<Usuario>>(json) ?? new List<Usuario>();
        }

        public async Task SalvarApostasJsonAsync(List<Aposta> apostas, string nomeArquivo = "apostas.json")
        {
            var caminhoArquivo = Path.Combine(_diretorioArquivos, nomeArquivo);
            var json = JsonConvert.SerializeObject(apostas, Formatting.Indented);
            await File.WriteAllTextAsync(caminhoArquivo, json, Encoding.UTF8);
        }

        public async Task<List<Aposta>> CarregarApostasJsonAsync(string nomeArquivo = "apostas.json")
        {
            var caminhoArquivo = Path.Combine(_diretorioArquivos, nomeArquivo);
            
            if (!File.Exists(caminhoArquivo))
                return new List<Aposta>();

            var json = await File.ReadAllTextAsync(caminhoArquivo, Encoding.UTF8);
            return JsonConvert.DeserializeObject<List<Aposta>>(json) ?? new List<Aposta>();
        }

        public async Task SalvarRelatorioCompletoAsync(List<Usuario> usuarios, List<Aposta> apostas, List<Historico> historicos, string nomeArquivo = "relatorio_completo.json")
        {
            var caminhoArquivo = Path.Combine(_diretorioArquivos, nomeArquivo);
            
            var relatorio = new
            {
                DataGeracao = DateTime.Now,
                Resumo = new
                {
                    TotalUsuarios = usuarios.Count,
                    TotalApostas = apostas.Count,
                    TotalOperacoes = historicos.Count,
                    ValorTotalApostado = apostas.Sum(a => a.Valor),
                    ValorTotalGanho = apostas.Where(a => a.Status == "Ganhou" && a.ValorGanho.HasValue).Sum(a => a.ValorGanho!.Value)
                },
                Usuarios = usuarios,
                Apostas = apostas,
                Historico = historicos
            };

            var json = JsonConvert.SerializeObject(relatorio, Formatting.Indented);
            await File.WriteAllTextAsync(caminhoArquivo, json, Encoding.UTF8);
        }

        public async Task<bool> ArquivoExisteAsync(string nomeArquivo)
        {
            var caminhoArquivo = Path.Combine(_diretorioArquivos, nomeArquivo);
            return await Task.FromResult(File.Exists(caminhoArquivo));
        }

        public async Task<string[]> ListarArquivosAsync(string extensao = "*")
        {
            var padrao = extensao == "*" ? "*.*" : $"*.{extensao}";
            var arquivos = Directory.GetFiles(_diretorioArquivos, padrao)
                .Select(Path.GetFileName)
                .Where(f => f != null)
                .Cast<string>()
                .ToArray();
            
            return await Task.FromResult(arquivos);
        }
    }
}
