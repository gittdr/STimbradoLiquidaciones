using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CARGAR_EXCEL.Models
{
    public static class Conexion
    {
        public static SqlConnection Open()
        {
            SqlConnection Conn = new SqlConnection("Data Source=172.24.16.112;Initial Catalog=TMWSuite;Persist Security Info=True;User ID=sa;Password=tdr9312");
            Conn.Open();
            return Conn;
        }

        public static SqlConnection Close()
        {
            SqlConnection Conn = new SqlConnection("Data Source=172.24.16.112;Initial Catalog=TMWSuite;Persist Security Info=True;User ID=sa;Password=tdr9312");
            Conn.Close();
            return Conn;
        }
    }
}