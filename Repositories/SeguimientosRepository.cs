using Microsoft.Data.SqlClient;
using RescatApp.Models;

namespace RescatApp.Repositories
{
    public class SeguimientosRepository
    {
        private readonly string _connectionString;

        public SeguimientosRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") 
                ?? throw new InvalidOperationException("Cadena de conexión 'DefaultConnection' no encontrada.");
        }

        public async Task<List<SeguimientoPostadopcion>> ObtenerPorMascotaIdAsync(int idMascota)
        {
            var seguimientos = new List<SeguimientoPostadopcion>();

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    string query = @"SELECT id_seguimiento, fecha_seguimiento, peso_actual, 
                                            estado_salud, observaciones, foto_evidencia, 
                                            id_mascota, id_adoptante 
                                     FROM tbl_seguimiento_postadopcion 
                                     WHERE id_mascota = @idMascota";

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@idMascota", idMascota);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                seguimientos.Add(new SeguimientoPostadopcion
                                {
                                    IdSeguimiento = reader.GetInt32(0),
                                    FechaSeguimiento = reader.GetDateTime(1),
                                    PesoActual = reader.IsDBNull(2) ? null : reader.GetDecimal(2),
                                    EstadoSalud = reader.IsDBNull(3) ? null : reader.GetString(3),
                                    Observaciones = reader.IsDBNull(4) ? null : reader.GetString(4),
                                    FotoEvidencia = reader.IsDBNull(5) ? null : reader.GetString(5),
                                    IdMascota = reader.GetInt32(6),
                                    IdAdoptante = reader.GetInt32(7)
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener seguimientos para la mascota {idMascota}: " + ex.Message, ex);
            }

            return seguimientos;
        }

        public async Task<int> CrearAsync(SeguimientoPostadopcion seguimiento)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    string query = @"INSERT INTO tbl_seguimiento_postadopcion 
                                     (fecha_seguimiento, peso_actual, estado_salud, observaciones, foto_evidencia, id_mascota, id_adoptante) 
                                     VALUES 
                                     (@fecha, @peso, @estadoSalud, @observaciones, @foto, @idMascota, @idAdoptante);
                                     SELECT SCOPE_IDENTITY();";

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@fecha", seguimiento.FechaSeguimiento);
                        command.Parameters.AddWithValue("@peso", (object?)seguimiento.PesoActual ?? DBNull.Value);
                        command.Parameters.AddWithValue("@estadoSalud", (object?)seguimiento.EstadoSalud ?? DBNull.Value);
                        command.Parameters.AddWithValue("@observaciones", (object?)seguimiento.Observaciones ?? DBNull.Value);
                        command.Parameters.AddWithValue("@foto", (object?)seguimiento.FotoEvidencia ?? DBNull.Value);
                        command.Parameters.AddWithValue("@idMascota", seguimiento.IdMascota);
                        command.Parameters.AddWithValue("@idAdoptante", seguimiento.IdAdoptante);

                        var newId = await command.ExecuteScalarAsync();
                        return Convert.ToInt32(newId);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al registrar el seguimiento post-adopción: " + ex.Message, ex);
            }
        }
    }
}