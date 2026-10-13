using Microsoft.Data.SqlClient;
using RescatApp.Interfaces;
using RescatApp.Models;

namespace RescatApp.Repositories
{
    public class SolicitudesAdopcionRepository : ISolicitudesAdopcionRepository
    {
        private readonly string _connectionString;

        public SolicitudesAdopcionRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") 
                ?? throw new InvalidOperationException("Cadena de conexión 'DefaultConnection' no encontrada.");
        }

        public async Task<List<SolicitudAdopcion>> ObtenerTodasAsync()
        {
            var lista = new List<SolicitudAdopcion>();
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    string query = @"SELECT id_solicitud, id_mascota, id_adoptante, fecha_solicitud, estado_solicitud 
                                     FROM tbl_solicitud_adopcion";

                    using (var command = new SqlCommand(query, connection))
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            lista.Add(new SolicitudAdopcion
                            {
                                IdSolicitud = reader.GetInt32(0),
                                IdMascota = reader.GetInt32(1),
                                IdAdoptante = reader.GetInt32(2),
                                FechaSolicitud = reader.IsDBNull(3) ? DateTime.Now : reader.GetDateTime(3),
                                EstadoSolicitud = reader.IsDBNull(4) ? "Pendiente" : reader.GetString(4),
                                Observaciones = null
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener las solicitudes de adopción: " + ex.Message, ex);
            }
            return lista;
        }

        public async Task<SolicitudAdopcion?> ObtenerPorIdAsync(int id)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    string query = @"SELECT id_solicitud, id_mascota, id_adoptante, fecha_solicitud, estado_solicitud 
                                     FROM tbl_solicitud_adopcion WHERE id_solicitud = @id";

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                return new SolicitudAdopcion
                                {
                                    IdSolicitud = reader.GetInt32(0),
                                    IdMascota = reader.GetInt32(1),
                                    IdAdoptante = reader.GetInt32(2),
                                    FechaSolicitud = reader.IsDBNull(3) ? DateTime.Now : reader.GetDateTime(3),
                                    EstadoSolicitud = reader.IsDBNull(4) ? "Pendiente" : reader.GetString(4),
                                    Observaciones = null
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al buscar la solicitud con ID {id}: " + ex.Message, ex);
            }
            return null;
        }

        public async Task<List<SolicitudAdopcion>> ObtenerPorUsuarioAsync(int idUsuario)
        {
            var lista = new List<SolicitudAdopcion>();
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    string query = @"SELECT id_solicitud, id_mascota, id_adoptante, fecha_solicitud, estado_solicitud 
                                     FROM tbl_solicitud_adopcion WHERE id_adoptante = @idUsuario";

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@idUsuario", idUsuario);
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                lista.Add(new SolicitudAdopcion
                                {
                                    IdSolicitud = reader.GetInt32(0),
                                    IdMascota = reader.GetInt32(1),
                                    IdAdoptante = reader.GetInt32(2),
                                    FechaSolicitud = reader.IsDBNull(3) ? DateTime.Now : reader.GetDateTime(3),
                                    EstadoSolicitud = reader.IsDBNull(4) ? "Pendiente" : reader.GetString(4),
                                    Observaciones = null
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al consultar solicitudes del usuario {idUsuario}: " + ex.Message, ex);
            }
            return lista;
        }

public async Task<int> CrearAsync(SolicitudAdopcion solicitud)
{
    try
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            // Determinar el ID del estado según el texto recibido
            int idEstado = solicitud.EstadoSolicitud?.ToLower() switch
            {
                "pendiente" => 1,
                "aprobada" => 2,
                "rechazada" => 3,
                _ => int.TryParse(solicitud.EstadoSolicitud, out int parsedId) ? parsedId : 1
            };

            string query = @"INSERT INTO tbl_solicitud_adopcion 
                             (id_mascota, id_adoptante, fecha_solicitud, estado_solicitud)
                             VALUES 
                             (@idMascota, @idAdoptante, @fechaSolicitud, @estadoSolicitud);
                             SELECT SCOPE_IDENTITY();";

            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@idMascota", System.Data.SqlDbType.Int).Value = solicitud.IdMascota;
                command.Parameters.Add("@idAdoptante", System.Data.SqlDbType.Int).Value = solicitud.IdAdoptante;
                command.Parameters.Add("@fechaSolicitud", System.Data.SqlDbType.DateTime).Value = solicitud.FechaSolicitud;
                command.Parameters.Add("@estadoSolicitud", System.Data.SqlDbType.Int).Value = idEstado;

                var newId = await command.ExecuteScalarAsync();
                return Convert.ToInt32(newId);
            }
        }
    }
    catch (Exception ex)
    {
        throw new Exception("Error al crear la solicitud de adopción: " + ex.Message, ex);
    }
}
        public async Task<bool> ActualizarEstadoAsync(int idSolicitud, string nuevoEstado)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    string query = @"UPDATE tbl_solicitud_adopcion 
                                     SET estado_solicitud = @estado 
                                     WHERE id_solicitud = @id";

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@estado", nuevoEstado);
                        command.Parameters.AddWithValue("@id", idSolicitud);

                        int rowsAffected = await command.ExecuteNonQueryAsync();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al actualizar el estado de la solicitud {idSolicitud}: " + ex.Message, ex);
            }
        }
    }
}