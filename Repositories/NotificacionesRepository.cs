using Microsoft.Data.SqlClient;
using RescatApp.Models;
using System.Data;

namespace RescatApp.Repositories
{
    public class NotificacionesRepository
    {
        private readonly string _connectionString;

        public NotificacionesRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("CadenaConexion") ?? "";
        }

        // Obtener todas las notificaciones de un usuario
        public List<Notificacion> ObtenerPorUsuario(int idUsuario)
        {
            var lista = new List<Notificacion>();

            using (var conexion = new SqlConnection(_connectionString))
            {
                var query = "SELECT id_notificacion, tipo_notificacion, mensaje, fecha_envio, leido, id_usuario FROM tbl_notificacion WHERE id_usuario = @IdUsuario";
                using (var comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@IdUsuario", idUsuario);
                    conexion.Open();
                    using (var lector = comando.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            lista.Add(new Notificacion
                            {
                                IdNotificacion = Convert.ToInt32(lector["id_notificacion"]),
                                TipoNotificacion = lector["tipo_notificacion"].ToString() ?? "",
                                Mensaje = lector["mensaje"].ToString() ?? "",
                                FechaEnvio = Convert.ToDateTime(lector["fecha_envio"]),
                                Leido = Convert.ToBoolean(lector["leido"]),
                                IdUsuario = Convert.ToInt32(lector["id_usuario"])
                            });
                        }
                    }
                }
            }

            return lista;
        }

        // Crear una nueva notificación
        public bool Crear(Notificacion notificacion)
        {
            using (var conexion = new SqlConnection(_connectionString))
            {
                var query = "INSERT INTO tbl_notificacion (tipo_notificacion, mensaje, fecha_envio, leido, id_usuario) VALUES (@Tipo, @Mensaje, @Fecha, @Leido, @IdUsuario)";
                using (var comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@Tipo", notificacion.TipoNotificacion);
                    comando.Parameters.AddWithValue("@Mensaje", notificacion.Mensaje);
                    comando.Parameters.AddWithValue("@Fecha", notificacion.FechaEnvio);
                    comando.Parameters.AddWithValue("@Leido", notificacion.Leido);
                    comando.Parameters.AddWithValue("@IdUsuario", notificacion.IdUsuario);

                    conexion.Open();
                    int filasAfectadas = comando.ExecuteNonQuery();
                    return filasAfectadas > 0;
                }
            }
        }
    }
}
