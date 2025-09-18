using ApostasCompulsivas.DTOs;
using ApostasCompulsivas.Models;

namespace ApostasCompulsivas.Services
{
    /// <summary>
    /// Serviço para mapeamento entre DTOs e entidades
    /// </summary>
    public static class MappingService
    {
        /// <summary>
        /// Converte Usuario para UsuarioDTO
        /// </summary>
        public static UsuarioDTO ToDTO(this Usuario usuario)
        {
            return new UsuarioDTO
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
                Saldo = usuario.Saldo,
                DataCadastro = usuario.DataCadastro,
                Ativo = usuario.Ativo,
                NivelRisco = usuario.ObterDescricaoRisco(),
                PontuacaoRisco = usuario.PontuacaoRisco,
                UltimaAvaliacao = usuario.UltimaAvaliacao,
                RecebeAlertas = usuario.RecebeAlertas,
                AceitaApoio = usuario.AceitaApoio,
                Telefone = usuario.Telefone,
                DataUltimaAposta = usuario.DataUltimaAposta,
                TotalApostasHoje = usuario.TotalApostasHoje,
                ValorApostadoHoje = usuario.ValorApostadoHoje,
                DiasConsecutivosApostando = usuario.DiasConsecutivosApostando,
                ConsentimentoAceito = usuario.ConsentimentoAceito,
                DataConsentimento = usuario.DataConsentimento
            };
        }

        /// <summary>
        /// Converte CriarUsuarioDTO para Usuario
        /// </summary>
        public static Usuario ToEntity(this CriarUsuarioDTO dto)
        {
            var usuario = new Usuario(dto.Nome, dto.Email, dto.SaldoInicial)
            {
                Telefone = dto.Telefone,
                ConsentimentoAceito = dto.ConsentimentoAceito,
                DataConsentimento = dto.ConsentimentoAceito ? DateTime.Now : null
            };
            usuario.CalcularPontuacaoRisco();
            return usuario;
        }

        /// <summary>
        /// Converte AtualizarUsuarioDTO para Usuario
        /// </summary>
        public static Usuario ToEntity(this AtualizarUsuarioDTO dto, Usuario usuarioExistente)
        {
            usuarioExistente.Nome = dto.Nome;
            usuarioExistente.Email = dto.Email;
            usuarioExistente.Telefone = dto.Telefone;
            usuarioExistente.RecebeAlertas = dto.RecebeAlertas;
            usuarioExistente.AceitaApoio = dto.AceitaApoio;
            return usuarioExistente;
        }

        /// <summary>
        /// Converte Aposta para ApostaDTO
        /// </summary>
        public static ApostaDTO ToDTO(this Aposta aposta)
        {
            return new ApostaDTO
            {
                Id = aposta.Id,
                UsuarioId = aposta.UsuarioId,
                TipoAposta = aposta.TipoAposta,
                Valor = aposta.Valor,
                Multiplicador = aposta.Multiplicador,
                Status = aposta.Status,
                ValorGanho = aposta.ValorGanho,
                DataAposta = aposta.DataAposta,
                DataResultado = aposta.DataResultado
            };
        }

        /// <summary>
        /// Converte CriarApostaDTO para Aposta
        /// </summary>
        public static Aposta ToEntity(this CriarApostaDTO dto)
        {
            return new Aposta(dto.UsuarioId, dto.TipoAposta, dto.Valor, dto.Multiplicador);
        }

        /// <summary>
        /// Converte Historico para HistoricoDTO
        /// </summary>
        public static HistoricoDTO ToDTO(this Historico historico)
        {
            return new HistoricoDTO
            {
                Id = historico.Id,
                UsuarioId = historico.UsuarioId,
                TipoOperacao = historico.TipoOperacao,
                Valor = historico.Valor,
                Descricao = historico.Descricao,
                DataOperacao = historico.DataOperacao,
                SaldoAnterior = historico.SaldoAnterior,
                SaldoPosterior = historico.SaldoPosterior
            };
        }

        /// <summary>
        /// Converte CriarHistoricoDTO para Historico
        /// </summary>
        public static Historico ToEntity(this CriarHistoricoDTO dto)
        {
            return new Historico(
                dto.UsuarioId,
                dto.TipoOperacao,
                dto.Valor,
                dto.Descricao,
                dto.SaldoAnterior,
                dto.SaldoPosterior
            );
        }
    }
}
