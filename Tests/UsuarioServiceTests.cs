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
    /// Testes unitários para UsuarioService
    /// </summary>
    public class UsuarioServiceTests
    {
        private readonly Mock<IUsuarioRepository> _mockUsuarioRepository;
        private readonly Mock<IHistoricoRepository> _mockHistoricoRepository;
        private readonly UsuarioService _usuarioService;

        public UsuarioServiceTests()
        {
            _mockUsuarioRepository = new Mock<IUsuarioRepository>();
            _mockHistoricoRepository = new Mock<IHistoricoRepository>();
            _usuarioService = new UsuarioService(_mockUsuarioRepository.Object, _mockHistoricoRepository.Object);
        }

        [Fact]
        public async Task CriarUsuarioAsync_ComDadosValidos_DeveCriarUsuarioComSucesso()
        {
            // Arrange
            var nome = "João Silva";
            var email = "joao@email.com";
            var saldoInicial = 100.00m;
            var telefone = "11999999999";

            var usuarioEsperado = new Usuario(nome, email, saldoInicial)
            {
                Id = 1,
                Telefone = telefone
            };

            _mockUsuarioRepository.Setup(x => x.EmailExistsAsync(email)).ReturnsAsync(false);
            _mockUsuarioRepository.Setup(x => x.CreateAsync(It.IsAny<Usuario>())).ReturnsAsync(usuarioEsperado);

            // Act
            var resultado = await _usuarioService.CriarUsuarioAsync(nome, email, saldoInicial, telefone);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(nome, resultado.Nome);
            Assert.Equal(email, resultado.Email);
            Assert.Equal(saldoInicial, resultado.Saldo);
            Assert.Equal(telefone, resultado.Telefone);
            _mockUsuarioRepository.Verify(x => x.CreateAsync(It.IsAny<Usuario>()), Times.Once);
        }

        [Fact]
        public async Task CriarUsuarioAsync_ComEmailDuplicado_DeveLancarExcecao()
        {
            // Arrange
            var nome = "João Silva";
            var email = "joao@email.com";
            var saldoInicial = 100.00m;

            _mockUsuarioRepository.Setup(x => x.EmailExistsAsync(email)).ReturnsAsync(true);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => 
                _usuarioService.CriarUsuarioAsync(nome, email, saldoInicial));
        }

        [Fact]
        public async Task CriarUsuarioAsync_ComNomeVazio_DeveLancarExcecao()
        {
            // Arrange
            var nome = "";
            var email = "joao@email.com";
            var saldoInicial = 100.00m;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => 
                _usuarioService.CriarUsuarioAsync(nome, email, saldoInicial));
        }

        [Fact]
        public async Task DepositarAsync_ComValorValido_DeveAtualizarSaldo()
        {
            // Arrange
            var usuarioId = 1;
            var valorDeposito = 50.00m;
            var usuario = new Usuario("João", "joao@email.com", 100.00m) { Id = usuarioId };

            _mockUsuarioRepository.Setup(x => x.GetByIdAsync(usuarioId)).ReturnsAsync(usuario);
            _mockUsuarioRepository.Setup(x => x.UpdateAsync(It.IsAny<Usuario>())).ReturnsAsync(usuario);

            // Act
            var resultado = await _usuarioService.DepositarAsync(usuarioId, valorDeposito);

            // Assert
            Assert.True(resultado);
            _mockUsuarioRepository.Verify(x => x.UpdateAsync(It.IsAny<Usuario>()), Times.Once);
        }

        [Fact]
        public async Task SacarAsync_ComSaldoInsuficiente_DeveLancarExcecao()
        {
            // Arrange
            var usuarioId = 1;
            var valorSaque = 150.00m;
            var usuario = new Usuario("João", "joao@email.com", 100.00m) { Id = usuarioId };

            _mockUsuarioRepository.Setup(x => x.GetByIdAsync(usuarioId)).ReturnsAsync(usuario);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => 
                _usuarioService.SacarAsync(usuarioId, valorSaque));
        }

        [Fact]
        public async Task SacarAsync_ComValorValido_DeveAtualizarSaldo()
        {
            // Arrange
            var usuarioId = 1;
            var valorSaque = 50.00m;
            var usuario = new Usuario("João", "joao@email.com", 100.00m) { Id = usuarioId };

            _mockUsuarioRepository.Setup(x => x.GetByIdAsync(usuarioId)).ReturnsAsync(usuario);
            _mockUsuarioRepository.Setup(x => x.UpdateAsync(It.IsAny<Usuario>())).ReturnsAsync(usuario);

            // Act
            var resultado = await _usuarioService.SacarAsync(usuarioId, valorSaque);

            // Assert
            Assert.True(resultado);
            _mockUsuarioRepository.Verify(x => x.UpdateAsync(It.IsAny<Usuario>()), Times.Once);
        }
    }
}
