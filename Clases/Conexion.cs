using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace RitramaAPP.Clases
{
    public class Conexion
    {
        public string GodiServer = @"DESKTOP-M2F5UC0\SQLEXPRESS";
        public string Laptop = @"WEBSERVER\SQLEXPRESS";
        //string bd = "RITRAMA";
        public SqlConnection cnn;
        public Boolean Conectar(string bd)
        {
            try
            {
                cnn = new SqlConnection(@"Server=RITRAMASRV01;" +
                "Database=" + bd + ";User Id=Npino;Password=Jossycar5%;");
                cnn.Open();
                return true;
            }
            catch (SqlException ex)
            {
                MessageBox.Show("error al conectarse a la base de datos. error code:" + ex);
                return false;
            }
        }

        public Boolean Desconectar()
        {
            try
            {
                if (cnn.State == System.Data.ConnectionState.Open)
                {
                    cnn.Close();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error al desconectarse de la base de datos. error: " + ex);
                return false;
            }
        }
    }
}
