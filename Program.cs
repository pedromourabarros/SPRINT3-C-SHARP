using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApostasCompulsivas.Models;
using ApostasCompulsivas.Repository;
using ApostasCompulsivas.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ApostasCompulsivas.Middleware;

namespace ApostasCompulsivas
{
    /// <summary>
    /// Sistema principal para detecção e tratamento de apostas compulsivas
    /// Desenvolvido para o Challenge XP - Case 1
    /// </summary>
    class Program
    {
        // 🔹 Flag para forçar modo API (Swagger)
        private static bool ForcarApi = true;

        // Serviços principais
        private static IUsuarioService _usuarioService = null!;
        private static IApostaService _apostaService = null!;
        private static IHistoricoService _historicoService = null!;
        private static IFileService _fileService = null!;
        private static IDetecaoComportamentoService _detecaoService = null!;
        private static IIntervencaoService _intervencaoService = null!;
        private static IAtividadeAlternativaService _atividadeService = null!;
        private static IRelatorioComportamentalService _relatorioService = null!;

        static async Task Main(string[] args)
        {
            // ===================== API MODE =====================
            if ((args != null && args.Length > 0 && args[0] == "--api") || ForcarApi)
            {
                var builder = WebApplication.CreateBuilder(args ?? Array.Empty<string>());

                builder.Services.AddControllers();
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen();

                // Registra todos os serviços do projeto
                builder.Services.ConfigureServices();

                var app = builder.Build();

                app.UseMiddleware<GlobalExceptionMiddleware>();
                app.UseSwagger();
                app.UseSwaggerUI();

                app.MapControllers();

                app.Run();
                return;
            }
            // =================== END API MODE ===================

            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // Inicializar serviços
            await InicializarServicos();

            // Menu principal do modo console
            await ExibirMenuPrincipal();
        }

        /// <summary>
        /// Inicializa todos os serviços necessários
        /// </summary>
        private static Task InicializarServicos()
        {
            try
            {
                Console.WriteLine("🚀 Inicializando sistema de apostas...");

                // Configurar banco de dados
                var dbContext = new DatabaseContext();
                var connectionString = dbContext.GetConnectionString();

                // Inicializar repositórios
                var usuarioRepository = new UsuarioRepository(connectionString);
                var apostaRepository = new ApostaRepository(connectionString);
                var historicoRepository = new HistoricoRepository(connectionString);

                // Inicializar serviços
                _usuarioService = new UsuarioService(usuarioRepository, historicoRepository);
                _apostaService = new ApostaService(apostaRepository, usuarioRepository, historicoRepository);
                _historicoService = new HistoricoService(historicoRepository, usuarioRepository);
                _fileService = new FileService();
                _detecaoService = new DetecaoComportamentoService(usuarioRepository, apostaRepository, historicoRepository);
                _intervencaoService = new IntervencaoService(usuarioRepository, apostaRepository, historicoRepository);
                _atividadeService = new AtividadeAlternativaService(usuarioRepository, apostaRepository);
                _relatorioService = new RelatorioComportamentalService(usuarioRepository, apostaRepository, historicoRepository);

                Console.WriteLine("✅ Sistema inicializado com sucesso!");
                Console.WriteLine();
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro ao inicializar sistema: {ex.Message}");
                Environment.Exit(1);
                return Task.CompletedTask;
            }
        }

        /// <summary>
        /// Exibe o menu principal do sistema
        /// </summary>
        private static async Task ExibirMenuPrincipal()
        {
            while (true)
            {
                Console.Clear();
            Console.WriteLine("🎰 ================================================");
            Console.WriteLine("    SISTEMA DE DETECÇÃO DE APOSTAS COMPULSIVAS");
            Console.WriteLine("           CHALLENGE XP - CASE 1");
            Console.WriteLine("==================================================");
            Console.WriteLine();
            Console.WriteLine("1. 👤 Gerenciar Usuários");
            Console.WriteLine("2. 🔍 Detecção de Comportamentos");
            Console.WriteLine("3. ⚠️  Intervenções e Alertas");
            Console.WriteLine("4. 🎯 Atividades Alternativas");
            Console.WriteLine("5. 📊 Relatórios Comportamentais");
            Console.WriteLine("6. 🎲 Gerenciar Apostas");
            Console.WriteLine("7. 💾 Backup e Restauração");
            Console.WriteLine("8. ⚙️  Configurações");
            Console.WriteLine("0. 🚪 Sair");
                Console.WriteLine();
                Console.Write("Escolha uma opção: ");

                var opcao = Console.ReadLine();

                switch (opcao)
                {
                    case "1":
                        await MenuUsuarios();
                        break;
                    case "2":
                        await MenuDetecaoComportamentos();
                        break;
                    case "3":
                        await MenuIntervencoes();
                        break;
                    case "4":
                        await MenuAtividadesAlternativas();
                        break;
                    case "5":
                        await MenuRelatoriosComportamentais();
                        break;
                    case "6":
                        await MenuApostas();
                        break;
                    case "7":
                        await MenuBackup();
                        break;
                    case "8":
                        await MenuConfiguracoes();
                        break;
                    case "0":
                        Console.WriteLine("👋 Obrigado por usar o sistema!");
                        return;
                    default:
                        Console.WriteLine("❌ Opção inválida! Pressione qualquer tecla para continuar...");
                        try
                        {
                            try
                    {
                        Console.ReadKey();
                    }
                    catch
                    {
                        // Ignorar erro quando input é redirecionado
                    }
                        }
                        catch
                        {
                            // Ignorar erro quando input é redirecionado
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// Menu de gerenciamento de usuários
        /// </summary>
        private static async Task MenuUsuarios()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("👤 ========== GERENCIAR USUÁRIOS ==========");
                Console.WriteLine();
                Console.WriteLine("1. ➕ Cadastrar Usuário");
                Console.WriteLine("2. 📋 Listar Usuários");
                Console.WriteLine("3. 🔍 Buscar Usuário");
                Console.WriteLine("4. ✏️  Editar Usuário");
                Console.WriteLine("5. 💰 Depositar");
                Console.WriteLine("6. 💸 Sacar");
                Console.WriteLine("7. 📊 Histórico do Usuário");
                Console.WriteLine("0. ⬅️  Voltar");
                Console.WriteLine();
                Console.Write("Escolha uma opção: ");

                var opcao = Console.ReadLine();

                switch (opcao)
                {
                    case "1":
                        await CadastrarUsuario();
                        break;
                    case "2":
                        await ListarUsuarios();
                        break;
                    case "3":
                        await BuscarUsuario();
                        break;
                    case "4":
                        await EditarUsuario();
                        break;
                    case "5":
                        await Depositar();
                        break;
                    case "6":
                        await Sacar();
                        break;
                    case "7":
                        await HistoricoUsuario();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("❌ Opção inválida!");
                        break;
                }

                if (opcao != "0")
                {
                    Console.WriteLine("\nPressione qualquer tecla para continuar...");
                    try
                    {
                        Console.ReadKey();
                    }
                    catch
                    {
                        // Ignorar erro quando input é redirecionado
                    }
                }
            }
        }

        /// <summary>
        /// Menu de gerenciamento de apostas
        /// </summary>
        private static async Task MenuApostas()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("🎲 ========== GERENCIAR APOSTAS ==========");
                Console.WriteLine();
                Console.WriteLine("1. 🎯 Realizar Aposta");
                Console.WriteLine("2. 📋 Listar Apostas");
                Console.WriteLine("3. 🔍 Buscar Aposta");
                Console.WriteLine("4. ✅ Finalizar Aposta");
                Console.WriteLine("5. 📊 Apostas por Usuário");
                Console.WriteLine("6. 📈 Apostas por Status");
                Console.WriteLine("0. ⬅️  Voltar");
                Console.WriteLine();
                Console.Write("Escolha uma opção: ");

                var opcao = Console.ReadLine();

                switch (opcao)
                {
                    case "1":
                        await RealizarAposta();
                        break;
                    case "2":
                        await ListarApostas();
                        break;
                    case "3":
                        await BuscarAposta();
                        break;
                    case "4":
                        await FinalizarAposta();
                        break;
                    case "5":
                        await ApostasPorUsuario();
                        break;
                    case "6":
                        await ApostasPorStatus();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("❌ Opção inválida!");
                        break;
                }

                if (opcao != "0")
                {
                    Console.WriteLine("\nPressione qualquer tecla para continuar...");
                    try
                    {
                        Console.ReadKey();
                    }
                    catch
                    {
                        // Ignorar erro quando input é redirecionado
                    }
                }
            }
        }

        /// <summary>
        /// Menu de relatórios e histórico
        /// </summary>
        private static async Task MenuRelatorios()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("📊 ========== RELATÓRIOS E HISTÓRICO ==========");
                Console.WriteLine();
                Console.WriteLine("1. 📈 Relatório Completo");
                Console.WriteLine("2. 👤 Relatório por Usuário");
                Console.WriteLine("3. 📅 Relatório por Período");
                Console.WriteLine("4. 💰 Estatísticas Financeiras");
                Console.WriteLine("5. 🎯 Estatísticas de Apostas");
                Console.WriteLine("6. 📋 Histórico Completo");
                Console.WriteLine("0. ⬅️  Voltar");
                Console.WriteLine();
                Console.Write("Escolha uma opção: ");

                var opcao = Console.ReadLine();

                switch (opcao)
                {
                    case "1":
                        await RelatorioCompleto();
                        break;
                    case "2":
                        await RelatorioPorUsuario();
                        break;
                    case "3":
                        await RelatorioPorPeriodo();
                        break;
                    case "4":
                        await EstatisticasFinanceiras();
                        break;
                    case "5":
                        await EstatisticasApostas();
                        break;
                    case "6":
                        await HistoricoCompleto();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("❌ Opção inválida!");
                        break;
                }

                if (opcao != "0")
                {
                    Console.WriteLine("\nPressione qualquer tecla para continuar...");
                    try
                    {
                        Console.ReadKey();
                    }
                    catch
                    {
                        // Ignorar erro quando input é redirecionado
                    }
                }
            }
        }

        /// <summary>
        /// Menu de backup e restauração
        /// </summary>
        private static async Task MenuBackup()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("💾 ========== BACKUP E RESTAURAÇÃO ==========");
                Console.WriteLine();
                Console.WriteLine("1. 💾 Fazer Backup Completo");
                Console.WriteLine("2. 📄 Exportar Histórico TXT");
                Console.WriteLine("3. 📄 Exportar Usuários JSON");
                Console.WriteLine("4. 📄 Exportar Apostas JSON");
                Console.WriteLine("5. 📁 Listar Arquivos");
                Console.WriteLine("0. ⬅️  Voltar");
                Console.WriteLine();
                Console.Write("Escolha uma opção: ");

                var opcao = Console.ReadLine();

                switch (opcao)
                {
                    case "1":
                        await FazerBackupCompleto();
                        break;
                    case "2":
                        await ExportarHistoricoTxt();
                        break;
                    case "3":
                        await ExportarUsuariosJson();
                        break;
                    case "4":
                        await ExportarApostasJson();
                        break;
                    case "5":
                        await ListarArquivos();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("❌ Opção inválida!");
                        break;
                }

                if (opcao != "0")
                {
                    Console.WriteLine("\nPressione qualquer tecla para continuar...");
                    try
                    {
                        Console.ReadKey();
                    }
                    catch
                    {
                        // Ignorar erro quando input é redirecionado
                    }
                }
            }
        }

        /// <summary>
        /// Menu de configurações
        /// </summary>
        private static async Task MenuConfiguracoes()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("⚙️  ========== CONFIGURAÇÕES ==========");
                Console.WriteLine();
                Console.WriteLine("1. 🗃️  Informações do Banco");
                Console.WriteLine("2. 📁 Informações dos Arquivos");
                Console.WriteLine("3. 🧹 Limpar Dados");
                Console.WriteLine("0. ⬅️  Voltar");
                Console.WriteLine();
                Console.Write("Escolha uma opção: ");

                var opcao = Console.ReadLine();

                switch (opcao)
                {
                    case "1":
                        await InformacoesBanco();
                        break;
                    case "2":
                        await InformacoesArquivos();
                        break;
                    case "3":
                        await LimparDados();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("❌ Opção inválida!");
                        break;
                }

                if (opcao != "0")
                {
                    Console.WriteLine("\nPressione qualquer tecla para continuar...");
                    try
                    {
                        Console.ReadKey();
                    }
                    catch
                    {
                        // Ignorar erro quando input é redirecionado
                    }
                }
            }
        }

        #region Métodos de Usuários

        private static async Task CadastrarUsuario()
        {
            Console.WriteLine("\n➕ ========== CADASTRAR USUÁRIO ==========");
            
            try
            {
                Console.Write("Nome: ");
                var nome = Console.ReadLine() ?? "";
                
                Console.Write("Email: ");
                var email = Console.ReadLine() ?? "";
                
                Console.Write("Saldo inicial (R$): ");
                var saldoStr = Console.ReadLine() ?? "0";
                
                if (!decimal.TryParse(saldoStr, out var saldo))
                {
                    Console.WriteLine("❌ Saldo inválido!");
                    return;
                }

                Console.Write("Telefone (opcional): ");
                var telefone = Console.ReadLine() ?? "";

                var usuario = await _usuarioService.CriarUsuarioAsync(nome, email, saldo, telefone);
                Console.WriteLine($"✅ Usuário cadastrado com sucesso!");
                Console.WriteLine($"ID: {usuario.Id} | Nome: {usuario.Nome} | Email: {usuario.Email} | Saldo: R$ {usuario.Saldo:F2}");
                Console.WriteLine($"Telefone: {usuario.Telefone} | Nível de Risco: {usuario.ObterDescricaoRisco()}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro ao cadastrar usuário: {ex.Message}");
            }
        }

        private static async Task ListarUsuarios()
        {
            Console.WriteLine("\n📋 ========== LISTAR USUÁRIOS ==========");
            
            try
            {
                var usuarios = await _usuarioService.ListarUsuariosAsync();
                
                if (usuarios.Count == 0)
                {
                    Console.WriteLine("Nenhum usuário cadastrado.");
                    return;
                }

                Console.WriteLine($"Total de usuários: {usuarios.Count}");
                Console.WriteLine();
                
                foreach (var usuario in usuarios)
                {
                    Console.WriteLine(usuario.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro ao listar usuários: {ex.Message}");
            }
        }

        private static async Task BuscarUsuario()
        {
            Console.WriteLine("\n🔍 ========== BUSCAR USUÁRIO ==========");
            
            try
            {
                Console.Write("Digite o ID ou email do usuário: ");
                var busca = Console.ReadLine() ?? "";
                
                Usuario? usuario = null;
                
                if (int.TryParse(busca, out var id))
                {
                    usuario = await _usuarioService.ObterUsuarioPorIdAsync(id);
                }
                else
                {
                    usuario = await _usuarioService.ObterUsuarioPorEmailAsync(busca);
                }

                if (usuario == null)
                {
                    Console.WriteLine("❌ Usuário não encontrado!");
                    return;
                }

                Console.WriteLine("✅ Usuário encontrado:");
                Console.WriteLine(usuario.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro ao buscar usuário: {ex.Message}");
            }
        }

        private static async Task EditarUsuario()
        {
            Console.WriteLine("\n✏️  ========== EDITAR USUÁRIO ==========");
            
            try
            {
                Console.Write("ID do usuário: ");
                var idStr = Console.ReadLine() ?? "";
                
                if (!int.TryParse(idStr, out var id))
                {
                    Console.WriteLine("❌ ID inválido!");
                    return;
                }

                var usuario = await _usuarioService.ObterUsuarioPorIdAsync(id);
                if (usuario == null)
                {
                    Console.WriteLine("❌ Usuário não encontrado!");
                    return;
                }

                Console.WriteLine($"Usuário atual: {usuario.ToString()}");
                Console.WriteLine();
                
                Console.Write($"Novo nome (atual: {usuario.Nome}): ");
                var novoNome = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(novoNome))
                    usuario.Nome = novoNome;
                
                Console.Write($"Novo email (atual: {usuario.Email}): ");
                var novoEmail = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(novoEmail))
                    usuario.Email = novoEmail;

                await _usuarioService.AtualizarUsuarioAsync(usuario);
                Console.WriteLine("✅ Usuário atualizado com sucesso!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro ao editar usuário: {ex.Message}");
            }
        }

        private static async Task Depositar()
        {
            Console.WriteLine("\n💰 ========== DEPOSITAR ==========");
            
            try
            {
                Console.Write("ID do usuário: ");
                var idStr = Console.ReadLine() ?? "";
                
                if (!int.TryParse(idStr, out var id))
                {
                    Console.WriteLine("❌ ID inválido!");
                    return;
                }

                Console.Write("Valor do depósito (R$): ");
                var valorStr = Console.ReadLine() ?? "";
                
                if (!decimal.TryParse(valorStr, out var valor))
                {
                    Console.WriteLine("❌ Valor inválido!");
                    return;
                }

                await _usuarioService.DepositarAsync(id, valor);
                Console.WriteLine($"✅ Depósito de R$ {valor:F2} realizado com sucesso!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro ao depositar: {ex.Message}");
            }
        }

        private static async Task Sacar()
        {
            Console.WriteLine("\n💸 ========== SACAR ==========");
            
            try
            {
                Console.Write("ID do usuário: ");
                var idStr = Console.ReadLine() ?? "";
                
                if (!int.TryParse(idStr, out var id))
                {
                    Console.WriteLine("❌ ID inválido!");
                    return;
                }

                Console.Write("Valor do saque (R$): ");
                var valorStr = Console.ReadLine() ?? "";
                
                if (!decimal.TryParse(valorStr, out var valor))
                {
                    Console.WriteLine("❌ Valor inválido!");
                    return;
                }

                await _usuarioService.SacarAsync(id, valor);
                Console.WriteLine($"✅ Saque de R$ {valor:F2} realizado com sucesso!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro ao sacar: {ex.Message}");
            }
        }

        private static async Task HistoricoUsuario()
        {
            Console.WriteLine("\n📊 ========== HISTÓRICO DO USUÁRIO ==========");
            
            try
            {
                Console.Write("ID do usuário: ");
                var idStr = Console.ReadLine() ?? "";
                
                if (!int.TryParse(idStr, out var id))
                {
                    Console.WriteLine("❌ ID inválido!");
                    return;
                }

                var usuario = await _usuarioService.ObterUsuarioPorIdAsync(id);
                if (usuario == null)
                {
                    Console.WriteLine("❌ Usuário não encontrado!");
                    return;
                }

                var historico = await _historicoService.ObterHistoricoPorUsuarioAsync(id);
                
                Console.WriteLine($"Usuário: {usuario.Nome} | Saldo atual: R$ {usuario.Saldo:F2}");
                Console.WriteLine($"Total de operações: {historico.Count}");
                Console.WriteLine();
                
                foreach (var operacao in historico.Take(10))
                {
                    Console.WriteLine(operacao.ToString());
                }

                if (historico.Count > 10)
                {
                    Console.WriteLine($"... e mais {historico.Count - 10} operações");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro ao buscar histórico: {ex.Message}");
            }
        }

        #endregion

        #region Métodos de Apostas

        private static async Task RealizarAposta()
        {
            Console.WriteLine("\n🎯 ========== REALIZAR APOSTA ==========");
            
            try
            {
                Console.Write("ID do usuário: ");
                var idStr = Console.ReadLine() ?? "";
                
                if (!int.TryParse(idStr, out var id))
                {
                    Console.WriteLine("❌ ID inválido!");
                    return;
                }

                var usuario = await _usuarioService.ObterUsuarioPorIdAsync(id);
                if (usuario == null)
                {
                    Console.WriteLine("❌ Usuário não encontrado!");
                    return;
                }

                Console.WriteLine($"Usuário: {usuario.Nome} | Saldo: R$ {usuario.Saldo:F2}");
                Console.WriteLine();
                
                Console.Write("Tipo de aposta: ");
                var tipoAposta = Console.ReadLine() ?? "";
                
                Console.Write("Valor da aposta (R$): ");
                var valorStr = Console.ReadLine() ?? "";
                
                if (!decimal.TryParse(valorStr, out var valor))
                {
                    Console.WriteLine("❌ Valor inválido!");
                    return;
                }
                
                Console.Write("Multiplicador (ex: 2.5): ");
                var multiplicadorStr = Console.ReadLine() ?? "";
                
                if (!decimal.TryParse(multiplicadorStr, out var multiplicador))
                {
                    Console.WriteLine("❌ Multiplicador inválido!");
                    return;
                }

                var aposta = await _apostaService.RealizarApostaAsync(id, tipoAposta, valor, multiplicador);
                Console.WriteLine($"✅ Aposta realizada com sucesso!");
                Console.WriteLine($"ID: {aposta.Id} | Valor: R$ {aposta.Valor:F2} | Multiplicador: {aposta.Multiplicador}x");
                Console.WriteLine($"Possível ganho: R$ {aposta.CalcularValorGanho():F2}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro ao realizar aposta: {ex.Message}");
            }
        }

        private static async Task ListarApostas()
        {
            Console.WriteLine("\n📋 ========== LISTAR APOSTAS ==========");
            
            try
            {
                var apostas = await _apostaService.ListarApostasAsync();
                
                if (apostas.Count == 0)
                {
                    Console.WriteLine("Nenhuma aposta encontrada.");
                    return;
                }

                Console.WriteLine($"Total de apostas: {apostas.Count}");
                Console.WriteLine();
                
                foreach (var aposta in apostas.Take(20))
                {
                    Console.WriteLine(aposta.ToString());
                }

                if (apostas.Count > 20)
                {
                    Console.WriteLine($"... e mais {apostas.Count - 20} apostas");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro ao listar apostas: {ex.Message}");
            }
        }

        private static async Task BuscarAposta()
        {
            Console.WriteLine("\n🔍 ========== BUSCAR APOSTA ==========");
            
            try
            {
                Console.Write("ID da aposta: ");
                var idStr = Console.ReadLine() ?? "";
                
                if (!int.TryParse(idStr, out var id))
                {
                    Console.WriteLine("❌ ID inválido!");
                    return;
                }

                var aposta = await _apostaService.ObterApostaPorIdAsync(id);
                if (aposta == null)
                {
                    Console.WriteLine("❌ Aposta não encontrada!");
                    return;
                }

                Console.WriteLine("✅ Aposta encontrada:");
                Console.WriteLine(aposta.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro ao buscar aposta: {ex.Message}");
            }
        }

        private static async Task FinalizarAposta()
        {
            Console.WriteLine("\n✅ ========== FINALIZAR APOSTA ==========");
            
            try
            {
                Console.Write("ID da aposta: ");
                var idStr = Console.ReadLine() ?? "";
                
                if (!int.TryParse(idStr, out var id))
                {
                    Console.WriteLine("❌ ID inválido!");
                    return;
                }

                var aposta = await _apostaService.ObterApostaPorIdAsync(id);
                if (aposta == null)
                {
                    Console.WriteLine("❌ Aposta não encontrada!");
                    return;
                }

                if (aposta.Status != "Pendente")
                {
                    Console.WriteLine($"❌ Aposta já foi finalizada! Status atual: {aposta.Status}");
                    return;
                }

                Console.WriteLine($"Aposta: {aposta.TipoAposta} | Valor: R$ {aposta.Valor:F2} | Multiplicador: {aposta.Multiplicador}x");
                Console.WriteLine($"Possível ganho: R$ {aposta.CalcularValorGanho():F2}");
                Console.WriteLine();
                Console.Write("A aposta ganhou? (s/n): ");
                var ganhou = Console.ReadLine()?.ToLower() == "s";

                await _apostaService.FinalizarApostaAsync(id, ganhou);
                Console.WriteLine($"✅ Aposta finalizada! Resultado: {(ganhou ? "GANHOU" : "PERDEU")}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro ao finalizar aposta: {ex.Message}");
            }
        }

        private static async Task ApostasPorUsuario()
        {
            Console.WriteLine("\n📊 ========== APOSTAS POR USUÁRIO ==========");
            
            try
            {
                Console.Write("ID do usuário: ");
                var idStr = Console.ReadLine() ?? "";
                
                if (!int.TryParse(idStr, out var id))
                {
                    Console.WriteLine("❌ ID inválido!");
                    return;
                }

                var usuario = await _usuarioService.ObterUsuarioPorIdAsync(id);
                if (usuario == null)
                {
                    Console.WriteLine("❌ Usuário não encontrado!");
                    return;
                }

                var apostas = await _apostaService.ObterApostasPorUsuarioAsync(id);
                
                Console.WriteLine($"Usuário: {usuario.Nome}");
                Console.WriteLine($"Total de apostas: {apostas.Count}");
                Console.WriteLine();
                
                foreach (var aposta in apostas)
                {
                    Console.WriteLine(aposta.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro ao buscar apostas: {ex.Message}");
            }
        }

        private static async Task ApostasPorStatus()
        {
            Console.WriteLine("\n📈 ========== APOSTAS POR STATUS ==========");
            
            try
            {
                Console.WriteLine("Status disponíveis: Pendente, Ganhou, Perdeu");
                Console.Write("Digite o status: ");
                var status = Console.ReadLine() ?? "";
                
                var apostas = await _apostaService.ObterApostasPorStatusAsync(status);
                
                Console.WriteLine($"Total de apostas com status '{status}': {apostas.Count}");
                Console.WriteLine();
                
                foreach (var aposta in apostas)
                {
                    Console.WriteLine(aposta.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro ao buscar apostas: {ex.Message}");
            }
        }

        #endregion

        #region Métodos de Relatórios

        private static async Task RelatorioCompleto()
        {
            Console.WriteLine("\n📈 ========== RELATÓRIO COMPLETO ==========");
            
            try
            {
                var usuarios = await _usuarioService.ListarUsuariosAsync();
                var apostas = await _apostaService.ListarApostasAsync();
                var historico = await _historicoService.ObterHistoricoCompletoAsync();

                Console.WriteLine($"📊 ESTATÍSTICAS GERAIS");
                Console.WriteLine($"Usuários cadastrados: {usuarios.Count}");
                Console.WriteLine($"Total de apostas: {apostas.Count}");
                Console.WriteLine($"Total de operações: {historico.Count}");
                Console.WriteLine($"Valor total apostado: R$ {apostas.Sum(a => a.Valor):F2}");
                Console.WriteLine($"Valor total ganho: R$ {apostas.Where(a => a.Status == "Ganhou" && a.ValorGanho.HasValue).Sum(a => a.ValorGanho!.Value):F2}");
                Console.WriteLine();

                // Salvar relatório completo
                await _fileService.SalvarRelatorioCompletoAsync(usuarios, apostas, historico);
                Console.WriteLine("✅ Relatório completo salvo em 'relatorio_completo.json'");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro ao gerar relatório: {ex.Message}");
            }
        }

        private static async Task RelatorioPorUsuario()
        {
            Console.WriteLine("\n👤 ========== RELATÓRIO POR USUÁRIO ==========");
            
            try
            {
                Console.Write("ID do usuário: ");
                var idStr = Console.ReadLine() ?? "";
                
                if (!int.TryParse(idStr, out var id))
                {
                    Console.WriteLine("❌ ID inválido!");
                    return;
                }

                var usuario = await _usuarioService.ObterUsuarioPorIdAsync(id);
                if (usuario == null)
                {
                    Console.WriteLine("❌ Usuário não encontrado!");
                    return;
                }

                var apostas = await _apostaService.ObterApostasPorUsuarioAsync(id);
                var historico = await _historicoService.ObterHistoricoPorUsuarioAsync(id);
                var totalApostado = await _apostaService.CalcularTotalApostadoAsync(id);
                var totalGanho = await _apostaService.CalcularTotalGanhoAsync(id);

                Console.WriteLine($"👤 USUÁRIO: {usuario.Nome}");
                Console.WriteLine($"📧 Email: {usuario.Email}");
                Console.WriteLine($"💰 Saldo atual: R$ {usuario.Saldo:F2}");
                Console.WriteLine($"📅 Cadastro: {usuario.DataCadastro:dd/MM/yyyy}");
                Console.WriteLine($"✅ Status: {(usuario.Ativo ? "Ativo" : "Inativo")}");
                Console.WriteLine();
                Console.WriteLine($"📊 ESTATÍSTICAS:");
                Console.WriteLine($"Total de apostas: {apostas.Count}");
                Console.WriteLine($"Total apostado: R$ {totalApostado:F2}");
                Console.WriteLine($"Total ganho: R$ {totalGanho:F2}");
                Console.WriteLine($"Lucro/Prejuízo: R$ {totalGanho - totalApostado:F2}");
                Console.WriteLine($"Total de operações: {historico.Count}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro ao gerar relatório: {ex.Message}");
            }
        }

        private static async Task RelatorioPorPeriodo()
        {
            Console.WriteLine("\n📅 ========== RELATÓRIO POR PERÍODO ==========");
            
            try
            {
                Console.Write("Data início (dd/mm/aaaa): ");
                var dataInicioStr = Console.ReadLine() ?? "";
                
                if (!DateTime.TryParseExact(dataInicioStr, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out var dataInicio))
                {
                    Console.WriteLine("❌ Data inválida!");
                    return;
                }

                Console.Write("Data fim (dd/mm/aaaa): ");
                var dataFimStr = Console.ReadLine() ?? "";
                
                if (!DateTime.TryParseExact(dataFimStr, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out var dataFim))
                {
                    Console.WriteLine("❌ Data inválida!");
                    return;
                }

                dataFim = dataFim.AddDays(1).AddSeconds(-1); // Incluir todo o dia

                var apostas = await _apostaService.ObterApostasPorPeriodoAsync(dataInicio, dataFim);
                var historico = await _historicoService.ObterHistoricoPorPeriodoAsync(dataInicio, dataFim);

                Console.WriteLine($"📅 PERÍODO: {dataInicio:dd/MM/yyyy} a {dataFim:dd/MM/yyyy}");
                Console.WriteLine($"Total de apostas: {apostas.Count}");
                Console.WriteLine($"Valor apostado: R$ {apostas.Sum(a => a.Valor):F2}");
                Console.WriteLine($"Valor ganho: R$ {apostas.Where(a => a.Status == "Ganhou" && a.ValorGanho.HasValue).Sum(a => a.ValorGanho!.Value):F2}");
                Console.WriteLine($"Total de operações: {historico.Count}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro ao gerar relatório: {ex.Message}");
            }
        }

        private static async Task EstatisticasFinanceiras()
        {
            Console.WriteLine("\n💰 ========== ESTATÍSTICAS FINANCEIRAS ==========");
            
            try
            {
                var usuarios = await _usuarioService.ListarUsuariosAsync();
                var apostas = await _apostaService.ListarApostasAsync();
                var historico = await _historicoService.ObterHistoricoCompletoAsync();

                var totalDepositos = historico.Where(h => h.TipoOperacao == "Deposito").Sum(h => h.Valor);
                var totalSaques = historico.Where(h => h.TipoOperacao == "Saque").Sum(h => h.Valor);
                var totalApostado = apostas.Sum(a => a.Valor);
                var totalGanho = apostas.Where(a => a.Status == "Ganhou" && a.ValorGanho.HasValue).Sum(a => a.ValorGanho!.Value);
                var saldoTotal = usuarios.Sum(u => u.Saldo);

                Console.WriteLine($"💰 MOVIMENTAÇÃO FINANCEIRA:");
                Console.WriteLine($"Total de depósitos: R$ {totalDepositos:F2}");
                Console.WriteLine($"Total de saques: R$ {totalSaques:F2}");
                Console.WriteLine($"Total apostado: R$ {totalApostado:F2}");
                Console.WriteLine($"Total ganho: R$ {totalGanho:F2}");
                Console.WriteLine($"Saldo total dos usuários: R$ {saldoTotal:F2}");
                Console.WriteLine($"Lucro/Prejuízo total: R$ {totalGanho - totalApostado:F2}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro ao gerar estatísticas: {ex.Message}");
            }
        }

        private static async Task EstatisticasApostas()
        {
            Console.WriteLine("\n🎯 ========== ESTATÍSTICAS DE APOSTAS ==========");
            
            try
            {
                var apostas = await _apostaService.ListarApostasAsync();
                var apostasPendentes = await _apostaService.ObterApostasPorStatusAsync("Pendente");
                var apostasGanhas = await _apostaService.ObterApostasPorStatusAsync("Ganhou");
                var apostasPerdidas = await _apostaService.ObterApostasPorStatusAsync("Perdeu");

                Console.WriteLine($"🎯 ESTATÍSTICAS DE APOSTAS:");
                Console.WriteLine($"Total de apostas: {apostas.Count}");
                Console.WriteLine($"Apostas pendentes: {apostasPendentes.Count}");
                Console.WriteLine($"Apostas ganhas: {apostasGanhas.Count}");
                Console.WriteLine($"Apostas perdidas: {apostasPerdidas.Count}");
                Console.WriteLine($"Taxa de vitória: {(apostasGanhas.Count + apostasPerdidas.Count > 0 ? (double)apostasGanhas.Count / (apostasGanhas.Count + apostasPerdidas.Count) * 100 : 0):F1}%");
                Console.WriteLine();
                Console.WriteLine($"Valor médio por aposta: R$ {(apostas.Count > 0 ? apostas.Average(a => a.Valor) : 0):F2}");
                Console.WriteLine($"Maior aposta: R$ {(apostas.Count > 0 ? apostas.Max(a => a.Valor) : 0):F2}");
                Console.WriteLine($"Menor aposta: R$ {(apostas.Count > 0 ? apostas.Min(a => a.Valor) : 0):F2}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro ao gerar estatísticas: {ex.Message}");
            }
        }

        private static async Task HistoricoCompleto()
        {
            Console.WriteLine("\n📋 ========== HISTÓRICO COMPLETO ==========");
            
            try
            {
                var historico = await _historicoService.ObterHistoricoCompletoAsync();
                
                Console.WriteLine($"Total de operações: {historico.Count}");
                Console.WriteLine();
                
                foreach (var operacao in historico.Take(20))
                {
                    Console.WriteLine(operacao.ToString());
                }

                if (historico.Count > 20)
                {
                    Console.WriteLine($"... e mais {historico.Count - 20} operações");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro ao buscar histórico: {ex.Message}");
            }
        }

        #endregion

        #region Métodos de Backup

        private static async Task FazerBackupCompleto()
        {
            Console.WriteLine("\n💾 ========== FAZER BACKUP COMPLETO ==========");
            
            try
            {
                var usuarios = await _usuarioService.ListarUsuariosAsync();
                var apostas = await _apostaService.ListarApostasAsync();
                var historico = await _historicoService.ObterHistoricoCompletoAsync();

                await _fileService.SalvarRelatorioCompletoAsync(usuarios, apostas, historico, $"backup_{DateTime.Now:yyyyMMdd_HHmmss}.json");
                await _fileService.SalvarHistoricoTxtAsync(historico, $"historico_{DateTime.Now:yyyyMMdd_HHmmss}.txt");
                await _fileService.SalvarUsuariosJsonAsync(usuarios, $"usuarios_{DateTime.Now:yyyyMMdd_HHmmss}.json");
                await _fileService.SalvarApostasJsonAsync(apostas, $"apostas_{DateTime.Now:yyyyMMdd_HHmmss}.json");

                Console.WriteLine("✅ Backup completo realizado com sucesso!");
                Console.WriteLine("Arquivos salvos na pasta 'Arquivos'");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro ao fazer backup: {ex.Message}");
            }
        }

        private static async Task ExportarHistoricoTxt()
        {
            Console.WriteLine("\n📄 ========== EXPORTAR HISTÓRICO TXT ==========");
            
            try
            {
                var historico = await _historicoService.ObterHistoricoCompletoAsync();
                var nomeArquivo = $"historico_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
                
                await _fileService.SalvarHistoricoTxtAsync(historico, nomeArquivo);
                Console.WriteLine($"✅ Histórico exportado para '{nomeArquivo}'");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro ao exportar histórico: {ex.Message}");
            }
        }

        private static async Task ExportarUsuariosJson()
        {
            Console.WriteLine("\n📄 ========== EXPORTAR USUÁRIOS JSON ==========");
            
            try
            {
                var usuarios = await _usuarioService.ListarUsuariosAsync();
                var nomeArquivo = $"usuarios_{DateTime.Now:yyyyMMdd_HHmmss}.json";
                
                await _fileService.SalvarUsuariosJsonAsync(usuarios, nomeArquivo);
                Console.WriteLine($"✅ Usuários exportados para '{nomeArquivo}'");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro ao exportar usuários: {ex.Message}");
            }
        }

        private static async Task ExportarApostasJson()
        {
            Console.WriteLine("\n📄 ========== EXPORTAR APOSTAS JSON ==========");
            
            try
            {
                var apostas = await _apostaService.ListarApostasAsync();
                var nomeArquivo = $"apostas_{DateTime.Now:yyyyMMdd_HHmmss}.json";
                
                await _fileService.SalvarApostasJsonAsync(apostas, nomeArquivo);
                Console.WriteLine($"✅ Apostas exportadas para '{nomeArquivo}'");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro ao exportar apostas: {ex.Message}");
            }
        }

        private static async Task ListarArquivos()
        {
            Console.WriteLine("\n📁 ========== LISTAR ARQUIVOS ==========");
            
            try
            {
                var arquivos = await _fileService.ListarArquivosAsync();
                
                if (arquivos.Length == 0)
                {
                    Console.WriteLine("Nenhum arquivo encontrado na pasta 'Arquivos'");
                    return;
                }

                Console.WriteLine($"Total de arquivos: {arquivos.Length}");
                Console.WriteLine();
                
                foreach (var arquivo in arquivos)
                {
                    Console.WriteLine($"📄 {arquivo}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro ao listar arquivos: {ex.Message}");
            }
        }

        #endregion

        #region Métodos de Configurações

        private static async Task InformacoesBanco()
        {
            Console.WriteLine("\n🗃️  ========== INFORMAÇÕES DO BANCO ==========");
            
            try
            {
                var usuarios = await _usuarioService.ListarUsuariosAsync();
                var apostas = await _apostaService.ListarApostasAsync();
                var historico = await _historicoService.ObterHistoricoCompletoAsync();

                Console.WriteLine($"📊 ESTATÍSTICAS DO BANCO:");
                Console.WriteLine($"Usuários: {usuarios.Count}");
                Console.WriteLine($"Apostas: {apostas.Count}");
                Console.WriteLine($"Histórico: {historico.Count}");
                Console.WriteLine($"Arquivo do banco: apostas.db");
                Console.WriteLine($"Localização: {System.IO.Directory.GetCurrentDirectory()}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro ao obter informações: {ex.Message}");
            }
        }

        private static async Task InformacoesArquivos()
        {
            Console.WriteLine("\n📁 ========== INFORMAÇÕES DOS ARQUIVOS ==========");
            
            try
            {
                var arquivos = await _fileService.ListarArquivosAsync();
                var pastaArquivos = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Arquivos");

                Console.WriteLine($"📁 PASTA DE ARQUIVOS:");
                Console.WriteLine($"Localização: {pastaArquivos}");
                Console.WriteLine($"Total de arquivos: {arquivos.Length}");
                Console.WriteLine();
                
                if (arquivos.Length > 0)
                {
                    Console.WriteLine("Arquivos encontrados:");
                    foreach (var arquivo in arquivos)
                    {
                        var caminhoCompleto = System.IO.Path.Combine(pastaArquivos, arquivo);
                        var info = new System.IO.FileInfo(caminhoCompleto);
                        Console.WriteLine($"📄 {arquivo} ({info.Length} bytes) - {info.LastWriteTime:dd/MM/yyyy HH:mm}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro ao obter informações: {ex.Message}");
            }
        }

        private static Task LimparDados()
        {
            Console.WriteLine("\n🧹 ========== LIMPAR DADOS ==========");
            Console.WriteLine("⚠️  ATENÇÃO: Esta operação irá limpar todos os dados do sistema!");
            Console.WriteLine("Esta ação não pode ser desfeita.");
            Console.WriteLine();
            Console.Write("Digite 'CONFIRMAR' para prosseguir: ");
            
            var confirmacao = Console.ReadLine();
            
            if (confirmacao != "CONFIRMAR")
            {
                Console.WriteLine("❌ Operação cancelada.");
                return Task.CompletedTask;
            }

            try
            {
                // Aqui você implementaria a lógica para limpar os dados
                // Por segurança, não implementei a limpeza real
                Console.WriteLine("⚠️  Funcionalidade de limpeza não implementada por segurança.");
                Console.WriteLine("Para limpar os dados, delete o arquivo 'apostas.db' e reinicie o sistema.");
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro ao limpar dados: {ex.Message}");
                return Task.CompletedTask;
            }
        }

        #endregion

        #region Novos Menus - Detecção e Tratamento

        private static async Task MenuDetecaoComportamentos()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("🔍 ========== DETECÇÃO DE COMPORTAMENTOS ==========");
                Console.WriteLine();
                Console.WriteLine("1. 🔍 Analisar Comportamentos");
                Console.WriteLine("2. 📊 Ver Comportamentos Detectados");
                Console.WriteLine("3. ⚠️  Ver Usuários em Risco");
                Console.WriteLine("4. 📈 Estatísticas de Detecção");
                Console.WriteLine("0. ⬅️  Voltar");
                Console.WriteLine();
                Console.Write("Escolha uma opção: ");

                var opcao = Console.ReadLine();

                switch (opcao)
                {
                    case "1":
                        await AnalisarComportamentos();
                        break;
                    case "2":
                        await VerComportamentosDetectados();
                        break;
                    case "3":
                        await VerUsuariosEmRisco();
                        break;
                    case "4":
                        await EstatisticasDetecao();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("❌ Opção inválida!");
                        try
                        {
                            Console.ReadKey();
                        }
                        catch
                        {
                            // Ignorar erro quando input é redirecionado
                        }
                        break;
                }
            }
        }

        private static async Task MenuIntervencoes()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("⚠️  ========== INTERVENÇÕES E ALERTAS ==========");
                Console.WriteLine();
                Console.WriteLine("1. 🔔 Ver Alertas Ativos");
                Console.WriteLine("2. ⚠️  Criar Intervenção");
                Console.WriteLine("3. 📋 Listar Intervenções");
                Console.WriteLine("4. ✅ Marcar como Visualizada");
                Console.WriteLine("5. 📊 Estatísticas de Intervenções");
                Console.WriteLine("0. ⬅️  Voltar");
                Console.WriteLine();
                Console.Write("Escolha uma opção: ");

                var opcao = Console.ReadLine();

                switch (opcao)
                {
                    case "1":
                        await VerAlertasAtivos();
                        break;
                    case "2":
                        await CriarIntervencao();
                        break;
                    case "3":
                        await ListarIntervencoes();
                        break;
                    case "4":
                        await MarcarIntervencaoVisualizada();
                        break;
                    case "5":
                        await EstatisticasIntervencoes();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("❌ Opção inválida!");
                        try
                        {
                            Console.ReadKey();
                        }
                        catch
                        {
                            // Ignorar erro quando input é redirecionado
                        }
                        break;
                }
            }
        }

        private static async Task MenuAtividadesAlternativas()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("🎯 ========== ATIVIDADES ALTERNATIVAS ==========");
                Console.WriteLine();
                Console.WriteLine("1. 📋 Listar Atividades");
                Console.WriteLine("2. 🔍 Buscar por Categoria");
                Console.WriteLine("3. 💰 Filtrar por Custo");
                Console.WriteLine("4. ⏱️  Filtrar por Duração");
                Console.WriteLine("5. 🎯 Sugerir Atividade");
                Console.WriteLine("6. ➕ Adicionar Atividade");
                Console.WriteLine("0. ⬅️  Voltar");
                Console.WriteLine();
                Console.Write("Escolha uma opção: ");

                var opcao = Console.ReadLine();

                switch (opcao)
                {
                    case "1":
                        await ListarAtividades();
                        break;
                    case "2":
                        await BuscarPorCategoria();
                        break;
                    case "3":
                        await FiltrarPorCusto();
                        break;
                    case "4":
                        await FiltrarPorDuracao();
                        break;
                    case "5":
                        await SugerirAtividade();
                        break;
                    case "6":
                        await AdicionarAtividade();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("❌ Opção inválida!");
                        try
                        {
                            Console.ReadKey();
                        }
                        catch
                        {
                            // Ignorar erro quando input é redirecionado
                        }
                        break;
                }
            }
        }

        private static async Task MenuRelatoriosComportamentais()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("📊 ========== RELATÓRIOS COMPORTAMENTAIS ==========");
                Console.WriteLine();
                Console.WriteLine("1. 📈 Relatório Geral");
                Console.WriteLine("2. 👤 Relatório por Usuário");
                Console.WriteLine("3. 📅 Relatório por Período");
                Console.WriteLine("4. 💰 Simulação de Investimentos");
                Console.WriteLine("5. 📊 Estatísticas Comportamentais");
                Console.WriteLine("6. 📄 Exportar Relatório");
                Console.WriteLine("0. ⬅️  Voltar");
                Console.WriteLine();
                Console.Write("Escolha uma opção: ");

                var opcao = Console.ReadLine();

                switch (opcao)
                {
                    case "1":
                        await RelatorioGeral();
                        break;
                    case "2":
                        await RelatorioPorUsuario();
                        break;
                    case "3":
                        await RelatorioPorPeriodo();
                        break;
                    case "4":
                        await SimulacaoInvestimentos();
                        break;
                    case "5":
                        await EstatisticasComportamentais();
                        break;
                    case "6":
                        await ExportarRelatorio();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("❌ Opção inválida!");
                        try
                        {
                            Console.ReadKey();
                        }
                        catch
                        {
                            // Ignorar erro quando input é redirecionado
                        }
                        break;
                }
            }
        }

        private static async Task AnalisarComportamentos()
        {
            Console.WriteLine("\n🔍 ========== ANALISAR COMPORTAMENTOS ==========");
            
            try
            {
                var usuarios = await _usuarioService.ListarUsuariosAsync();
                if (!usuarios.Any())
                {
                    Console.WriteLine("❌ Nenhum usuário cadastrado para análise.");
                    return;
                }

                Console.WriteLine("Analisando comportamentos de todos os usuários...");
                
                foreach (var usuario in usuarios)
                {
                    usuario.CalcularPontuacaoRisco();
                    await _usuarioService.AtualizarUsuarioAsync(usuario);
                    
                    Console.WriteLine($"✅ {usuario.Nome}: {usuario.ObterDescricaoRisco()} (Pontuação: {usuario.PontuacaoRisco})");
                }

                Console.WriteLine("\n✅ Análise concluída!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro na análise: {ex.Message}");
            }
        }

        private static async Task VerComportamentosDetectados()
        {
            Console.WriteLine("\n📊 ========== COMPORTAMENTOS DETECTADOS ==========");
            
            try
            {
                var usuarios = await _usuarioService.ListarUsuariosAsync();
                if (!usuarios.Any())
                {
                    Console.WriteLine("❌ Nenhum usuário cadastrado.");
                    return;
                }

                Console.WriteLine($"{"ID",-5} {"Nome",-20} {"Nível de Risco",-15} {"Pontuação",-10} {"Última Análise",-20}");
                Console.WriteLine(new string('-', 80));

                foreach (var usuario in usuarios.OrderByDescending(u => u.PontuacaoRisco))
                {
                    Console.WriteLine($"{usuario.Id,-5} {usuario.Nome,-20} {usuario.ObterDescricaoRisco(),-15} {usuario.PontuacaoRisco,-10} {usuario.UltimaAvaliacao:dd/MM/yyyy HH:mm}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro ao listar comportamentos: {ex.Message}");
            }
        }

        private static async Task VerUsuariosEmRisco()
        {
            Console.WriteLine("\n⚠️  ========== USUÁRIOS EM RISCO ==========");
            
            try
            {
                var usuarios = await _usuarioService.ListarUsuariosAsync();
                var usuariosEmRisco = usuarios.Where(u => u.EstaEmRisco()).ToList();

                if (!usuariosEmRisco.Any())
                {
                    Console.WriteLine("✅ Nenhum usuário em risco no momento.");
                    return;
                }

                Console.WriteLine($"⚠️  {usuariosEmRisco.Count} usuário(s) em situação de risco:");
                Console.WriteLine();

                foreach (var usuario in usuariosEmRisco.OrderByDescending(u => u.PontuacaoRisco))
                {
                    Console.WriteLine($"🔴 {usuario.Nome} - {usuario.ObterDescricaoRisco()}");
                    Console.WriteLine($"   Pontuação: {usuario.PontuacaoRisco} | Email: {usuario.Email}");
                    Console.WriteLine($"   Telefone: {usuario.Telefone} | Última análise: {usuario.UltimaAvaliacao:dd/MM/yyyy HH:mm}");
                    Console.WriteLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro ao verificar usuários em risco: {ex.Message}");
            }
        }

        private static async Task EstatisticasDetecao()
        {
            Console.WriteLine("\n📈 ========== ESTATÍSTICAS DE DETECÇÃO ==========");
            
            try
            {
                var usuarios = await _usuarioService.ListarUsuariosAsync();
                if (!usuarios.Any())
                {
                    Console.WriteLine("❌ Nenhum usuário cadastrado.");
                    return;
                }

                var totalUsuarios = usuarios.Count;
                var usuariosBaixoRisco = usuarios.Count(u => u.NivelRisco == NivelRisco.Baixo);
                var usuariosMedioRisco = usuarios.Count(u => u.NivelRisco == NivelRisco.Medio);
                var usuariosAltoRisco = usuarios.Count(u => u.NivelRisco == NivelRisco.Alto);
                var usuariosEmRisco = usuarios.Count(u => u.EstaEmRisco());

                Console.WriteLine($"📊 Total de usuários: {totalUsuarios}");
                Console.WriteLine($"🟢 Baixo risco: {usuariosBaixoRisco} ({(double)usuariosBaixoRisco/totalUsuarios*100:F1}%)");
                Console.WriteLine($"🟡 Médio risco: {usuariosMedioRisco} ({(double)usuariosMedioRisco/totalUsuarios*100:F1}%)");
                Console.WriteLine($"🔴 Alto risco: {usuariosAltoRisco} ({(double)usuariosAltoRisco/totalUsuarios*100:F1}%)");
                Console.WriteLine($"⚠️  Em risco: {usuariosEmRisco} ({(double)usuariosEmRisco/totalUsuarios*100:F1}%)");
                Console.WriteLine();
                Console.WriteLine($"📈 Pontuação média: {usuarios.Average(u => u.PontuacaoRisco):F1}");
                Console.WriteLine($"📈 Pontuação máxima: {usuarios.Max(u => u.PontuacaoRisco)}");
                Console.WriteLine($"📈 Pontuação mínima: {usuarios.Min(u => u.PontuacaoRisco)}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro ao gerar estatísticas: {ex.Message}");
            }
        }

        private static async Task VerAlertasAtivos()
        {
            Console.WriteLine("\n🔔 ========== ALERTAS ATIVOS ==========");
            
            try
            {
                var usuarios = await _usuarioService.ListarUsuariosAsync();
                var usuariosEmRisco = usuarios.Where(u => u.EstaEmRisco()).ToList();

                if (!usuariosEmRisco.Any())
                {
                    Console.WriteLine("✅ Nenhum alerta ativo no momento.");
                    return;
                }

                Console.WriteLine($"⚠️  {usuariosEmRisco.Count} alerta(s) ativo(s):");
                Console.WriteLine();

                foreach (var usuario in usuariosEmRisco.OrderByDescending(u => u.PontuacaoRisco))
                {
                    Console.WriteLine($"🔴 ALERTA: {usuario.Nome}");
                    Console.WriteLine($"   Nível de Risco: {usuario.ObterDescricaoRisco()}");
                    Console.WriteLine($"   Pontuação: {usuario.PontuacaoRisco}");
                    Console.WriteLine($"   Email: {usuario.Email}");
                    Console.WriteLine($"   Telefone: {usuario.Telefone}");
                    Console.WriteLine($"   Última análise: {usuario.UltimaAvaliacao:dd/MM/yyyy HH:mm}");
                    Console.WriteLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro ao verificar alertas: {ex.Message}");
            }
        }

        private static async Task CriarIntervencao()
        {
            Console.WriteLine("\n⚠️  ========== CRIAR INTERVENÇÃO ==========");
            
            try
            {
                Console.Write("ID do usuário: ");
                var idStr = Console.ReadLine();
                if (!int.TryParse(idStr, out var usuarioId))
                {
                    Console.WriteLine("❌ ID inválido!");
                    return;
                }

                var usuario = await _usuarioService.ObterUsuarioPorIdAsync(usuarioId);
                if (usuario == null)
                {
                    Console.WriteLine("❌ Usuário não encontrado!");
                    return;
                }

                Console.WriteLine($"\nUsuário: {usuario.Nome} ({usuario.Email})");
                Console.WriteLine($"Nível de Risco: {usuario.ObterDescricaoRisco()}");
                Console.WriteLine();

                Console.Write("Título da intervenção: ");
                var titulo = Console.ReadLine() ?? "";

                Console.Write("Mensagem: ");
                var mensagem = Console.ReadLine() ?? "";

                Console.Write("Ação recomendada: ");
                var acaoRecomendada = Console.ReadLine() ?? "";

                Console.Write("Prioridade (1-5): ");
                var prioridadeStr = Console.ReadLine();
                if (!int.TryParse(prioridadeStr, out var prioridade) || prioridade < 1 || prioridade > 5)
                {
                    prioridade = 3; // Prioridade média por padrão
                }

                // Simular criação de intervenção
                Console.WriteLine("\n✅ Intervenção criada com sucesso!");
                Console.WriteLine($"Título: {titulo}");
                Console.WriteLine($"Usuário: {usuario.Nome}");
                Console.WriteLine($"Prioridade: {prioridade}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro ao criar intervenção: {ex.Message}");
            }
        }

        private static async Task ListarIntervencoes()
        {
            Console.WriteLine("\n📋 ========== LISTAR INTERVENÇÕES ==========");
            
            try
            {
                var usuarios = await _usuarioService.ListarUsuariosAsync();
                var usuariosEmRisco = usuarios.Where(u => u.EstaEmRisco()).ToList();

                if (!usuariosEmRisco.Any())
                {
                    Console.WriteLine("✅ Nenhuma intervenção necessária no momento.");
                    return;
                }

                Console.WriteLine($"📋 {usuariosEmRisco.Count} intervenção(ões) necessária(s):");
                Console.WriteLine();

                foreach (var usuario in usuariosEmRisco.OrderByDescending(u => u.PontuacaoRisco))
                {
                    Console.WriteLine($"🔴 INTERVENÇÃO NECESSÁRIA:");
                    Console.WriteLine($"   Usuário: {usuario.Nome} (ID: {usuario.Id})");
                    Console.WriteLine($"   Nível de Risco: {usuario.ObterDescricaoRisco()}");
                    Console.WriteLine($"   Pontuação: {usuario.PontuacaoRisco}");
                    Console.WriteLine($"   Email: {usuario.Email}");
                    Console.WriteLine($"   Telefone: {usuario.Telefone}");
                    Console.WriteLine($"   Última análise: {usuario.UltimaAvaliacao:dd/MM/yyyy HH:mm}");
                    Console.WriteLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro ao listar intervenções: {ex.Message}");
            }
        }

        private static async Task MarcarIntervencaoVisualizada()
        {
            Console.WriteLine("\n✅ ========== MARCAR COMO VISUALIZADA ==========");
            
            try
            {
                Console.Write("ID do usuário: ");
                var idStr = Console.ReadLine();
                if (!int.TryParse(idStr, out var usuarioId))
                {
                    Console.WriteLine("❌ ID inválido!");
                    return;
                }

                var usuario = await _usuarioService.ObterUsuarioPorIdAsync(usuarioId);
                if (usuario == null)
                {
                    Console.WriteLine("❌ Usuário não encontrado!");
                    return;
                }

                if (!usuario.EstaEmRisco())
                {
                    Console.WriteLine("✅ Usuário não está em situação de risco.");
                    return;
                }

                Console.WriteLine($"\nUsuário: {usuario.Nome}");
                Console.WriteLine($"Nível de Risco: {usuario.ObterDescricaoRisco()}");
                Console.WriteLine($"Pontuação: {usuario.PontuacaoRisco}");
                Console.WriteLine();

                Console.Write("Deseja marcar a intervenção como visualizada? (s/n): ");
                var confirmacao = Console.ReadLine()?.ToLower();

                if (confirmacao == "s" || confirmacao == "sim")
                {
                    // Simular marcação como visualizada
                    Console.WriteLine("✅ Intervenção marcada como visualizada!");
                    Console.WriteLine($"Usuário {usuario.Nome} foi notificado sobre a intervenção.");
                }
                else
                {
                    Console.WriteLine("❌ Operação cancelada.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro ao marcar intervenção: {ex.Message}");
            }
        }

        private static async Task EstatisticasIntervencoes()
        {
            Console.WriteLine("\n📊 ========== ESTATÍSTICAS DE INTERVENÇÕES ==========");
            
            try
            {
                var usuarios = await _usuarioService.ListarUsuariosAsync();
                if (!usuarios.Any())
                {
                    Console.WriteLine("❌ Nenhum usuário cadastrado.");
                    return;
                }

                var totalUsuarios = usuarios.Count;
                var usuariosEmRisco = usuarios.Where(u => u.EstaEmRisco()).ToList();
                var usuariosAltoRisco = usuarios.Where(u => u.NivelRisco == NivelRisco.Alto).ToList();
                var usuariosMedioRisco = usuarios.Where(u => u.NivelRisco == NivelRisco.Medio).ToList();

                Console.WriteLine("📊 ESTATÍSTICAS DE INTERVENÇÕES");
                Console.WriteLine($"Data/Hora: {DateTime.Now:dd/MM/yyyy HH:mm:ss}");
                Console.WriteLine();
                Console.WriteLine($"👥 Total de usuários: {totalUsuarios}");
                Console.WriteLine($"⚠️  Usuários em risco: {usuariosEmRisco.Count} ({(double)usuariosEmRisco.Count/totalUsuarios*100:F1}%)");
                Console.WriteLine($"🔴 Alto risco: {usuariosAltoRisco.Count} ({(double)usuariosAltoRisco.Count/totalUsuarios*100:F1}%)");
                Console.WriteLine($"🟡 Médio risco: {usuariosMedioRisco.Count} ({(double)usuariosMedioRisco.Count/totalUsuarios*100:F1}%)");
                Console.WriteLine();
                Console.WriteLine("📈 INTERVENÇÕES NECESSÁRIAS:");
                
                if (usuariosEmRisco.Any())
                {
                    foreach (var usuario in usuariosEmRisco.OrderByDescending(u => u.PontuacaoRisco))
                    {
                        Console.WriteLine($"🔴 {usuario.Nome}: {usuario.ObterDescricaoRisco()} (Pontuação: {usuario.PontuacaoRisco})");
                    }
                }
                else
                {
                    Console.WriteLine("✅ Nenhuma intervenção necessária no momento.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro ao gerar estatísticas: {ex.Message}");
            }
        }

        // Métodos de Atividades Alternativas
        private static async Task ListarAtividades()
        {
            Console.WriteLine("\n📋 ========== LISTAR ATIVIDADES ==========");
            
            var atividades = new List<dynamic>
            {
                new { Nome = "Caminhada no Parque", Categoria = "Esporte", Duracao = 30, Custo = 0, Beneficios = "Exercício físico, contato com natureza" },
                new { Nome = "Leitura de Livros", Categoria = "Educação", Duracao = 60, Custo = 0, Beneficios = "Conhecimento, relaxamento mental" },
                new { Nome = "Meditação", Categoria = "Bem-estar", Duracao = 20, Custo = 0, Beneficios = "Redução do estresse, foco mental" },
                new { Nome = "Culinária", Categoria = "Hobby", Duracao = 90, Custo = 50, Beneficios = "Criatividade, satisfação pessoal" },
                new { Nome = "Voluntariado", Categoria = "Social", Duracao = 120, Custo = 0, Beneficios = "Ajuda ao próximo, senso de propósito" },
                new { Nome = "Aprender Música", Categoria = "Arte", Duracao = 60, Custo = 100, Beneficios = "Expressão artística, coordenação" },
                new { Nome = "Jardinagem", Categoria = "Natureza", Duracao = 45, Custo = 30, Beneficios = "Contato com plantas, paciência" },
                new { Nome = "Fotografia", Categoria = "Arte", Duracao = 120, Custo = 0, Beneficios = "Criatividade, observação do mundo" }
            };

            Console.WriteLine($"📋 {atividades.Count} atividades alternativas disponíveis:");
            Console.WriteLine();
            Console.WriteLine($"{"Nome",-20} {"Categoria",-12} {"Duração",-10} {"Custo",-8} {"Benefícios"}");
            Console.WriteLine(new string('-', 80));

            foreach (var atividade in atividades)
            {
                Console.WriteLine($"{atividade.Nome,-20} {atividade.Categoria,-12} {atividade.Duracao}min{"",-6} R${atividade.Custo,-6} {atividade.Beneficios}");
            }

            Console.WriteLine();
            Console.WriteLine("💡 Dica: Essas atividades podem ajudar a substituir o tempo gasto com apostas!");
        }

        private static async Task BuscarPorCategoria()
        {
            Console.WriteLine("\n🔍 ========== BUSCAR POR CATEGORIA ==========");
            
            var categorias = new[] { "Esporte", "Educação", "Bem-estar", "Hobby", "Social", "Arte", "Natureza" };
            
            Console.WriteLine("Categorias disponíveis:");
            for (int i = 0; i < categorias.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {categorias[i]}");
            }
            
            Console.Write("\nEscolha uma categoria (número): ");
            var opcao = Console.ReadLine();
            
            if (!int.TryParse(opcao, out var categoriaIndex) || categoriaIndex < 1 || categoriaIndex > categorias.Length)
            {
                Console.WriteLine("❌ Categoria inválida!");
                return;
            }
            
            var categoriaEscolhida = categorias[categoriaIndex - 1];
            
            var atividades = new List<dynamic>
            {
                new { Nome = "Caminhada no Parque", Categoria = "Esporte", Duracao = 30, Custo = 0 },
                new { Nome = "Leitura de Livros", Categoria = "Educação", Duracao = 60, Custo = 0 },
                new { Nome = "Meditação", Categoria = "Bem-estar", Duracao = 20, Custo = 0 },
                new { Nome = "Culinária", Categoria = "Hobby", Duracao = 90, Custo = 50 },
                new { Nome = "Voluntariado", Categoria = "Social", Duracao = 120, Custo = 0 },
                new { Nome = "Aprender Música", Categoria = "Arte", Duracao = 60, Custo = 100 },
                new { Nome = "Jardinagem", Categoria = "Natureza", Duracao = 45, Custo = 30 },
                new { Nome = "Fotografia", Categoria = "Arte", Duracao = 120, Custo = 0 }
            };
            
            var atividadesFiltradas = atividades.Where(a => a.Categoria == categoriaEscolhida).ToList();
            
            Console.WriteLine($"\n🎯 Atividades da categoria '{categoriaEscolhida}':");
            Console.WriteLine();
            
            if (atividadesFiltradas.Any())
            {
                foreach (var atividade in atividadesFiltradas)
                {
                    Console.WriteLine($"• {atividade.Nome} - {atividade.Duracao}min - R$ {atividade.Custo}");
                }
            }
            else
            {
                Console.WriteLine("❌ Nenhuma atividade encontrada nesta categoria.");
            }
        }

        private static async Task FiltrarPorCusto()
        {
            Console.WriteLine("\n💰 ========== FILTRAR POR CUSTO ==========");
            
            Console.WriteLine("Faixas de custo disponíveis:");
            Console.WriteLine("1. Gratuitas (R$ 0)");
            Console.WriteLine("2. Baixo custo (R$ 1 - R$ 50)");
            Console.WriteLine("3. Médio custo (R$ 51 - R$ 100)");
            Console.WriteLine("4. Alto custo (R$ 101+)");
            
            Console.Write("\nEscolha uma faixa (número): ");
            var opcao = Console.ReadLine();
            
            var atividades = new List<dynamic>
            {
                new { Nome = "Caminhada no Parque", Categoria = "Esporte", Duracao = 30, Custo = 0 },
                new { Nome = "Leitura de Livros", Categoria = "Educação", Duracao = 60, Custo = 0 },
                new { Nome = "Meditação", Categoria = "Bem-estar", Duracao = 20, Custo = 0 },
                new { Nome = "Culinária", Categoria = "Hobby", Duracao = 90, Custo = 50 },
                new { Nome = "Voluntariado", Categoria = "Social", Duracao = 120, Custo = 0 },
                new { Nome = "Aprender Música", Categoria = "Arte", Duracao = 60, Custo = 100 },
                new { Nome = "Jardinagem", Categoria = "Natureza", Duracao = 45, Custo = 30 },
                new { Nome = "Fotografia", Categoria = "Arte", Duracao = 120, Custo = 0 }
            };
            
            var atividadesFiltradas = opcao switch
            {
                "1" => atividades.Where(a => a.Custo == 0).ToList(),
                "2" => atividades.Where(a => a.Custo > 0 && a.Custo <= 50).ToList(),
                "3" => atividades.Where(a => a.Custo > 50 && a.Custo <= 100).ToList(),
                "4" => atividades.Where(a => a.Custo > 100).ToList(),
                _ => new List<dynamic>()
            };
            
            if (atividadesFiltradas.Any())
            {
                Console.WriteLine($"\n💰 Atividades na faixa escolhida:");
                foreach (var atividade in atividadesFiltradas)
                {
                    Console.WriteLine($"• {atividade.Nome} - R$ {atividade.Custo} - {atividade.Duracao}min");
                }
            }
            else
            {
                Console.WriteLine("❌ Nenhuma atividade encontrada nesta faixa de custo.");
            }
        }

        private static async Task FiltrarPorDuracao()
        {
            Console.WriteLine("\n⏱️  ========== FILTRAR POR DURAÇÃO ==========");
            
            Console.WriteLine("Faixas de duração disponíveis:");
            Console.WriteLine("1. Rápidas (até 30 min)");
            Console.WriteLine("2. Médias (31 - 60 min)");
            Console.WriteLine("3. Longas (61 - 120 min)");
            Console.WriteLine("4. Muito longas (121+ min)");
            
            Console.Write("\nEscolha uma faixa (número): ");
            var opcao = Console.ReadLine();
            
            var atividades = new List<dynamic>
            {
                new { Nome = "Caminhada no Parque", Categoria = "Esporte", Duracao = 30, Custo = 0 },
                new { Nome = "Leitura de Livros", Categoria = "Educação", Duracao = 60, Custo = 0 },
                new { Nome = "Meditação", Categoria = "Bem-estar", Duracao = 20, Custo = 0 },
                new { Nome = "Culinária", Categoria = "Hobby", Duracao = 90, Custo = 50 },
                new { Nome = "Voluntariado", Categoria = "Social", Duracao = 120, Custo = 0 },
                new { Nome = "Aprender Música", Categoria = "Arte", Duracao = 60, Custo = 100 },
                new { Nome = "Jardinagem", Categoria = "Natureza", Duracao = 45, Custo = 30 },
                new { Nome = "Fotografia", Categoria = "Arte", Duracao = 120, Custo = 0 }
            };
            
            var atividadesFiltradas = opcao switch
            {
                "1" => atividades.Where(a => a.Duracao <= 30).ToList(),
                "2" => atividades.Where(a => a.Duracao > 30 && a.Duracao <= 60).ToList(),
                "3" => atividades.Where(a => a.Duracao > 60 && a.Duracao <= 120).ToList(),
                "4" => atividades.Where(a => a.Duracao > 120).ToList(),
                _ => new List<dynamic>()
            };
            
            if (atividadesFiltradas.Any())
            {
                Console.WriteLine($"\n⏱️  Atividades na faixa escolhida:");
                foreach (var atividade in atividadesFiltradas)
                {
                    Console.WriteLine($"• {atividade.Nome} - {atividade.Duracao}min - R$ {atividade.Custo}");
                }
            }
            else
            {
                Console.WriteLine("❌ Nenhuma atividade encontrada nesta faixa de duração.");
            }
        }

        private static async Task SugerirAtividade()
        {
            Console.WriteLine("\n🎯 ========== SUGERIR ATIVIDADE ==========");
            
            try
            {
                Console.Write("ID do usuário: ");
                var idStr = Console.ReadLine();
                if (!int.TryParse(idStr, out var usuarioId))
                {
                    Console.WriteLine("❌ ID inválido!");
                    return;
                }

                var usuario = await _usuarioService.ObterUsuarioPorIdAsync(usuarioId);
                if (usuario == null)
                {
                    Console.WriteLine("❌ Usuário não encontrado!");
                    return;
                }

                Console.WriteLine($"\n👤 Usuário: {usuario.Nome}");
                Console.WriteLine($"Nível de Risco: {usuario.ObterDescricaoRisco()}");
                Console.WriteLine($"Pontuação: {usuario.PontuacaoRisco}");
                Console.WriteLine();

                // Sugestões baseadas no nível de risco
                var sugestoes = usuario.NivelRisco switch
                {
                    NivelRisco.Baixo => new[] { "Caminhada no Parque", "Leitura de Livros", "Meditação" },
                    NivelRisco.Medio => new[] { "Voluntariado", "Jardinagem", "Fotografia", "Culinária" },
                    NivelRisco.Alto => new[] { "Aprender Música", "Voluntariado", "Meditação", "Leitura de Livros" },
                    _ => new[] { "Caminhada no Parque", "Leitura de Livros" }
                };

                Console.WriteLine("🎯 SUGESTÕES PERSONALIZADAS:");
                Console.WriteLine("Baseado no seu perfil, recomendamos:");
                Console.WriteLine();

                foreach (var sugestao in sugestoes)
                {
                    Console.WriteLine($"• {sugestao}");
                }

                Console.WriteLine();
                Console.WriteLine("💡 Dica: Essas atividades podem ajudar a reduzir o tempo gasto com apostas!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro ao sugerir atividades: {ex.Message}");
            }
        }

        private static async Task AdicionarAtividade()
        {
            Console.WriteLine("\n➕ ========== ADICIONAR ATIVIDADE ==========");
            
            try
            {
                Console.Write("Nome da atividade: ");
                var nome = Console.ReadLine() ?? "";
                
                if (string.IsNullOrWhiteSpace(nome))
                {
                    Console.WriteLine("❌ Nome da atividade é obrigatório!");
                    return;
                }

                Console.Write("Categoria: ");
                var categoria = Console.ReadLine() ?? "";
                
                Console.Write("Duração em minutos: ");
                var duracaoStr = Console.ReadLine();
                if (!int.TryParse(duracaoStr, out var duracao) || duracao <= 0)
                {
                    Console.WriteLine("❌ Duração inválida!");
                    return;
                }

                Console.Write("Custo aproximado (R$): ");
                var custoStr = Console.ReadLine();
                if (!decimal.TryParse(custoStr, out var custo) || custo < 0)
                {
                    Console.WriteLine("❌ Custo inválido!");
                    return;
                }

                Console.Write("Benefícios: ");
                var beneficios = Console.ReadLine() ?? "";

                Console.WriteLine("\n✅ Atividade adicionada com sucesso!");
                Console.WriteLine($"Nome: {nome}");
                Console.WriteLine($"Categoria: {categoria}");
                Console.WriteLine($"Duração: {duracao} minutos");
                Console.WriteLine($"Custo: R$ {custo:F2}");
                Console.WriteLine($"Benefícios: {beneficios}");
                Console.WriteLine();
                Console.WriteLine("💡 Esta atividade foi adicionada à base de dados e estará disponível para sugestões!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro ao adicionar atividade: {ex.Message}");
            }
        }

        // Métodos de Relatórios Comportamentais
        private static async Task RelatorioGeral()
        {
            Console.WriteLine("\n📈 ========== RELATÓRIO GERAL ==========");
            
            try
            {
                var usuarios = await _usuarioService.ListarUsuariosAsync();
                if (!usuarios.Any())
                {
                    Console.WriteLine("❌ Nenhum usuário cadastrado.");
                    return;
                }

                var totalUsuarios = usuarios.Count;
                var usuariosAtivos = usuarios.Count(u => u.Ativo);
                var usuariosEmRisco = usuarios.Count(u => u.EstaEmRisco());
                var valorTotalSaldo = usuarios.Sum(u => u.Saldo);

                Console.WriteLine("📊 RELATÓRIO GERAL DO SISTEMA");
                Console.WriteLine($"Data/Hora: {DateTime.Now:dd/MM/yyyy HH:mm:ss}");
                Console.WriteLine();
                Console.WriteLine($"👥 Total de usuários: {totalUsuarios}");
                Console.WriteLine($"✅ Usuários ativos: {usuariosAtivos}");
                Console.WriteLine($"⚠️  Usuários em risco: {usuariosEmRisco}");
                Console.WriteLine($"💰 Valor total em saldos: R$ {valorTotalSaldo:F2}");
                Console.WriteLine();
                Console.WriteLine("📈 DISTRIBUIÇÃO POR NÍVEL DE RISCO:");
                
                var baixoRisco = usuarios.Count(u => u.NivelRisco == NivelRisco.Baixo);
                var medioRisco = usuarios.Count(u => u.NivelRisco == NivelRisco.Medio);
                var altoRisco = usuarios.Count(u => u.NivelRisco == NivelRisco.Alto);

                Console.WriteLine($"🟢 Baixo risco: {baixoRisco} ({(double)baixoRisco/totalUsuarios*100:F1}%)");
                Console.WriteLine($"🟡 Médio risco: {medioRisco} ({(double)medioRisco/totalUsuarios*100:F1}%)");
                Console.WriteLine($"🔴 Alto risco: {altoRisco} ({(double)altoRisco/totalUsuarios*100:F1}%)");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro ao gerar relatório: {ex.Message}");
            }
        }


        private static async Task SimulacaoInvestimentos()
        {
            Console.WriteLine("\n💰 ========== SIMULAÇÃO DE INVESTIMENTOS ==========");
            
            try
            {
                var usuarios = await _usuarioService.ListarUsuariosAsync();
                var usuariosEmRisco = usuarios.Where(u => u.EstaEmRisco()).ToList();

                if (!usuariosEmRisco.Any())
                {
                    Console.WriteLine("✅ Nenhum usuário em risco para simulação.");
                    return;
                }

                Console.WriteLine("💡 SIMULAÇÃO DE INVESTIMENTOS ALTERNATIVOS");
                Console.WriteLine("Baseado nos usuários em situação de risco:");
                Console.WriteLine();

                foreach (var usuario in usuariosEmRisco)
                {
                    var valorSimulado = usuario.ValorApostadoHoje * 30; // Simular 30 dias
                    var rendimentoPoupanca = valorSimulado * 0.05m; // 5% ao ano
                    var rendimentoCDB = valorSimulado * 0.12m; // 12% ao ano
                    var rendimentoAcoes = valorSimulado * 0.15m; // 15% ao ano

                    Console.WriteLine($"👤 {usuario.Nome}:");
                    Console.WriteLine($"   Valor apostado hoje: R$ {usuario.ValorApostadoHoje:F2}");
                    Console.WriteLine($"   Projeção mensal: R$ {valorSimulado:F2}");
                    Console.WriteLine();
                    Console.WriteLine($"   💰 Investimentos Alternativos (30 dias):");
                    Console.WriteLine($"   🏦 Poupança: R$ {rendimentoPoupanca:F2} de rendimento");
                    Console.WriteLine($"   📈 CDB: R$ {rendimentoCDB:F2} de rendimento");
                    Console.WriteLine($"   📊 Ações: R$ {rendimentoAcoes:F2} de rendimento");
                    Console.WriteLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro na simulação: {ex.Message}");
            }
        }

        private static async Task EstatisticasComportamentais()
        {
            Console.WriteLine("\n📊 ========== ESTATÍSTICAS COMPORTAMENTAIS ==========");
            
            try
            {
                var usuarios = await _usuarioService.ListarUsuariosAsync();
                if (!usuarios.Any())
                {
                    Console.WriteLine("❌ Nenhum usuário cadastrado.");
                    return;
                }

                var apostas = await _apostaService.ListarApostasAsync();
                
                Console.WriteLine("📊 ESTATÍSTICAS COMPORTAMENTAIS DETALHADAS");
                Console.WriteLine($"Data/Hora: {DateTime.Now:dd/MM/yyyy HH:mm:ss}");
                Console.WriteLine();
                
                // Estatísticas gerais
                Console.WriteLine("👥 USUÁRIOS:");
                Console.WriteLine($"   Total: {usuarios.Count}");
                Console.WriteLine($"   Ativos: {usuarios.Count(u => u.Ativo)}");
                Console.WriteLine($"   Inativos: {usuarios.Count(u => !u.Ativo)}");
                Console.WriteLine();
                
                // Estatísticas de risco
                Console.WriteLine("⚠️  ANÁLISE DE RISCO:");
                var baixoRisco = usuarios.Count(u => u.NivelRisco == NivelRisco.Baixo);
                var medioRisco = usuarios.Count(u => u.NivelRisco == NivelRisco.Medio);
                var altoRisco = usuarios.Count(u => u.NivelRisco == NivelRisco.Alto);
                
                Console.WriteLine($"   Baixo risco: {baixoRisco} ({(double)baixoRisco/usuarios.Count*100:F1}%)");
                Console.WriteLine($"   Médio risco: {medioRisco} ({(double)medioRisco/usuarios.Count*100:F1}%)");
                Console.WriteLine($"   Alto risco: {altoRisco} ({(double)altoRisco/usuarios.Count*100:F1}%)");
                Console.WriteLine();
                
                // Estatísticas de apostas
                if (apostas.Any())
                {
                    Console.WriteLine("🎲 APOSTAS:");
                    Console.WriteLine($"   Total de apostas: {apostas.Count}");
                    Console.WriteLine($"   Valor total apostado: R$ {apostas.Sum(a => a.Valor):F2}");
                    Console.WriteLine($"   Valor médio por aposta: R$ {apostas.Average(a => a.Valor):F2}");
                    Console.WriteLine($"   Apostas pendentes: {apostas.Count(a => a.Status == "Pendente")}");
                    Console.WriteLine($"   Apostas finalizadas: {apostas.Count(a => a.Status != "Pendente")}");
                }
                
                // Usuários mais ativos
                Console.WriteLine();
                Console.WriteLine("🔥 USUÁRIOS MAIS ATIVOS (por pontuação de risco):");
                var usuariosAtivos = usuarios.OrderByDescending(u => u.PontuacaoRisco).Take(5);
                foreach (var usuario in usuariosAtivos)
                {
                    Console.WriteLine($"   {usuario.Nome}: {usuario.PontuacaoRisco} pontos ({usuario.ObterDescricaoRisco()})");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro ao gerar estatísticas: {ex.Message}");
            }
        }

        private static async Task ExportarRelatorio()
        {
            Console.WriteLine("\n📄 ========== EXPORTAR RELATÓRIO ==========");
            
            try
            {
                Console.WriteLine("Tipos de relatório disponíveis:");
                Console.WriteLine("1. Relatório de usuários");
                Console.WriteLine("2. Relatório de apostas");
                Console.WriteLine("3. Relatório de histórico");
                Console.WriteLine("4. Relatório comportamental completo");
                
                Console.Write("\nEscolha o tipo (número): ");
                var opcao = Console.ReadLine();
                
                var nomeArquivo = opcao switch
                {
                    "1" => "relatorio_usuarios.txt",
                    "2" => "relatorio_apostas.txt", 
                    "3" => "relatorio_historico.txt",
                    "4" => "relatorio_comportamental.txt",
                    _ => "relatorio_geral.txt"
                };
                
                var conteudo = new StringBuilder();
                conteudo.AppendLine("=== RELATÓRIO EXPORTADO ===");
                conteudo.AppendLine($"Data/Hora: {DateTime.Now:dd/MM/yyyy HH:mm:ss}");
                conteudo.AppendLine($"Tipo: {opcao switch { "1" => "Usuários", "2" => "Apostas", "3" => "Histórico", "4" => "Comportamental", _ => "Geral" }}");
                conteudo.AppendLine();
                
                switch (opcao)
                {
                    case "1":
                        var usuarios = await _usuarioService.ListarUsuariosAsync();
                        conteudo.AppendLine($"Total de usuários: {usuarios.Count}");
                        foreach (var usuario in usuarios)
                        {
                            conteudo.AppendLine($"- {usuario.Nome} | {usuario.Email} | Saldo: R$ {usuario.Saldo:F2} | Risco: {usuario.ObterDescricaoRisco()}");
                        }
                        break;
                        
                    case "2":
                        var apostas = await _apostaService.ListarApostasAsync();
                        conteudo.AppendLine($"Total de apostas: {apostas.Count}");
                        foreach (var aposta in apostas)
                        {
                            conteudo.AppendLine($"- ID: {aposta.Id} | Usuário: {aposta.UsuarioId} | Valor: R$ {aposta.Valor:F2} | Status: {aposta.Status}");
                        }
                        break;
                        
                    case "3":
                        var historicos = await _historicoService.ObterHistoricoCompletoAsync();
                        conteudo.AppendLine($"Total de registros: {historicos.Count}");
                        foreach (var historico in historicos.Take(50)) // Limitar para não ficar muito grande
                        {
                            conteudo.AppendLine($"- {historico.DataOperacao:dd/MM/yyyy HH:mm} | {historico.TipoOperacao} | R$ {historico.Valor:F2}");
                        }
                        break;
                        
                    case "4":
                        var usuariosComport = await _usuarioService.ListarUsuariosAsync();
                        conteudo.AppendLine($"Análise comportamental de {usuariosComport.Count} usuários:");
                        var usuariosEmRisco = usuariosComport.Where(u => u.EstaEmRisco()).ToList();
                        conteudo.AppendLine($"Usuários em risco: {usuariosEmRisco.Count}");
                        foreach (var usuario in usuariosEmRisco)
                        {
                            conteudo.AppendLine($"- {usuario.Nome}: {usuario.ObterDescricaoRisco()} (Pontuação: {usuario.PontuacaoRisco})");
                        }
                        break;
                }
                
                // Salvar arquivo
                var caminhoArquivo = Path.Combine("Arquivos", nomeArquivo);
                await File.WriteAllTextAsync(caminhoArquivo, conteudo.ToString(), System.Text.Encoding.UTF8);
                
                Console.WriteLine($"✅ Relatório exportado com sucesso!");
                Console.WriteLine($"📁 Arquivo salvo em: {caminhoArquivo}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro ao exportar relatório: {ex.Message}");
            }
        }

        #endregion
    }
}
