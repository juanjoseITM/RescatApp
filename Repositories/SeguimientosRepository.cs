using RescatApp.Identities;
using Microsoft.Data.SqlClient;
using System.Data;

namespace RescatApp.Repositories
{
    public class SeguimientosRepository
    {
        private readonly string _connectionString;

        public SeguimientosRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConexionSQL") ?? "";
        }

        public List<Seguimiento> ObtenerTodos()
        {
            var lista = new List<Seguimiento>();
            using (var conexion = new SqlConnection(_connectionString))
            {
                conexion.Open();
                var cmd = new SqlCommand("SELECT id_seguimiento, fecha_seguimiento, peso_actual, estado_salud, observaciones, foto_evidencia, id_mascota, id_adoptante FROM tbl_seguimiento_postadopcion", conexion);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new Seguimiento
                        {
                            id_seguimiento = Convert.ToInt32(reader["id_seguimiento"]),
                            fecha_seguimiento = reader["fecha_seguimiento"] != DBNull.Value ? Convert.ToDateTime(reader["fecha_seguimiento"]) : null,
                            peso_actual = reader["peso_actual"] != DBNull.Value ? Convert.ToDecimal(reader["peso_actual"]) : null,
                            estado_salud = reader["estado_salud"]?.ToString(),
                            observaciones = reader["observaciones"]?.ToString(),
                            foto_evidencia = reader["foto_evidencia"]?.ToString(),
                            id_mascota = Convert.ToInt32(reader["id_mascota"]),
                            id_adoptante = Convert.ToInt32(reader["id_adoptante"])
                        });
                    }
                }
            }
            return lista;
        }

        public bool Guardar(Seguimiento seguimiento)
        {
            using (var conexion = new SqlConnection(_connectionString))
            {
                conexion.Open();
                var query = @"INSERT INTO tbl_seguimiento_postadopcion (fecha_seguimiento, peso_actual, estado_salud, observaciones, foto_evidencia, id_mascota, id_adoptante) 
                              VALUES (@fecha, @peso, @estado, @obs, @foto, @idMascota, @idAdoptante)";
                var cmd = new SqlCommand(query, conexion);
                cmd.Parameters.AddWithValue("@fecha", (object?)seguimiento.fecha_seguimiento ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@peso", (object?)seguimiento.peso_actual ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@estado", (object?)seguimiento.estado_salud ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@obs", (object?)seguimiento.observaciones ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@foto", (object?)seguimiento.foto_evidencia ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@idMascota", seguimiento.id_mascota);
                cmd.Parameters.AddWithValue("@idAdoptante", seguimiento.id_adoptante);

                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }
}
