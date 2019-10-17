using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

namespace Videoclub
{
    class Usuario
    {
        static SqlConnection connection = new SqlConnection("Data Source=DESKTOP-F3J0NS5\\SQLEXPRESS;Initial Catalog=Videoclub;Integrated Security=True");
        public int ID { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Email { get; set; }
        public string Contrasena { get; set; }
        public List<Peliculas> ListaPeliculasUsuario { get; set; }

       
        public Usuario(int iD, string nombre, string apellido, DateTime fechaNacimiento, string email, string contrasena, List<Peliculas> listaPeliculasUsuario) : this(iD, nombre, apellido, fechaNacimiento, email, contrasena)
        {
            ListaPeliculasUsuario = listaPeliculasUsuario;
        }

        public Usuario(int iD, string nombre, string apellido, DateTime fechaNacimiento, string email, string contrasena)
        {
            ID = iD;
            Nombre = nombre;
            Apellido = apellido;
            FechaNacimiento = fechaNacimiento;
            Email = email;
            Contrasena = contrasena;
        }

        public Usuario()
        {
        } //Constructor vacio


        public bool ComprobarUsuario(string email, string contrasena)
        {
            bool usuarioRegistrado = false;

            string query = $"SELECT Email from Usuario WHERE Email LIKE '{email}'";
            SqlCommand command = new SqlCommand(query, connection); //Conectar el comando con sql, SIEMPRE TIENE QUE ESTAR
            connection.Open(); //Abrir la conexión
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                connection.Close();
                query = $"SELECT * from Usuario WHERE Email LIKE '{email}' AND Contrasena LIKE '{contrasena}'";
                reader = ConsultarBase(query);
                if (reader.Read())
                {
                    connection.Close();
                    usuarioRegistrado = true;
                }
                else
                {
                    connection.Close();
                    Console.WriteLine("Contraseña incorrecta");
                    //Llevarle al método de iniciar sesion
                }
            }
            else
            {
                connection.Close();
                //Console.WriteLine($"El email {email} no está registrado, debe registrarse primero");
                //LLevarle al método de registro
            }

            return usuarioRegistrado;
        }
        public SqlDataReader ConsultarBase(string query)
        {
            connection.Open(); //Abrir la conexión
            SqlCommand command = new SqlCommand(query, connection); //Conectar el comando con sql, SIEMPRE TIENE QUE ESTAR
            SqlDataReader reader = command.ExecuteReader();
            return reader;

        }
        public bool EdadCorrecta()
        {
            return true;
        }
        public bool EmailCorrecto()
        {
            return true;
        }
        public bool ContrasenaCorrecta()
        {
            return true;
        }

        public List<Peliculas> CalcularListaPeliculasU()
        {
            List<Peliculas> peliculas = new List<Peliculas>();
            int edadUsuario = CalcularEdad();

            string query = $"SELECT* from Peliculas WHERE EdadRecomendada<= {edadUsuario}";
            SqlDataReader reader = ConsultarBase(query);
            while (reader.Read())
            {
                Peliculas pelicula = new Peliculas(Convert.ToInt32(reader[0].ToString()), reader[1].ToString(), reader[2].ToString(), Convert.ToInt32(reader[3].ToString()), reader[4].ToString()); //Para crear un objeto
                peliculas.Add(pelicula);
            }
            connection.Close();
            return peliculas;

            ////Se le muestran las peliculas al usuario dependiendo de su edad
            //List<Peliculas> peliculasUsuario = new List<Peliculas>();
            //foreach (Peliculas item in peliculas)
            //{
            //    if (edadUsuario > item.EdadRecomendada)
            //    {                  
            //        Peliculas p1 = new Peliculas(item.ID,item.Titulo, item.Sipnosis, item.EdadRecomendada, item.Estado);
            //        peliculasUsuario.Add(p1);
            //    }
            //}
        }
        public int CalcularEdad()
        {
            int edad = Convert.ToInt32(DateTime.Now.Year - FechaNacimiento.Year);
            if (DateTime.Now.Month >= FechaNacimiento.Month)
            {
                if (DateTime.Now.Day >= FechaNacimiento.Day)
                {
                    return edad;
                }
                else
                {
                    return edad - 1;
                }
            }
            else
            {
                return edad - 1;
            }

        }

        
    }
}
