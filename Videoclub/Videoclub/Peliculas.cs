using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

namespace Videoclub
{
    class Peliculas
    {
        static SqlConnection connection = new SqlConnection("Data Source=DESKTOP-F3J0NS5\\SQLEXPRESS;Initial Catalog=Northwind;Integrated Security=True");
        public int ID { get; set; }
        public string Titulo { get; set; }
        public string Sipnosis { get; set; }
        public int EdadRecomendada { get; set; }
        public string Estado { get; set; }

        public Peliculas(int iD, string titulo, string sipnosis, int edadRecomendada, string estado)
        {

            ID = iD;
            Titulo = titulo;
            Sipnosis = sipnosis;
            EdadRecomendada = edadRecomendada;
            Estado = estado;
        }

        public Peliculas(string titulo, string sipnosis, int edadRecomendada, string estado)
        {
            Titulo = titulo;
            Sipnosis = sipnosis;
            EdadRecomendada = edadRecomendada;
            Estado = estado;
        }

        public Peliculas()
        {
        }

        public void MostrarDatos()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"Título:{Titulo}");
            Console.WriteLine($"Sipnosis:{Sipnosis}");
            Console.WriteLine($"Edad recomendada: mayores de {EdadRecomendada} años");
            Console.WriteLine($"Estado:{Estado}");
        }
        public void UpdateEstado(Peliculas pelicula)
        {
            string query = $"SELECT Estado from Peliculas WHERE ID={pelicula.ID}";
            SqlDataReader reader = ConsultarBase(query);
            if (reader.Read())
            {
                string estado = reader[0].ToString().ToUpper();

                if (estado == "DISPONIBLE")
                {
                    query = $"Update Habitaciones SET ESTADO ='OCUPADA'";
                    ModificarBase(query);
                }
                else if (estado == "OCUPADA")
                {
                    query = $"Update Habitaciones SET ESTADO ='DISPONIBLE'";
                    ModificarBase(query);
                }
                else
                {
                    Console.WriteLine("No se ha podido cambiar el estado");
                }
            }
        }
        public SqlDataReader ConsultarBase(string query)
        {
            connection.Open(); //Abrir la conexión
            SqlCommand command = new SqlCommand(query, connection); //Conectar el comando con sql, SIEMPRE TIENE QUE ESTAR
            SqlDataReader reader = command.ExecuteReader();
            return reader;

        }
        public static void ModificarBase(string query)
        {
            if (query != null)
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection); //Conectar el comando con sql, SIEMPRE TIENE QUE ESTAR
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

    }
}
