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
            //try
            //{
            //}
            //catch (Exception)
            //{
            //    Console.WriteLine("ERROR");
            //}

        }

        public static void MenuInicio()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\nBIENVENIDO AL VIDEOCLUB EXPIRIENCE:\n¿Quieres spoilear a tus amigos?\nAhora puedes tener los mejores estrenos antes de su lanzamiento\n");
            Console.WriteLine("¿Qué acción desea realizar?");
            Console.WriteLine("*********************************");
            Console.WriteLine("\n1.Iniciar Sesión");
            Console.WriteLine("\n2.Registrarse");
            Console.WriteLine("\n3.Salir");
            Console.ResetColor();
            int opcion;

            if (Int32.TryParse(Console.ReadLine(), out opcion))
            {
                switch (opcion)
                {
                    case 1:
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("Email: ");
                        string email = Console.ReadLine().ToLower();
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

                            Console.Clear();
                            MenuSecundario(usuario);
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine($"***El email {email} no está registrado***\n - Debe registrarse primero, si desea realizar esta acción pulse 1");
                            Console.WriteLine($" - Si desea volver al menu inicual pulse cualquier tecla");
                            Console.ResetColor();
                            string respuesta = Console.ReadLine();
                            if (Int32.TryParse(respuesta, out opcion))
                            {
                                if (opcion == 1)
                                {
                                    Console.Clear();
                                    RegistrarUsuario();
                                }
                                else
                                {
                                    Console.Clear();
                                    MenuInicio();
                                }

                            }
                            else
                            {
                                Console.Clear();
                                MenuInicio();
                            }
                        }
                        break;
                    case 2:
                        Console.Clear();
                        RegistrarUsuario();
                        break;
                    case 3:
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.WriteLine("Esperamos que vuelva pronto");
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("\n***El número introducido no representa ninguna opción, por favor introduzcalo otra vez***\n");
                        Console.ResetColor();
                        Console.ReadKey();
                        Console.Clear();
                        MenuInicio();
                        break;
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("***Debe introducir un dígito***");
                Console.ResetColor();
                Console.ReadKey();
                Console.Clear();
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
            Console.ResetColor();
            int opcion = 0;
            if (int.TryParse(Console.ReadLine(), out opcion))
            {
                switch (opcion)
                {
                    case 1:
                        Console.Clear();
                        MostrarPeliculas(usuario);
                        break;
                    case 2:
                        Console.Clear();
                        AlquilarPelicula(usuario);
                        break;
                    case 3:
                        Console.Clear();
                        MisAlquileres(usuario);
                        break;
                    case 4:
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.WriteLine("Esperamos que vuelva pronto");
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("***Número incorrecto, introduzcalo otra vez***");
                        Console.ResetColor();
                        Console.ReadKey();
                        Console.Clear();
                        MenuSecundario(usuario);
                        break;
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n***Debe introducir un dígito del 1-4***\n");
                Console.ResetColor();
                Console.ReadKey();
                Console.Clear();
                MenuSecundario(usuario);
            }
        }
        public static void RegistrarUsuario()
        {
            //Creo un nuevo objeto
            Usuario usuario = new Usuario();
            //Le pido al usuario el email y la contraseña
            string email;
            string contrasena;
            bool emailCorrecto;
            bool contrasenaCorrecta;
            do
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write("Email: ");
                email = Console.ReadLine().ToLower();
                emailCorrecto = usuario.EmailCorrecto(email);
                Console.Write("Contraseña: ");
                contrasena = Console.ReadLine();
                contrasenaCorrecta = usuario.ContrasenaCorrecta(contrasena);
                Console.ResetColor();

            } while (!emailCorrecto || !contrasenaCorrecta);

            //Iterar la clase usuario para llamar al método de comprobar para saber si el ususario está registrado


            if (!usuario.ComprobarUsuario(email, contrasena))//Comprueba que el usuario no esté registrado
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("Introduzca su nombre");
                string nombre = Console.ReadLine().ToUpper();
                Console.WriteLine("Introduzca su apellido");
                string apellido = Console.ReadLine().ToUpper();
                string fechaNacimiento;
                bool fechaCorrecta;
                do
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine("Introduzca su fecha de nacimiento en el formato dd/mm/aaaa");
                    fechaNacimiento = Console.ReadLine();
                    fechaCorrecta = usuario.EdadCorrecta(fechaNacimiento);
                    Console.ResetColor();

                } while (!fechaCorrecta);

                //Insertar los datos del usuario en Sql (nuevo registro, nueva fila)
                string query = $"Insert into Usuario (Nombre,Apellido,FechaNacimiento,Email,Contrasena) Values('{nombre}','{apellido}','{fechaNacimiento}','{email}','{contrasena}')"; //Revisar que los nombres de las columnas estén escritas igual que en SQL
                ModificarBase(query);
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\nUSUARIO REGISTRADO");
                Console.ResetColor();

                query = $"Select * from Usuario where Email LIKE '{email}' ";
                connection.Open(); //Abrir la conexión
                SqlCommand command = new SqlCommand(query, connection); //Conectar el comando con sql, SIEMPRE TIENE QUE ESTAR
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    usuario = new Usuario(Convert.ToInt32(reader[0].ToString()), reader[1].ToString(), reader[2].ToString(), Convert.ToDateTime(reader[3].ToString()), reader[4].ToString(), reader[5].ToString(), usuario.CalcularListaPeliculasU());
                }
                connection.Close();

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("\nSi desea iniciar sesión pulse 1, si desea salir pulse cualquier tecla");
                string respuesta = Console.ReadLine();
                if (Convert.ToInt32(respuesta) == 1)
                {
                    Console.Clear();
                    MenuSecundario(usuario);
                }

            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("***El usuario ya está registrado***");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Si desea iniciar sesión pulse 1, si desea salir pulse cualquier tecla");
                Console.ResetColor();
                string respuesta = Console.ReadLine();
                if (Convert.ToInt32(respuesta) == 1)
                {
                    Console.Clear();
                    MenuSecundario(usuario);
                }
            }


        }
        public static void MostrarPeliculas(Usuario usuario)
        {
            int i = 1;
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("\nLISTADO DE PELÍCULAS RECOMENDADAS: \n");
            foreach (Peliculas pelicula in usuario.ListaPeliculasUsuario)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"{i}. {pelicula.Titulo}");
                i++;
            }
            Console.ResetColor();

            //Darle la opcion de ver los datos de una película
            int opcion;
            do
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("\n\nSi desea ver los detalles de alguna película pulse el número correspondiente a la misma\nSi desea volver al menú principal pulse '0'\n");
                Console.ResetColor();
                string pelicula = Console.ReadLine();
                if (Int32.TryParse(pelicula, out opcion))
                {
                    if (opcion < usuario.ListaPeliculasUsuario.Count && opcion > 0)
                    {
                        Console.Clear();
                        usuario.ListaPeliculasUsuario[(opcion - 1)].MostrarDatos();
                    }
                    else if (opcion != 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("\n***El número introducido no representa ninguna opción, por favor introduzcalo otra vez***\n");
                        Console.ResetColor();
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\n***La opción seleccionada no es correcta, debe seleccionar un dígito***\n");
                    Console.ResetColor();
                }

            } while (opcion != 0);
            Console.Clear();
            MenuSecundario(usuario);
        }
        public static void AlquilarPelicula(Usuario usuario)
        {
            int opcion;
            do
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("LISTADO DE PELÍCULAS DISPONIBLES");
                int i = 1;
                Console.ResetColor();
                foreach (Peliculas pelicula in usuario.ListaPeliculasUsuario)
                {
                    if (pelicula.Estado == "DISPONIBLE")
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine($"{pelicula.ID}. {pelicula.Titulo}");
                        i++;
                    }
                }
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("\n¿Qué película desea alquilar?\n Si desea volver al menú principal pulse 0");

                string select = Console.ReadLine();
                if (Int32.TryParse(select, out opcion))
                {
                    if (opcion < usuario.ListaPeliculasUsuario.Count && opcion > 0)
                    {
                        int idUsuario = usuario.ID;
                        int idPelicula = usuario.ListaPeliculasUsuario[(opcion - 1)].ID;

                        string query = $"INSERT INTO Alquiler (IDUsuario,IDPelicula,FechaAlquiler) VALUES ({idUsuario},{idPelicula},GETDATE())"; //o ponerlo como GETDATE(), para que lo ejecute SQL
                        ModificarBase(query);

                        query = $"Update Peliculas SET ESTADO ='OCUPADA' Where ID='{idPelicula}'";
                        ModificarBase(query);
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine($"{usuario.Nombre} ha alquilado la película {usuario.ListaPeliculasUsuario[(opcion - 1)].Titulo}");

                    }
                    else if (opcion == 0)
                    {
                        Console.Clear();
                        MenuSecundario(usuario);
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("\n***Debe introducir el dígito de una de las opciones***\n");
                    Console.ResetColor();
                    Console.ReadKey();
                    Console.Clear();
                }

            } while (opcion != 0);


        }
        public static void MisAlquileres(Usuario usuario)
        {
            int opcion;
            do
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("\nEsta es la lista de películas que tiene alquidadas:\n");
                //Lista de peliculas alquiladas del usuario
                List<Alquiler> alquilerUsuario = new List<Alquiler>();

                string query = $"SELECT * FROM Alquiler WHERE IDUsuario={usuario.ID} AND FechaDevolucion IS NULL";
                SqlDataReader reader = ConsultarBase(query);
                while (reader.Read())
                {
                    Alquiler alquiler = new Alquiler(Convert.ToInt32(reader[0].ToString()), Convert.ToInt32(reader[1].ToString()), Convert.ToInt32(reader[2].ToString()), Convert.ToDateTime(reader[3].ToString())); //Para crear un objeto
                    alquilerUsuario.Add(alquiler);
                }
                connection.Close();

                //Le muestro la lista de peliculas que tiene alquiladas y que todavia no ha devuelto
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("\tNOMBRE || \tNÚMERO || \tTÍTULO PELÍCULA \t || \tFECHA DE ALQUILER || \t");
                Console.WriteLine("\t----------------------------------------------------------------");
                foreach (Alquiler item in alquilerUsuario)
                {
                    for (int i = 0; i < usuario.ListaPeliculasUsuario.Count; i++)
                    {
                        if (usuario.ListaPeliculasUsuario[i].ID == item.IDPelicula)
                        {
                            if (DateTime.Today >= item.FechaAlquiler.AddDays(2))
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine($"\t{usuario.Nombre} || \t{item.IDPelicula} ||\t{usuario.ListaPeliculasUsuario[i].Titulo} \t||\t{item.FechaAlquiler} ||");
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Cyan;
                                Console.WriteLine($"\t{usuario.Nombre} || \t{item.IDPelicula} ||\t{usuario.ListaPeliculasUsuario[i].Titulo} \t||\t{item.FechaAlquiler} ||");
                            }
                        }
                    }

                }

                Console.WriteLine("\nSi desea devolver alguna de las películas, introduzca el número correspondiente\n Si desea volver al menú principal pulse 0\n");
                string respuesta = Console.ReadLine();
                if (Int32.TryParse(respuesta, out opcion))
                {
                    //HACER UN BOOLEANO!!!!!!!!
                    //Recorrer la lista de peliculas alquiladas y cuando identifica la seleccionada modifica la BBDD
                    bool peliculaDevuelta = false;
                    foreach (Alquiler item in alquilerUsuario)
                    {
                        if (opcion == item.IDPelicula)
                        {
                            int idUsuario = usuario.ID;
                            int idPelicula = item.IDPelicula;

                            query = $"Update Alquiler SET FechaDevolucion=GETDATE() WHERE IDUsuario={idUsuario} AND IDPelicula={idPelicula}"; //o ponerlo como GETDATE(), para que lo ejecute SQL
                            ModificarBase(query);

                            query = $"Update Peliculas SET ESTADO='DISPONIBLE' Where ID='{idPelicula}'";
                            ModificarBase(query);
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            peliculaDevuelta = true;
                        }
                    }
                    if (peliculaDevuelta == true)
                    {
                        Console.WriteLine($"{usuario.Nombre}, la devolución se ha realizado con éxito");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("\n***La opción seleccionada no representa ninguna película***\n");
                        Console.WriteLine("\n***Opción no válida, debe introducir el dígito de una de las opciones***\n");
                        Console.ResetColor();
                        Console.ReadLine();
                        Console.Clear();
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\n***Opción no válida, debe introducir el dígito de una de las opciones***\n");
                    Console.ResetColor();
                    Console.ReadLine();
                    Console.Clear();
                }

            } while (opcion != 2);
            Console.Clear();
            MenuSecundario(usuario);

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

