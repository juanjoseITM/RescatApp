using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using RescatApp.Models;

namespace RescatApp.Repositories
{
    public class RefugioRepository
    {
        private readonly string _connectionString;

        public RefugioRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public List<Refugio> ObtenerTodos()
        {
            var refugios = new List<Refugio>();
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT id_refugio, nombre_refugio, direccion, telefono, descripcion FROM tbl_refugio";
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        refugios.Add(new Refugio
                        {
                            id_refugio = reader.GetInt32(0),
                            nombre_refugio = reader.IsDBNull(1) ? null : reader.GetString(1),
                            direccion = reader.IsDBNull(2) ? null : reader.GetString(2),
                            telefono = reader.IsDBNull(3) ? null : reader.GetString(3),
                            descripcion = reader.IsDBNull(4) ? null : reader.GetString(4)
                        });
                    }
                }
            }
            return refugios;
        }

        public void Insertar(Refugio refugio)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = "INSERT INTO tbl_refugio (nombre_refugio, direccion, telefono, descripcion) VALUES (@nombre_refugio, @direccion, @telefono, @descripcion)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@nombre_refugio", (object)refugio.nombre_refugio ?? System.DBNull.Value);
                command.Parameters.AddWithValue("@direccion", (object)refugio.direccion ?? System.DBNull.Value);
                command.Parameters.AddWithValue("@telefono", (object)refugio.telefono ?? System.DBNull.Value);
                command.Parameters.AddWithValue("@descripcion", (object)refugio.descripcion ?? System.DBNull.Value);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}