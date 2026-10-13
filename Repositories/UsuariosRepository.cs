using Microsoft.Data.SqlClient;
using RescatApp.Interfaces;
using RescatApp.Models;

namespace RescatApp.Repositories
{
    public class UsuariosRepository : IUsuariosRepository
    {
        private readonly string _connectionString;

        public UsuariosRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") 
                ?? throw new InvalidOperationException("Cadena de conexión 'DefaultConnection' no encontrada en appsettings.");
        }

        public async Task<List<Usuario>> ObtenerTodosAsync()
        {
            var usuarios = new List<Usuario>();
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    string query = @"SELECT id_usuario, nombre, correo, telefono, tipo_usuario, fecha_registro 
                                     FROM tbl_usuario";

                    using (var command = new SqlCommand(query, connection))
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            usuarios.Add(MapearUsuario(reader));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al consultar la lista de usuarios: " + ex.Message, ex);
            }
            return usuarios;
        }

        public async Task<Usuario?> ObtenerPorIdAsync(int id)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    string query = @"SELECT id_usuario, nombre, correo, telefono, tipo_usuario, fecha_registro 
                                     FROM tbl_usuario WHERE id_usuario = @id";

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                return MapearUsuario(reader);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al consultar el usuario con ID {id}: " + ex.Message, ex);
            }
            return null;
        }

        public async Task<Usuario?> ObtenerPorEmailAsync(string email)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    string query = @"SELECT id_usuario, nombre, correo, telefono, tipo_usuario, fecha_registro 
                                     FROM tbl_usuario WHERE LOWER(correo) = LOWER(@email)";

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@email", email);
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                return MapearUsuario(reader);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al buscar usuario con email {email}: " + ex.Message, ex);
            }
            return null;
        }

        public async Task<int> CrearAsync(Usuario usuario)
{
    try
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            // Mapear el rol de texto a su ID numérico correspondiente
            int idTipoUsuario = usuario.Rol?.ToLower() switch
            {
                "adoptante" => 1,
                "rescatista" => 2,
                "admin" or "administrador" => 3,
                _ => int.TryParse(usuario.Rol, out int parsedId) ? parsedId : 1
            };

            string query = @"INSERT INTO tbl_usuario (nombre, correo, telefono, contrasena, tipo_usuario, fecha_registro)
                             VALUES (@nombre, @correo, @telefono, @contrasena, @tipoUsuario, GETDATE());
                             SELECT SCOPE_IDENTITY();";

            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@nombre", usuario.Nombre);
                command.Parameters.AddWithValue("@correo", usuario.Email);
                command.Parameters.AddWithValue("@telefono", (object?)usuario.Telefono ?? DBNull.Value);
                command.Parameters.AddWithValue("@contrasena", usuario.Clave ?? "123456");
                command.Parameters.AddWithValue("@tipoUsuario", idTipoUsuario);

                var newId = await command.ExecuteScalarAsync();
                return Convert.ToInt32(newId);
            }
        }
    }
    catch (Exception ex)
    {
        throw new Exception("Error al crear el usuario: " + ex.Message, ex);
    }
}

        public async Task<bool> ActualizarAsync(Usuario usuario)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    string query = @"UPDATE tbl_usuario 
                                     SET nombre = @nombre, correo = @correo, telefono = @telefono, tipo_usuario = @tipoUsuario
                                     WHERE id_usuario = @id";

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", usuario.IdUsuario);
                        command.Parameters.AddWithValue("@nombre", usuario.Nombre);
                        command.Parameters.AddWithValue("@correo", usuario.Email);
                        command.Parameters.AddWithValue("@telefono", (object?)usuario.Telefono ?? DBNull.Value);
                        command.Parameters.AddWithValue("@tipoUsuario", usuario.Rol);

                        int rows = await command.ExecuteNonQueryAsync();
                        return rows > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al actualizar el usuario {usuario.IdUsuario}: " + ex.Message, ex);
            }
        }

        public async Task<bool> EliminarAsync(int id)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    string query = "DELETE FROM tbl_usuario WHERE id_usuario = @id";

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        int rows = await command.ExecuteNonQueryAsync();
                        return rows > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al eliminar el usuario {id}: " + ex.Message, ex);
            }
        }

        public async Task<Usuario?> ValidarCredencialesAsync(string email, string clave)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    string query = @"SELECT id_usuario, nombre, correo, telefono, tipo_usuario, fecha_registro 
                                     FROM tbl_usuario 
                                     WHERE LOWER(correo) = LOWER(@email) AND contrasena = @clave";

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@email", email);
                        command.Parameters.AddWithValue("@clave", clave);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                return MapearUsuario(reader);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al validar las credenciales del usuario: " + ex.Message, ex);
            }
            return null;
        }

        private static Usuario MapearUsuario(SqlDataReader reader)
{
    int tipoInt = reader.IsDBNull(4) ? 1 : reader.GetInt32(4);
    string rolTexto = tipoInt switch
    {
        1 => "Adoptante",
        2 => "Rescatista",
        3 => "Administrador",
        _ => "Adoptante"
    };

    return new Usuario
    {
        IdUsuario = reader.GetInt32(0),
        Nombre = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
        Email = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
        Telefono = reader.IsDBNull(3) ? null : reader.GetString(3),
        Rol = rolTexto,
        FechaRegistro = reader.IsDBNull(5) ? DateTime.Now : reader.GetDateTime(5)
    };
}
    }
}