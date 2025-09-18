using System;
using System.Threading.Tasks;
using Xunit;
using Moq;
using ApostasCompulsivas.Models;
using ApostasCompulsivas.Services;
using ApostasCompulsivas.Repository;

namespace ApostasCompulsivas.Tests
{
    /// <summary>
    /// Testes unitários para DetecaoComportamentoService
    /// </summary>
    public class DetecaoComportamentoTests
    {
        private readonly Mock<IUsuarioRepository> _mockUsuarioRepository;
        private readonly Mock<IApostaRepository> _mockApostaRepository;
        private readonly DetecaoComportamentoService _detecaoService;

        public DetecaoComportamentoTests()
        {
            _mockUsuarioRepository = new Mock<IUsuarioRepository>();
            _mockApostaRepository = new Mock<IApostaRepository>();
            var mockHistoricoRepository = new Mock<IHistoricoRepository>();
            _detecaoService = new DetecaoComportamentoService(_mockUsuarioRepository.Object, _mockApostaRepository.Object, mockHistoricoRepository.Object);
        }

        [Fact]
        public void CalcularPontuacaoRisco_ComComportamentoNormal_DeveRetornarBaixoRisco()
        {
            // Arrange
            var usuario = new Usuario("João", "joao@email.com", 1000.00m)
            {
                TotalApostasHoje = 2,
                ValorApostadoHoje = 50.00m,
                DiasConsecutivosApostando = 1,
                DataUltimaAposta = DateTime.Now.AddHours(-5)
            };

            // Act
            usuario.CalcularPontuacaoRisco();

            // Assert
            Assert.Equal(NivelRisco.Baixo, usuario.NivelRisco);
            Assert.True(usuario.PontuacaoRisco < 40);
        }

        [Fact]
        public void CalcularPontuacaoRisco_ComComportamentoCompulsivo_DeveRetornarAltoRisco()
        {
            // Arrange
            var usuario = new Usuario("João", "joao@email.com", 1000.00m)
            {
                TotalApostasHoje = 10, // Muitas apostas hoje
                ValorApostadoHoje = 200.00m, // 20% do saldo
                DiasConsecutivosApostando = 5, // Muitos dias consecutivos
                DataUltimaAposta = DateTime.Now.AddMinutes(-30) // Aposta recente
            };

            // Act
            usuario.CalcularPontuacaoRisco();

            // Assert
            Assert.Equal(NivelRisco.Alto, usuario.NivelRisco);
            Assert.True(usuario.PontuacaoRisco >= 70);
        }

        [Fact]
        public void CalcularPontuacaoRisco_ComComportamentoModerado_DeveRetornarMedioRisco()
        {
            // Arrange
            var usuario = new Usuario("João", "joao@email.com", 1000.00m)
            {
                TotalApostasHoje = 6, // Algumas apostas
                ValorApostadoHoje = 100.00m, // 10% do saldo
                DiasConsecutivosApostando = 4, // Alguns dias consecutivos
                DataUltimaAposta = DateTime.Now.AddHours(-1) // Aposta recente
            };

            // Act
            usuario.CalcularPontuacaoRisco();

            // Assert
            Assert.Equal(NivelRisco.Medio, usuario.NivelRisco);
            Assert.True(usuario.PontuacaoRisco >= 40 && usuario.PontuacaoRisco < 70);
        }

        [Fact]
        public void EstaEmRisco_ComAltoRisco_DeveRetornarTrue()
        {
            // Arrange
            var usuario = new Usuario("João", "joao@email.com", 1000.00m)
            {
                NivelRisco = NivelRisco.Alto
            };

            // Act
            var resultado = usuario.EstaEmRisco();

            // Assert
            Assert.True(resultado);
        }

        [Fact]
        public void EstaEmRisco_ComBaixoRisco_DeveRetornarFalse()
        {
            // Arrange
            var usuario = new Usuario("João", "joao@email.com", 1000.00m)
            {
                NivelRisco = NivelRisco.Baixo
            };

            // Act
            var resultado = usuario.EstaEmRisco();

            // Assert
            Assert.False(resultado);
        }

        [Fact]
        public void ObterDescricaoRisco_ComAltoRisco_DeveRetornarDescricaoCorreta()
        {
            // Arrange
            var usuario = new Usuario("João", "joao@email.com", 1000.00m)
            {
                NivelRisco = NivelRisco.Alto
            };

            // Act
            var resultado = usuario.ObterDescricaoRisco();

            // Assert
            Assert.Contains("Alto Risco", resultado);
            Assert.Contains("Ajuda profissional", resultado);
        }
    }
}
