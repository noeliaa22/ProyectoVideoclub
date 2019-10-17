using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

namespace Videoclub
{
    class Alquiler
    {
        static SqlConnection connection = new SqlConnection("Data Source=DESKTOP-F3J0NS5\\SQLEXPRESS;Initial Catalog=Northwind;Integrated Security=True");
        public int ID { get; set; }
        public int IDUsuario { get; set; }
        public int IDPelicula { get; set; }
        public DateTime FechaAlquiler { get; set; }
        public DateTime FechaDevolucion { get; set; }

        public Alquiler(int iD,int iDUsuario, int iDPelicula, DateTime fechaAlquiler, DateTime fechaDevolucion)
        {
            ID = iD;
            IDUsuario = iDUsuario;
            IDPelicula = iDPelicula;
            FechaAlquiler = fechaAlquiler;
            FechaDevolucion = fechaDevolucion;
        }

        public Alquiler(int iD,int iDUsuario, int iDPelicula, DateTime fechaAlquiler)
        {
            ID = iD;
            IDUsuario = iDUsuario;
            IDPelicula = iDPelicula;
            FechaAlquiler = fechaAlquiler;
        }

        public void Insertar()
        {

        }
    }
}
