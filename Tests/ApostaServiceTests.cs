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
    /// Testes unitários para ApostaService
    /// </summary>
    public class ApostaServiceTests
    {
        private readonly Mock<IApostaRepository> _mockApostaRepository;
        private readonly Mock<IUsuarioRepository> _mockUsuarioRepository;
        private readonly Mock<IHistoricoRepository> _mockHistoricoRepository;
        private readonly ApostaService _apostaService;

        public ApostaServiceTests()
        {
            _mockApostaRepository = new Mock<IApostaRepository>();
            _mockUsuarioRepository = new Mock<IUsuarioRepository>();
            _mockHistoricoRepository = new Mock<IHistoricoRepository>();
            _apostaService = new ApostaService(_mockApostaRepository.Object, _mockUsuarioRepository.Object, _mockHistoricoRepository.Object);
        }

        [Fact]
        public async Task RealizarApostaAsync_ComSaldoSuficiente_DeveCriarAposta()
        {
            // Arrange
            var usuarioId = 1;
            var tipoAposta = "Futebol";
            var valor = 50.00m;
            var multiplicador = 2.0m;

            var usuario = new Usuario("João", "joao@email.com", 100.00m) { Id = usuarioId };
            var apostaEsperada = new Aposta(usuarioId, tipoAposta, valor, multiplicador) { Id = 1 };

            _mockUsuarioRepository.Setup(x => x.GetByIdAsync(usuarioId)).ReturnsAsync(usuario);
            _mockApostaRepository.Setup(x => x.CreateAsync(It.IsAny<Aposta>())).ReturnsAsync(apostaEsperada);
            _mockUsuarioRepository.Setup(x => x.UpdateAsync(It.IsAny<Usuario>())).ReturnsAsync(usuario);

            // Act
            var resultado = await _apostaService.RealizarApostaAsync(usuarioId, tipoAposta, valor, multiplicador);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(usuarioId, resultado.UsuarioId);
            Assert.Equal(tipoAposta, resultado.TipoAposta);
            Assert.Equal(valor, resultado.Valor);
            Assert.Equal(multiplicador, resultado.Multiplicador);
            Assert.Equal("Pendente", resultado.Status);
            _mockApostaRepository.Verify(x => x.CreateAsync(It.IsAny<Aposta>()), Times.Once);
        }

        [Fact]
        public async Task RealizarApostaAsync_ComSaldoInsuficiente_DeveLancarExcecao()
        {
            // Arrange
            var usuarioId = 1;
            var tipoAposta = "Futebol";
            var valor = 150.00m;
            var multiplicador = 2.0m;

            var usuario = new Usuario("João", "joao@email.com", 100.00m) { Id = usuarioId };

            _mockUsuarioRepository.Setup(x => x.GetByIdAsync(usuarioId)).ReturnsAsync(usuario);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => 
                _apostaService.RealizarApostaAsync(usuarioId, tipoAposta, valor, multiplicador));
        }

        [Fact]
        public async Task FinalizarApostaAsync_ComApostaValida_DeveAtualizarStatus()
        {
            // Arrange
            var apostaId = 1;
            var usuarioId = 1;
            var aposta = new Aposta(usuarioId, "Futebol", 50.00m, 2.0m) 
            { 
                Id = apostaId,
                Status = "Pendente"
            };
            var usuario = new Usuario("João", "joao@email.com", 100.00m) { Id = usuarioId };

            _mockApostaRepository.Setup(x => x.GetByIdAsync(apostaId)).ReturnsAsync(aposta);
            _mockUsuarioRepository.Setup(x => x.GetByIdAsync(usuarioId)).ReturnsAsync(usuario);
            _mockApostaRepository.Setup(x => x.UpdateAsync(It.IsAny<Aposta>())).ReturnsAsync(aposta);
            _mockUsuarioRepository.Setup(x => x.UpdateAsync(It.IsAny<Usuario>())).ReturnsAsync(usuario);

            // Act
            var resultado = await _apostaService.FinalizarApostaAsync(apostaId, true);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal("Ganhou", resultado.Status);
            Assert.NotNull(resultado.DataResultado);
            Assert.Equal(100.00m, resultado.ValorGanho);
            _mockApostaRepository.Verify(x => x.UpdateAsync(It.IsAny<Aposta>()), Times.Once);
        }
    }
}
