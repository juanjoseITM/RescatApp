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
                ?? throw new InvalidOperationException("Cadena de conexión 'DefaultConnection' no encontrada.");
        }

        public async Task<List<Usuario>> ObtenerTodosAsync()
        {
            var usuarios = new List<Usuario>();
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    string query = @"SELECT id_usuario, nombre, correo, contrasena, telefono, direccion, id_tipo_usuario, fecha_registro 
                                     FROM tbl_usuario";

                    using (var command = new SqlCommand(query, connection))
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            usuarios.Add(new Usuario
                            {
                                IdUsuario = reader.GetInt32(0),
                                Nombre = reader.GetString(1),
                                Correo = reader.GetString(2),
                                Contrasena = reader.GetString(3),
                                Telefono = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                                Direccion = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                                IdTipoUsuario = reader.GetInt32(6),
                                FechaRegistro = reader.GetDateTime(7)
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el listado de usuarios: " + ex.Message, ex);
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
                    string query = @"SELECT id_usuario, nombre, correo, contrasena, telefono, direccion, id_tipo_usuario, fecha_registro 
                                     FROM tbl_usuario WHERE id_usuario = @id";

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                return new Usuario
                                {
                                    IdUsuario = reader.GetInt32(0),
                                    Nombre = reader.GetString(1),
                                    Correo = reader.GetString(2),
                                    Contrasena = reader.GetString(3),
                                    Telefono = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                                    Direccion = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                                    IdTipoUsuario = reader.GetInt32(6),
                                    FechaRegistro = reader.GetDateTime(7)
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al buscar el usuario con ID {id}: " + ex.Message, ex);
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
                    string query = @"SELECT id_usuario, nombre, correo, contrasena, telefono, direccion, id_tipo_usuario, fecha_registro 
                                     FROM tbl_usuario WHERE correo = @email";

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@email", email);
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                return new Usuario
                                {
                                    IdUsuario = reader.GetInt32(0),
                                    Nombre = reader.GetString(1),
                                    Correo = reader.GetString(2),
                                    Contrasena = reader.GetString(3),
                                    Telefono = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                                    Direccion = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                                    IdTipoUsuario = reader.GetInt32(6),
                                    FechaRegistro = reader.GetDateTime(7)
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al consultar usuario por correo {email}: " + ex.Message, ex);
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
                    string query = @"INSERT INTO tbl_usuario (nombre, correo, contrasena, telefono, direccion, id_tipo_usuario, fecha_registro)
                                     VALUES (@nombre, @correo, @contrasena, @telefono, @direccion, @idTipoUsuario, @fechaRegistro);
                                     SELECT SCOPE_IDENTITY();";

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@nombre", usuario.Nombre);
                        command.Parameters.AddWithValue("@correo", usuario.Correo);
                        command.Parameters.AddWithValue("@contrasena", usuario.Contrasena);
                        command.Parameters.AddWithValue("@telefono", (object?)usuario.Telefono ?? DBNull.Value);
                        command.Parameters.AddWithValue("@direccion", (object?)usuario.Direccion ?? DBNull.Value);
                        command.Parameters.AddWithValue("@idTipoUsuario", usuario.IdTipoUsuario);
                        command.Parameters.AddWithValue("@fechaRegistro", usuario.FechaRegistro);

                        var newId = await command.ExecuteScalarAsync();
                        return Convert.ToInt32(newId);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al registrar el nuevo usuario: " + ex.Message, ex);
            }
        }
    }
}