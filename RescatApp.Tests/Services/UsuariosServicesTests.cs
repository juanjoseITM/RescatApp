using Moq;
using RescatApp.Interfaces;
using RescatApp.Models;
using RescatApp.Services;

namespace RescatApp.Tests.Services
{
    public class UsuariosServicesTests
    {
        [Fact]
        public async Task EmailEnUsoAsync_CorreoLibre_DevuelveFalse()
        {
            // Arrange
            var repo = new Mock<IUsuariosRepository>();
            repo.Setup(r => r.ObtenerPorEmailAsync("libre@test.com"))
                .ReturnsAsync((Usuario?)null);
            var service = new UsuariosServices(repo.Object);

            // Act
            var resultado = await service.EmailEnUsoAsync("libre@test.com");

            // Assert
            Assert.False(resultado);
        }

        [Fact]
        public async Task EmailEnUsoAsync_CorreoOcupado_DevuelveTrue()
        {
            // Arrange
            var repo = new Mock<IUsuariosRepository>();
            repo.Setup(r => r.ObtenerPorEmailAsync("juan@test.com"))
                .ReturnsAsync(new Usuario {IdUsuario = 1});

            var service = new UsuariosServices(repo.Object);

            // Act
            var resultado = await service.EmailEnUsoAsync("juan@test.com");

            // Assert
            Assert.True(resultado);

        }

        [Fact]
        public async Task EmailEnUsoAsync_MismoUsuarioAlActualizar_DevuelveFalse()
        {
            // Arrange
            var repo = new Mock<IUsuariosRepository>();
            repo.Setup(r => r.ObtenerPorEmailAsync("juan@test.com"))
                .ReturnsAsync(new Usuario { IdUsuario = 1 });

            var service = new UsuariosServices(repo.Object);

                // Act
                var resultado = await service.EmailEnUsoAsync("juan@test.com", 1);

                // Assert
                Assert.False(resultado);

        }

        [Fact]
        public async Task EmailEnUsoAsync_CorreoDeOtroAlActualizar_DevuelveTrue()
        {
            // Arrange
            var repo = new Mock<IUsuariosRepository>();
            repo.Setup(r => r.ObtenerPorEmailAsync("juan@test.com"))
                .ReturnsAsync(new Usuario { IdUsuario = 2 });

            var service = new UsuariosServices(repo.Object);

            // Act
            var resultado = await service.EmailEnUsoAsync("juan@test.com", 1);

            // Assert
            Assert.True(resultado);
        }

    } 
    
}