using System;
using System.Data.SqlClient;
using System.Collections.Generic; //IMPORTANTE
using System.Linq; //Para usar el Element At
namespace Videoclub
{
    class Program
    {
        static SqlConnection connection = new SqlConnection("Data Source=DESKTOP-F3J0NS5\\SQLEXPRESS;Initial Catalog=Videoclub;Integrated Security=True");
        static void Main(string[] args)
        {

            MenuInicio();

            List<Usuario> usuarios = new List<Usuario>();

        }

        public static void MenuInicio()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\nBIENVENIDO AL VIDEOCLUB EXPIRIENCE:\n¿Quieres spoilear a tus amigos?\nAhora puedes tener los mejores estrenos antes de su lanzamiento\n");
            Console.WriteLine("¿Qué acción desea realizar?");
            Console.WriteLine("*********************************");
            Console.WriteLine("\n1.Iniciar Sesión");
            Console.WriteLine("\n2.Registrarse");
            Console.WriteLine("\n3.Salir");
            int opcion;

            if (Int32.TryParse(Console.ReadLine(), out opcion))
            {
                switch (opcion)
                {
                    case 1:
                        Console.Write("Email: ");
                        string email = Console.ReadLine();
                        Console.WriteLine();
                        Console.Write("Contraseña: ");
                        string contrasena = Console.ReadLine();
                        Console.WriteLine();
                        Usuario usuario = new Usuario();
                        if (usuario.ComprobarUsuario(email, contrasena))
                        {
                            string query = $"Select * from Usuario where Email LIKE '{email}' ";
                            connection.Open(); //Abrir la conexión
                            SqlCommand command = new SqlCommand(query, connection); //Conectar el comando con sql, SIEMPRE TIENE QUE ESTAR
                            SqlDataReader reader = command.ExecuteReader();

                            if (reader.Read())
                            {
                                usuario = new Usuario(Convert.ToInt32(reader[0].ToString()), reader[1].ToString(), reader[2].ToString(), Convert.ToDateTime(reader[3].ToString()), reader[4].ToString(), reader[5].ToString(), usuario.CalcularListaPeliculasU());
                            }

                            connection.Close();

                            MenuSecundario(usuario);
                        }
                        else
                        {
                            Console.WriteLine($"El email {email} no está registrado\nDebe registrarse primero, si desea registrarse pulse 1, si desea salir pulse cualquier tecla");
                            string respuesta = Console.ReadLine();
                            if (Int32.TryParse(respuesta, out opcion))
                            {
                                if (opcion == 1)
                                {
                                    RegistrarUsuario();
                                }

                            }
                            else
                            {
                                ;
                            }
                        }
                        break;
                    case 2:
                        RegistrarUsuario();
                        break;
                    case 3:
                        Console.WriteLine("Esperamos que vuelva pronto");
                        break;
                    default:
                        Console.WriteLine("\n***El número introducido no representa ninguna opción, por favor introduzcalo otra vez***\n");
                        MenuInicio();
                        break;
                }
            }
            else
            {
                Console.WriteLine("Debe introducir un dígito");
                MenuInicio();
            }
        }
        public static void MenuSecundario(Usuario usuario)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("*********************************");
            Console.WriteLine("\n¿Qué acción quiere realizar?");
            Console.WriteLine("\n1.Ver películas disponibles");
            Console.WriteLine("\n2.Alquilar película ");
            Console.WriteLine("\n3.Mis alquileres");
            Console.WriteLine("\n4.Logout");
            Console.WriteLine("*********************************");
            int opcion = 0;
            if (int.TryParse(Console.ReadLine(), out opcion))
            {
                switch (opcion)
                {
                    case 1:
                        MostrarPeliculas(usuario);
                        break;
                    case 2:
                        AlquilarPelicula(usuario);
                        break;
                    case 3:
                        MisAlquileres(usuario);
                        break;
                    case 4:
                        Console.WriteLine("Esperamos que vuelva pronto");
                        break;
                    default:
                        Console.WriteLine("Número incorrecto, introduzcalo otra vez");
                        MenuSecundario(usuario);
                        break;
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("\n Debe introducir un número del 1-4\n");
                MenuSecundario(usuario);
            }
        }
        public static void RegistrarUsuario()
        {
            //Le pido al usuario el email y la contraseña
            Console.WriteLine("Email");
            string email = Console.ReadLine();
            Console.WriteLine("Contraseña");
            string contrasena = Console.ReadLine();

            //Iterar la clase usuario para llamar al método de comprobar para saber si el ususario está registrado
            Usuario usuario = new Usuario();

            if (!usuario.ComprobarUsuario(email, contrasena))//Comprueba que el usuario no esté registrado
            {
                Console.WriteLine("Introduzca su nombre");
                string nombre = Console.ReadLine().ToUpper();
                Console.WriteLine("Introduzca su apellido");
                string apellido = Console.ReadLine().ToUpper();
                Console.WriteLine("Introduzca su fecha de nacimiento en el formato dd/mm/aaaa");
                DateTime fechaNacimiento = Convert.ToDateTime(Console.ReadLine());

                //Insertar los datos del usuario en Sql (nuevo registro, nueva fila)
                string query = $"Insert into Usuario (Nombre,Apellido,FechaNacimiento,Email,Contrasena) Values('{nombre}','{apellido}','{fechaNacimiento}','{email}','{contrasena}')"; //Revisar que los nombres de las columnas estén escritas igual que en SQL
                ModificarBase(query);
                Console.WriteLine("\nUsuario registrado");

                query = $"Select * from Usuario where Email LIKE '{email}' ";
                connection.Open(); //Abrir la conexión
                SqlCommand command = new SqlCommand(query, connection); //Conectar el comando con sql, SIEMPRE TIENE QUE ESTAR
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    usuario = new Usuario(Convert.ToInt32(reader[0].ToString()), reader[1].ToString(), reader[2].ToString(), Convert.ToDateTime(reader[3].ToString()), reader[4].ToString(), reader[5].ToString(), usuario.CalcularListaPeliculasU());
                }
                connection.Close();

                Console.WriteLine("\nSi desea iniciar sesión pulse 1, si desea salir pulse cualquier tecla");
                string respuesta = Console.ReadLine();
                if (Convert.ToInt32(respuesta) == 1)
                {
                    MenuSecundario(usuario);
                }

            }
            else
            {
                Console.WriteLine("El usuario ya está registrado, si desea iniciar sesión pulse 1, si desea salir pulse cualquier tecla");
                string respuesta = Console.ReadLine();
                if (Convert.ToInt32(respuesta) == 1)
                {
                    MenuSecundario(usuario);
                }
            }


        }
        public static void MostrarPeliculas(Usuario usuario)
        {
            int i = 1;
            foreach (Peliculas pelicula in usuario.ListaPeliculasUsuario)
            {
                Console.WriteLine($"{i}. {pelicula.Titulo}");
                i++;
            }

            //Darle la opcion de ver los datos de una película
            int opcion;
            do
            {
                Console.WriteLine("\n\n¿Qué película desea seleccionar?\nSi desea volver al menú principal pulse 0\n");
                string pelicula = Console.ReadLine();
                if (Int32.TryParse(pelicula, out opcion))
                {
                    if (opcion < usuario.ListaPeliculasUsuario.Count && opcion > 0)
                    {
                        usuario.ListaPeliculasUsuario[(opcion - 1)].MostrarDatos();
                    }
                    else if (opcion != 0)
                    {
                        Console.WriteLine("\n***El número introducido no representa ninguna opción, por favor introduzcalo otra vez***\n");
                    }
                }
                else
                {
                    Console.WriteLine("\n***La opción seleccionada no es correcta, debe seleccionar un dígito***\n");
                }

            } while (opcion != 0);
            MenuSecundario(usuario);
        }
        public static void AlquilarPelicula(Usuario usuario)
        {
                int opcion;
            do
            {
                Console.WriteLine("LISTADO DE PELÍCULAS DISPONIBLES");
                int i = 1;
                foreach (Peliculas pelicula in usuario.ListaPeliculasUsuario)
                {
                    if (pelicula.Estado == "DISPONIBLE")
                    {
                        Console.WriteLine($"{i}. {pelicula.Titulo}");
                        i++;
                    }
                }

                Console.WriteLine("\n¿Qué película desea alquilar?\n Si desea volver al menú principal pulse 0");
                string select = Console.ReadLine();
                if (Int32.TryParse(select, out opcion))
                {
                    if (opcion < usuario.ListaPeliculasUsuario.Count && opcion > 0)
                    {
                        int idUsuario = usuario.ID;
                        int idPelicula = usuario.ListaPeliculasUsuario[(opcion-1)].ID;

                        string query = $"INSERT INTO Alquiler (IDUsuario,IDPelicula,FechaAlquiler) VALUES ({idUsuario},{idPelicula},GETDATE())"; //o ponerlo como GETDATE(), para que lo ejecute SQL
                        ModificarBase(query);

                        query = $"Update Peliculas SET ESTADO ='OCUPADA' Where ID='{idPelicula}'";
                        ModificarBase(query);
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine($"{usuario.Nombre} ha alquilado la película {usuario.ListaPeliculasUsuario[(opcion - 1)].Titulo}");
                        
                    }
                    else if (opcion==0)
                    {
                        MenuSecundario(usuario);
                    }
                }
                else 
                {
                    Console.WriteLine("Debe introducir el dígito de una de las opciones");
                }

            } while (opcion!=0);


        }
        public static void MisAlquileres(Usuario usuario)
        {
            Console.WriteLine("\nEsta es la lista de películas que tiene alquidadas:\n");
            //Lista de peliculas alquiladas del usuario
            List<Alquiler> alquilerUsuario = new List<Alquiler>();
            string query = $"SELECT * FROM Alquiler WHERE IDUsuario={usuario.ID} AND FechaDevolucion IS NULL";
            SqlDataReader reader= ConsultarBase(query);
            while (reader.Read())
            {
                Alquiler alquiler = new Alquiler(Convert.ToInt32(reader[0].ToString()),Convert.ToInt32(reader[1].ToString()), Convert.ToInt32(reader[2].ToString()),Convert.ToDateTime(reader[3].ToString())); //Para crear un objeto
                alquilerUsuario.Add(alquiler);
            }

            //Le muestro la lista de peliculas que tiene alquiladas y que todavia no ha devuelto
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\tNOMBRE ||\tTÍTULO PELÍCULA \t||\tFECHA DE ALQUILER ||\t");
            Console.WriteLine("\t----------------------------------------------------------------");
            foreach (Alquiler item in alquilerUsuario)
            {
                for (int i = 0; i < usuario.ListaPeliculasUsuario.Count; i++)
                {
                    if (usuario.ListaPeliculasUsuario[i].ID==item.IDPelicula)
                    {
                        if (DateTime.Today>=item.FechaAlquiler.AddDays(2))
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"\t{usuario.Nombre} ||\t{usuario.ListaPeliculasUsuario[i].Titulo} \t||\t{item.FechaAlquiler} ||");
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.WriteLine($"\t{usuario.Nombre} ||\t{usuario.ListaPeliculasUsuario[i].Titulo} \t||\t{item.FechaAlquiler} ||");
                        }
                    }
                }

            }

            do
            {
            Console.WriteLine("Si desea devolver alguna de las películas pulse 1\n Si desea salir pulse 0");

            } while (true);

            //Se le da la opcion al usuario de devolver la película 

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
        public static SqlDataReader ConsultarBase(string query)
        {
            connection.Open(); //Abrir la conexión
            SqlCommand command = new SqlCommand(query, connection); //Conectar el comando con sql, SIEMPRE TIENE QUE ESTAR
            SqlDataReader reader = command.ExecuteReader();
            return reader;

        }

    }
}

