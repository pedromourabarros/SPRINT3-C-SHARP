using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApostasCompulsivas.Models;
using ApostasCompulsivas.Repository;

namespace ApostasCompulsivas.Services
{
    /// <summary>
    /// Serviço responsável pelas atividades alternativas
    /// </summary>
    public class AtividadeAlternativaService : IAtividadeAlternativaService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IApostaRepository _apostaRepository;

        public AtividadeAlternativaService(IUsuarioRepository usuarioRepository, IApostaRepository apostaRepository)
        {
            _usuarioRepository = usuarioRepository;
            _apostaRepository = apostaRepository;
        }

        public async Task<List<AtividadeAlternativa>> ListarAtividadesAsync()
        {
            // Retornar atividades pré-cadastradas
            return await ObterAtividadesPreCadastradas();
        }

        public async Task<List<AtividadeAlternativa>> ListarAtividadesPorCategoriaAsync(CategoriaAtividade categoria)
        {
            var atividades = await ListarAtividadesAsync();
            return atividades.Where(a => a.Categoria == categoria).ToList();
        }

        public async Task<List<AtividadeAlternativa>> ListarAtividadesPorCustoAsync(decimal custoMaximo)
        {
            var atividades = await ListarAtividadesAsync();
            return atividades.Where(a => a.CustoAproximado <= custoMaximo).ToList();
        }

        public async Task<List<AtividadeAlternativa>> ListarAtividadesPorDuracaoAsync(int duracaoMaxima)
        {
            var atividades = await ListarAtividadesAsync();
            return atividades.Where(a => a.DuracaoMinutos <= duracaoMaxima).ToList();
        }

        public async Task<List<AtividadeAlternativa>> ListarAtividadesPorDificuldadeAsync(int nivelMaximo)
        {
            var atividades = await ListarAtividadesAsync();
            return atividades.Where(a => a.NivelDificuldade <= nivelMaximo).ToList();
        }

        public async Task<AtividadeAlternativa> SugerirAtividadePersonalizadaAsync(int usuarioId)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(usuarioId);
            if (usuario == null) return null;

            var atividades = await ListarAtividadesAsync();
            
            // Filtrar por perfil do usuário
            var atividadesFiltradas = atividades.Where(a => a.Ativa).ToList();

            // Se usuário tem pouco dinheiro, priorizar atividades gratuitas
            if (usuario.Saldo < 100)
            {
                atividadesFiltradas = atividadesFiltradas.Where(a => a.CustoAproximado == 0).ToList();
            }

            // Se usuário está em alto risco, priorizar atividades relaxantes
            if (usuario.NivelRisco == NivelRisco.Alto)
            {
                atividadesFiltradas = atividadesFiltradas
                    .Where(a => a.Categoria == CategoriaAtividade.Relaxamento || 
                               a.Categoria == CategoriaAtividade.Exercicio)
                    .ToList();
            }

            // Retornar atividade aleatória das filtradas
            if (atividadesFiltradas.Any())
            {
                var random = new Random();
                return atividadesFiltradas[random.Next(atividadesFiltradas.Count)];
            }

            return atividades.FirstOrDefault();
        }

        public async Task<List<AtividadeAlternativa>> BuscarAtividadesAsync(string termo)
        {
            var atividades = await ListarAtividadesAsync();
            return atividades.Where(a => 
                a.Nome.Contains(termo, StringComparison.OrdinalIgnoreCase) ||
                a.Descricao.Contains(termo, StringComparison.OrdinalIgnoreCase) ||
                a.Beneficios.Contains(termo, StringComparison.OrdinalIgnoreCase)
            ).ToList();
        }

        public async Task<AtividadeAlternativa> ObterAtividadePorIdAsync(int id)
        {
            var atividades = await ListarAtividadesAsync();
            return atividades.FirstOrDefault(a => a.Id == id);
        }

        public async Task<bool> CriarAtividadeAsync(AtividadeAlternativa atividade)
        {
            // Implementar persistência no banco de dados
            return true;
        }

        public async Task<bool> AtualizarAtividadeAsync(AtividadeAlternativa atividade)
        {
            // Implementar atualização no banco de dados
            return true;
        }

        public async Task<bool> DesativarAtividadeAsync(int id)
        {
            // Implementar desativação no banco de dados
            return true;
        }

        public async Task<List<AtividadeAlternativa>> ObterAtividadesRecomendadasAsync(int usuarioId)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(usuarioId);
            if (usuario == null) return new List<AtividadeAlternativa>();

            var atividades = await ListarAtividadesAsync();
            var recomendadas = new List<AtividadeAlternativa>();

            // Recomendar baseado no nível de risco
            switch (usuario.NivelRisco)
            {
                case NivelRisco.Alto:
                    recomendadas.AddRange(atividades.Where(a => 
                        a.Categoria == CategoriaAtividade.Relaxamento ||
                        a.Categoria == CategoriaAtividade.Exercicio ||
                        a.Categoria == CategoriaAtividade.Voluntariado
                    ).Take(3));
                    break;
                case NivelRisco.Medio:
                    recomendadas.AddRange(atividades.Where(a => 
                        a.Categoria == CategoriaAtividade.Esportes ||
                        a.Categoria == CategoriaAtividade.Artes ||
                        a.Categoria == CategoriaAtividade.Social
                    ).Take(3));
                    break;
                default:
                    recomendadas.AddRange(atividades.Take(3));
                    break;
            }

            return recomendadas;
        }

        private async Task<List<AtividadeAlternativa>> ObterAtividadesPreCadastradas()
        {
            return new List<AtividadeAlternativa>
            {
                new AtividadeAlternativa
                {
                    Id = 1,
                    Nome = "Caminhada no Parque",
                    Descricao = "Caminhe por 30 minutos em um parque próximo",
                    Categoria = CategoriaAtividade.Exercicio,
                    DuracaoMinutos = 30,
                    CustoAproximado = 0,
                    Localizacao = "Parque da cidade",
                    RequerEquipamento = false,
                    NivelDificuldade = 1,
                    Beneficios = "Melhora o humor, reduz estresse, fortalece o coração"
                },
                new AtividadeAlternativa
                {
                    Id = 2,
                    Nome = "Meditação",
                    Descricao = "Pratique meditação por 15 minutos",
                    Categoria = CategoriaAtividade.Relaxamento,
                    DuracaoMinutos = 15,
                    CustoAproximado = 0,
                    Localizacao = "Em casa",
                    RequerEquipamento = false,
                    NivelDificuldade = 2,
                    Beneficios = "Reduz ansiedade, melhora concentração, promove bem-estar"
                },
                new AtividadeAlternativa
                {
                    Id = 3,
                    Nome = "Leitura",
                    Descricao = "Leia um livro por 1 hora",
                    Categoria = CategoriaAtividade.Leitura,
                    DuracaoMinutos = 60,
                    CustoAproximado = 0,
                    Localizacao = "Em casa ou biblioteca",
                    RequerEquipamento = false,
                    NivelDificuldade = 1,
                    Beneficios = "Expande conhecimento, melhora vocabulário, reduz estresse"
                },
                new AtividadeAlternativa
                {
                    Id = 4,
                    Nome = "Pintura",
                    Descricao = "Pinte um quadro ou desenhe",
                    Categoria = CategoriaAtividade.Artes,
                    DuracaoMinutos = 90,
                    CustoAproximado = 50,
                    Localizacao = "Em casa",
                    RequerEquipamento = true,
                    EquipamentoNecessario = "Tintas, pincéis, tela",
                    NivelDificuldade = 2,
                    Beneficios = "Expressa criatividade, reduz ansiedade, melhora coordenação"
                },
                new AtividadeAlternativa
                {
                    Id = 5,
                    Nome = "Cozinhar",
                    Descricao = "Prepare uma refeição especial",
                    Categoria = CategoriaAtividade.Hobbies,
                    DuracaoMinutos = 120,
                    CustoAproximado = 80,
                    Localizacao = "Em casa",
                    RequerEquipamento = false,
                    NivelDificuldade = 2,
                    Beneficios = "Desenvolve habilidades, proporciona satisfação, alimenta bem"
                },
                new AtividadeAlternativa
                {
                    Id = 6,
                    Nome = "Trabalho Voluntário",
                    Descricao = "Ajude em uma instituição de caridade",
                    Categoria = CategoriaAtividade.Voluntariado,
                    DuracaoMinutos = 180,
                    CustoAproximado = 0,
                    Localizacao = "Instituições locais",
                    RequerEquipamento = false,
                    NivelDificuldade = 1,
                    Beneficios = "Ajuda outros, proporciona satisfação, desenvolve empatia"
                },
                new AtividadeAlternativa
                {
                    Id = 7,
                    Nome = "Futebol",
                    Descricao = "Jogue futebol com amigos",
                    Categoria = CategoriaAtividade.Esportes,
                    DuracaoMinutos = 90,
                    CustoAproximado = 20,
                    Localizacao = "Campo ou quadra",
                    RequerEquipamento = true,
                    EquipamentoNecessario = "Chuteira, bola",
                    NivelDificuldade = 3,
                    Beneficios = "Melhora condicionamento, socializa, diverte"
                },
                new AtividadeAlternativa
                {
                    Id = 8,
                    Nome = "Música",
                    Descricao = "Toque um instrumento ou cante",
                    Categoria = CategoriaAtividade.Musica,
                    DuracaoMinutos = 60,
                    CustoAproximado = 0,
                    Localizacao = "Em casa",
                    RequerEquipamento = true,
                    EquipamentoNecessario = "Instrumento musical",
                    NivelDificuldade = 3,
                    Beneficios = "Expressa emoções, melhora coordenação, relaxa"
                },
                new AtividadeAlternativa
                {
                    Id = 9,
                    Nome = "Curso Online",
                    Descricao = "Faça um curso online sobre um tema de interesse",
                    Categoria = CategoriaAtividade.Educacao,
                    DuracaoMinutos = 120,
                    CustoAproximado = 100,
                    Localizacao = "Online",
                    RequerEquipamento = true,
                    EquipamentoNecessario = "Computador, internet",
                    NivelDificuldade = 2,
                    Beneficios = "Aprende algo novo, melhora currículo, desenvolve habilidades"
                },
                new AtividadeAlternativa
                {
                    Id = 10,
                    Nome = "Encontro com Amigos",
                    Descricao = "Reúna-se com amigos para conversar",
                    Categoria = CategoriaAtividade.Social,
                    DuracaoMinutos = 120,
                    CustoAproximado = 30,
                    Localizacao = "Café, restaurante ou casa",
                    RequerEquipamento = false,
                    NivelDificuldade = 1,
                    Beneficios = "Fortalece amizades, reduz isolamento, diverte"
                }
            };
        }
    }
}
