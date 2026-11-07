using Moq;
using RescatApp.Interfaces;
using RescatApp.Models;
using RescatApp.Services;
using Xunit;

namespace RescatApp.Tests
{
    public class MascotasServicesTests
    {
        private readonly Mock<IMascotasRepository> _repo;
        private readonly MascotasServices _service;

        public MascotasServicesTests()
        {
            _repo = new Mock<IMascotasRepository>();
            _service = new MascotasServices(_repo.Object);
        }

        [Fact]
        public async Task CrearAsync_SinFecha_AsignaFechaActual()
        {
            _repo.Setup(r => r.CrearAsync(It.IsAny<Mascota>())).ReturnsAsync(1);
            var mascota = new Mascota();

            var result = await _service.CrearAsync(mascota);

            Assert.NotEqual(default, result.FechaIngreso);
        }

        [Fact]
        public async Task CrearAsync_ConFecha_ConservaLaFecha()
        {
            _repo.Setup(r => r.CrearAsync(It.IsAny<Mascota>())).ReturnsAsync(1);
            var fecha = new DateTime(2025, 1, 15);
            var mascota = new Mascota { FechaIngreso = fecha };

            var result = await _service.CrearAsync(mascota);

            Assert.Equal(fecha, result.FechaIngreso);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task CrearAsync_SinEstado_AsignaDisponible(string? estado)
        {
            _repo.Setup(r => r.CrearAsync(It.IsAny<Mascota>())).ReturnsAsync(1);
            var mascota = new Mascota { EstadoAdopcion = estado! };

            var result = await _service.CrearAsync(mascota);

            Assert.Equal("Disponible", result.EstadoAdopcion);
        }

        [Fact]
        public async Task CrearAsync_ConEstado_ConservaElEstado()
        {
            _repo.Setup(r => r.CrearAsync(It.IsAny<Mascota>())).ReturnsAsync(1);
            var mascota = new Mascota { EstadoAdopcion = "Adoptada" };

            var result = await _service.CrearAsync(mascota);

            Assert.Equal("Adoptada", result.EstadoAdopcion);
        }

        [Fact]
        public async Task CrearAsync_AsignaElIdGenerado()
        {
            _repo.Setup(r => r.CrearAsync(It.IsAny<Mascota>())).ReturnsAsync(42);

            var result = await _service.CrearAsync(new Mascota());

            Assert.Equal(42, result.IdMascota);
        }

        [Fact]
        public async Task ObtenerMascotasAsync_DevuelveLoDelRepositorio()
        {
            var lista = new List<Mascota> { new Mascota(), new Mascota() };
            _repo.Setup(r => r.ObtenerMascotasAsync("Perro", "Grande")).ReturnsAsync(lista);

            var result = await _service.ObtenerMascotasAsync("Perro", "Grande");

            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task ObtenerPorIdAsync_NoExiste_DevuelveNull()
        {
            _repo.Setup(r => r.ObtenerPorIdAsync(99)).ReturnsAsync((Mascota?)null);

            var result = await _service.ObtenerPorIdAsync(99);

            Assert.Null(result);
        }

        [Fact]
        public async Task ActualizarAsync_DevuelveElResultadoDelRepositorio()
        {
            var mascota = new Mascota();
            _repo.Setup(r => r.ActualizarAsync(mascota)).ReturnsAsync(true);

            var result = await _service.ActualizarAsync(mascota);

            Assert.True(result);
        }

        [Fact]
        public async Task EliminarAsync_DevuelveElResultadoDelRepositorio()
        {
            _repo.Setup(r => r.EliminarAsync(5)).ReturnsAsync(false);

            var result = await _service.EliminarAsync(5);

            Assert.False(result);
        }
    }
}
