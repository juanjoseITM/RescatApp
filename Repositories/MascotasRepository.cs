using Microsoft.Data.SqlClient;
using RescatApp.Interfaces;
using RescatApp.Models;

namespace RescatApp.Repositories
{
    public class MascotasRepository : IMascotasRepository
    {
        private readonly string _connectionString;

        public MascotasRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") 
                ?? throw new InvalidOperationException("Cadena de conexión 'DefaultConnection' no encontrada en appsettings.");
        }

        // List (Obtener todas las mascotas con filtros opcionales)
        public async Task<List<Mascota>> ObtenerMascotasAsync(string? especie, string? tamano)
        {
            var mascotas = new List<Mascota>();

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    
                    string query = @"SELECT id_mascota, nombre, especie, raza, edad_aprox, sexo, 
                                            tamano, estado_salud, descripcion, fecha_ingreso, 
                                            estado_adopcion, id_rescatista 
                                     FROM tbl_mascota 
                                     WHERE (@especie IS NULL OR LOWER(especie) = LOWER(@especie))
                                       AND (@tamano IS NULL OR LOWER(tamano) = LOWER(@tamano))";

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@especie", (object?)especie ?? DBNull.Value);
                        command.Parameters.AddWithValue("@tamano", (object?)tamano ?? DBNull.Value);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                mascotas.Add(new Mascota
                                {
                                    IdMascota = reader.GetInt32(0),
                                    Nombre = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                                    Especie = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                                    Raza = reader.IsDBNull(3) ? null : reader.GetString(3),
                                    EdadAprox = reader.IsDBNull(4) ? null : reader.GetInt32(4),
                                    Sexo = reader.IsDBNull(5) ? null : reader.GetString(5),
                                    Tamano = reader.IsDBNull(6) ? null : reader.GetString(6),
                                    EstadoSalud = reader.IsDBNull(7) ? null : reader.GetString(7),
                                    Descripcion = reader.IsDBNull(8) ? null : reader.GetString(8),
                                    FechaIngreso = reader.IsDBNull(9) ? DateTime.Now : reader.GetDateTime(9),
                                    EstadoAdopcion = reader.IsDBNull(10) ? string.Empty : reader.GetString(10),
                                    IdRescatista = reader.IsDBNull(11) ? 0 : reader.GetInt32(11)
                                });
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error en la consulta SQL al obtener la lista de mascotas: " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Error inesperado al procesar la lista de mascotas: " + ex.Message, ex);
            }

            return mascotas;
        }

        // Read (Mascota por ID)
        public async Task<Mascota?> ObtenerPorIdAsync(int id)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    string query = @"SELECT id_mascota, nombre, especie, raza, edad_aprox, sexo, 
                                            tamano, estado_salud, descripcion, fecha_ingreso, 
                                            estado_adopcion, id_rescatista 
                                     FROM tbl_mascota 
                                     WHERE id_mascota = @id";

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                return new Mascota
                                {
                                    IdMascota = reader.GetInt32(0),
                                    Nombre = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                                    Especie = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                                    Raza = reader.IsDBNull(3) ? null : reader.GetString(3),
                                    EdadAprox = reader.IsDBNull(4) ? null : reader.GetInt32(4),
                                    Sexo = reader.IsDBNull(5) ? null : reader.GetString(5),
                                    Tamano = reader.IsDBNull(6) ? null : reader.GetString(6),
                                    EstadoSalud = reader.IsDBNull(7) ? null : reader.GetString(7),
                                    Descripcion = reader.IsDBNull(8) ? null : reader.GetString(8),
                                    FechaIngreso = reader.IsDBNull(9) ? DateTime.Now : reader.GetDateTime(9),
                                    EstadoAdopcion = reader.IsDBNull(10) ? string.Empty : reader.GetString(10),
                                    IdRescatista = reader.IsDBNull(11) ? 0 : reader.GetInt32(11)
                                };
                            }
                        }
                    }
                }
                return null;
            }
            catch (SqlException ex)
            {
                throw new Exception($"Error de SQL al buscar la mascota con ID {id}: " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error inesperado al buscar la mascota con ID {id}: " + ex.Message, ex);
            }
        }

        // Create
        public async Task<int> CrearAsync(Mascota mascota)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    string query = @"INSERT INTO tbl_mascota 
                                     (nombre, especie, raza, edad_aprox, sexo, tamano, estado_salud, descripcion, fecha_ingreso, estado_adopcion, id_rescatista) 
                                     VALUES 
                                     (@nombre, @especie, @raza, @edad, @sexo, @tamano, @estadoSalud, @descripcion, @fechaIngreso, @estadoAdopcion, @idRescatista);
                                     SELECT SCOPE_IDENTITY();";

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@nombre", mascota.Nombre);
                        command.Parameters.AddWithValue("@especie", mascota.Especie);
                        command.Parameters.AddWithValue("@raza", (object?)mascota.Raza ?? DBNull.Value);
                        command.Parameters.AddWithValue("@edad", (object?)mascota.EdadAprox ?? DBNull.Value);
                        command.Parameters.AddWithValue("@sexo", (object?)mascota.Sexo ?? DBNull.Value);
                        command.Parameters.AddWithValue("@tamano", (object?)mascota.Tamano ?? DBNull.Value);
                        command.Parameters.AddWithValue("@estadoSalud", (object?)mascota.EstadoSalud ?? DBNull.Value);
                        command.Parameters.AddWithValue("@descripcion", (object?)mascota.Descripcion ?? DBNull.Value);
                        command.Parameters.AddWithValue("@fechaIngreso", mascota.FechaIngreso);
                        command.Parameters.AddWithValue("@estadoAdopcion", mascota.EstadoAdopcion);
                        command.Parameters.AddWithValue("@idRescatista", mascota.IdRescatista);

                        var newId = await command.ExecuteScalarAsync();
                        return Convert.ToInt32(newId);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error de SQL al insertar la mascota: " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Error inesperado al crear la mascota: " + ex.Message, ex);
            }
        }

        // Update
        public async Task<bool> ActualizarAsync(Mascota mascota)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    string query = @"UPDATE tbl_mascota SET 
                                     nombre = @nombre, especie = @especie, raza = @raza, 
                                     edad_aprox = @edad, sexo = @sexo, tamano = @tamano, 
                                     estado_salud = @estadoSalud, descripcion = @descripcion, 
                                     estado_adopcion = @estadoAdopcion 
                                     WHERE id_mascota = @id";

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", mascota.IdMascota);
                        command.Parameters.AddWithValue("@nombre", mascota.Nombre);
                        command.Parameters.AddWithValue("@especie", mascota.Especie);
                        command.Parameters.AddWithValue("@raza", (object?)mascota.Raza ?? DBNull.Value);
                        command.Parameters.AddWithValue("@edad", (object?)mascota.EdadAprox ?? DBNull.Value);
                        command.Parameters.AddWithValue("@sexo", (object?)mascota.Sexo ?? DBNull.Value);
                        command.Parameters.AddWithValue("@tamano", (object?)mascota.Tamano ?? DBNull.Value);
                        command.Parameters.AddWithValue("@estadoSalud", (object?)mascota.EstadoSalud ?? DBNull.Value);
                        command.Parameters.AddWithValue("@descripcion", (object?)mascota.Descripcion ?? DBNull.Value);
                        command.Parameters.AddWithValue("@estadoAdopcion", mascota.EstadoAdopcion);

                        int rows = await command.ExecuteNonQueryAsync();
                        return rows > 0;
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception($"Error de SQL al actualizar la mascota {mascota.IdMascota}: " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error inesperado al actualizar la mascota {mascota.IdMascota}: " + ex.Message, ex);
            }
        }

        // Delete
        public async Task<bool> EliminarAsync(int id)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    string query = "DELETE FROM tbl_mascota WHERE id_mascota = @id";

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        int rows = await command.ExecuteNonQueryAsync();
                        return rows > 0;
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception($"Error de SQL al eliminar la mascota {id}: " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error inesperado al eliminar la mascota {id}: " + ex.Message, ex);
            }
        }
    }
}