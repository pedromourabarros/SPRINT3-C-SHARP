using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApostasCompulsivas.Models;
using ApostasCompulsivas.Repository;

namespace ApostasCompulsivas.Services
{
    /// <summary>
    /// Serviço que gerencia intervenções e alertas para usuários em situação de risco
    /// </summary>
    public class IntervencaoService : IIntervencaoService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IApostaRepository _apostaRepository;
        private readonly IHistoricoRepository _historicoRepository;

        public IntervencaoService(IUsuarioRepository usuarioRepository, IApostaRepository apostaRepository, IHistoricoRepository historicoRepository)
        {
            _usuarioRepository = usuarioRepository;
            _apostaRepository = apostaRepository;
            _historicoRepository = historicoRepository;
        }

        public async Task<Intervencao> CriarIntervencaoAsync(int usuarioId, TipoIntervencao tipo, string titulo, string mensagem, int prioridade = 1)
        {
            var intervencao = new Intervencao(usuarioId, tipo, titulo, mensagem, prioridade);
            
            // Aqui você implementaria a persistência no banco de dados
            // await _intervencaoRepository.CreateAsync(intervencao);
            
            return intervencao;
        }

        public async Task<List<Intervencao>> ObterIntervencoesPendentesAsync(int usuarioId)
        {
            // Implementar busca no banco de dados
            return new List<Intervencao>();
        }

        public async Task<List<Intervencao>> ObterIntervencoesPorTipoAsync(TipoIntervencao tipo)
        {
            // Implementar busca no banco de dados
            return new List<Intervencao>();
        }

        public async Task<bool> MarcarComoVisualizadaAsync(int intervencaoId)
        {
            // Implementar atualização no banco de dados
            return true;
        }

        public async Task<bool> MarcarComoAceitaAsync(int intervencaoId)
        {
            // Implementar atualização no banco de dados
            return true;
        }

        public async Task<List<Intervencao>> ObterIntervencoesUrgentesAsync()
        {
            // Implementar busca no banco de dados
            return new List<Intervencao>();
        }

        public async Task<Intervencao> CriarAlertaComportamentoAsync(int usuarioId, string descricao)
        {
            var titulo = "⚠️ Alerta de Comportamento";
            var mensagem = $"Detectamos um padrão preocupante em suas apostas: {descricao}\n\n" +
                          "Recomendamos que você:\n" +
                          "• Faça uma pausa nas apostas\n" +
                          "• Reflita sobre seus motivos\n" +
                          "• Considere buscar ajuda profissional\n\n" +
                          "Lembre-se: apostas devem ser apenas entretenimento, não uma forma de resolver problemas financeiros.";

            return await CriarIntervencaoAsync(usuarioId, TipoIntervencao.AlertaComportamento, titulo, mensagem, 5);
        }

        public async Task<Intervencao> CriarSugestaoAtividadeAsync(int usuarioId, string atividade)
        {
            var titulo = "🎯 Sugestão de Atividade Alternativa";
            var mensagem = $"Que tal experimentar uma atividade diferente?\n\n" +
                          $"Sugerimos: {atividade}\n\n" +
                          "Atividades alternativas podem:\n" +
                          "• Reduzir o estresse\n" +
                          "• Proporcionar satisfação duradoura\n" +
                          "• Melhorar sua qualidade de vida\n" +
                          "• Ajudar a controlar impulsos de apostas";

            return await CriarIntervencaoAsync(usuarioId, TipoIntervencao.SugestaoAtividade, titulo, mensagem, 3);
        }

        public async Task<Intervencao> CriarLimiteApostasAsync(int usuarioId, decimal limite)
        {
            var titulo = "💰 Limite de Apostas Sugerido";
            var mensagem = $"Com base no seu comportamento, sugerimos um limite diário de R$ {limite:F2}\n\n" +
                          "Este limite foi calculado considerando:\n" +
                          "• Seu saldo atual\n" +
                          "• Seu padrão de apostas\n" +
                          "• Sua capacidade financeira\n\n" +
                          "Respeitar limites é fundamental para apostas responsáveis.";

            return await CriarIntervencaoAsync(usuarioId, TipoIntervencao.LimiteApostas, titulo, mensagem, 4);
        }

        public async Task<Intervencao> CriarPausaObrigatoriaAsync(int usuarioId, int horas)
        {
            var titulo = "⏸️ Pausa Obrigatória";
            var mensagem = $"Para sua segurança, você não poderá realizar apostas pelas próximas {horas} horas.\n\n" +
                          "Use este tempo para:\n" +
                          "• Refletir sobre seus motivos para apostar\n" +
                          "• Praticar atividades relaxantes\n" +
                          "• Conectar-se com amigos e família\n" +
                          "• Buscar ajuda se necessário\n\n" +
                          "Esta pausa é para seu bem-estar.";

            return await CriarIntervencaoAsync(usuarioId, TipoIntervencao.PausaObrigatoria, titulo, mensagem, 5);
        }

        public async Task<Intervencao> CriarContatoApoioAsync(int usuarioId)
        {
            var titulo = "🤝 Rede de Apoio Disponível";
            var mensagem = "Você não está sozinho! Existem recursos disponíveis para ajudá-lo:\n\n" +
                          "📞 Linha de Apoio: 0800-123-4567\n" +
                          "💬 Chat Online: www.apoioapostas.com.br\n" +
                          "👥 Grupos de Apoio: Reuniões semanais\n" +
                          "👨‍⚕️ Profissionais: Psicólogos especializados\n\n" +
                          "Buscar ajuda é um sinal de força, não de fraqueza.";

            return await CriarIntervencaoAsync(usuarioId, TipoIntervencao.ContatoApoio, titulo, mensagem, 4);
        }

        public async Task<Intervencao> CriarEducacaoRiscosAsync(int usuarioId)
        {
            var titulo = "📚 Educação sobre Riscos das Apostas";
            var mensagem = "Conheça os riscos associados às apostas compulsivas:\n\n" +
                          "⚠️ Riscos Financeiros:\n" +
                          "• Perda de dinheiro que você não pode perder\n" +
                          "• Endividamento e problemas financeiros\n" +
                          "• Impacto na família e relacionamentos\n\n" +
                          "⚠️ Riscos Emocionais:\n" +
                          "• Ansiedade e depressão\n" +
                          "• Isolamento social\n" +
                          "• Perda de controle\n\n" +
                          "Lembre-se: apostas responsáveis são apenas entretenimento.";

            return await CriarIntervencaoAsync(usuarioId, TipoIntervencao.EducacaoRiscos, titulo, mensagem, 2);
        }

        public async Task<Intervencao> CriarSimulacaoInvestimentoAsync(int usuarioId, decimal valor)
        {
            var titulo = "💡 Simulação de Investimento Alternativo";
            var mensagem = $"Que tal investir R$ {valor:F2} de forma mais inteligente?\n\n" +
                          "Com este valor, você poderia:\n" +
                          "• Investir em fundos de renda fixa (6% ao ano)\n" +
                          "• Aplicar em ações de empresas sólidas\n" +
                          "• Contribuir para sua aposentadoria\n" +
                          "• Fazer um curso ou capacitação\n\n" +
                          "Investimentos reais crescem com o tempo, apostas podem desaparecer em segundos.";

            return await CriarIntervencaoAsync(usuarioId, TipoIntervencao.SimulacaoInvestimento, titulo, mensagem, 3);
        }

        public async Task<Intervencao> CriarEstatisticasConscientizacaoAsync(int usuarioId)
        {
            var titulo = "📊 Estatísticas para Conscientização";
            var mensagem = "Dados importantes sobre apostas compulsivas:\n\n" +
                          "📈 Estatísticas:\n" +
                          "• 2-3% da população desenvolve problemas com apostas\n" +
                          "• 90% das pessoas com problemas de apostas perdem dinheiro\n" +
                          "• Apenas 1% dos apostadores são lucrativos a longo prazo\n\n" +
                          "🎯 Realidade:\n" +
                          "• Casas de apostas sempre têm vantagem\n" +
                          "• Quanto mais você aposta, mais você perde\n" +
                          "• Apostas não são uma estratégia de investimento";

            return await CriarIntervencaoAsync(usuarioId, TipoIntervencao.EstatisticasConscientizacao, titulo, mensagem, 2);
        }

        public async Task<Intervencao> CriarLembreteResponsabilidadeAsync(int usuarioId)
        {
            var titulo = "🎯 Lembrete de Apostas Responsáveis";
            var mensagem = "Lembre-se dos princípios das apostas responsáveis:\n\n" +
                          "✅ Faça apostas apenas com dinheiro que pode perder\n" +
                          "✅ Estabeleça limites de tempo e dinheiro\n" +
                          "✅ Nunca aposte quando estiver chateado ou bêbado\n" +
                          "✅ Trate apostas como entretenimento, não como investimento\n" +
                          "✅ Pare quando atingir seus limites\n\n" +
                          "Sua saúde e bem-estar são mais importantes que qualquer aposta.";

            return await CriarIntervencaoAsync(usuarioId, TipoIntervencao.LembreteResponsabilidade, titulo, mensagem, 1);
        }

        public async Task<Intervencao> CriarConviteTerapiaAsync(int usuarioId)
        {
            var titulo = "🧠 Convite para Acompanhamento Profissional";
            var mensagem = "Consideramos que seria benéfico para você buscar acompanhamento profissional:\n\n" +
                          "👨‍⚕️ Psicólogos especializados em vícios\n" +
                          "👥 Grupos de apoio (Gamblers Anonymous)\n" +
                          "📚 Terapia cognitivo-comportamental\n" +
                          "💊 Acompanhamento psiquiátrico se necessário\n\n" +
                          "Buscar ajuda profissional é um passo importante para recuperação e bem-estar.";

            return await CriarIntervencaoAsync(usuarioId, TipoIntervencao.ConviteTerapia, titulo, mensagem, 5);
        }
    }
}
