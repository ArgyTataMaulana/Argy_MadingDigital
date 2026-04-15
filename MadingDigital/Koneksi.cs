using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;


namespace MadingDigital
{
    class Koneksi
    {
        public MySqlConnection GetConn()
        {
            // untuk menghubungkan ke database
            string connString = "server=localhost;database=madingDigital;uid=root;pwd=Satoru12345;";
            MySqlConnection conn = new MySqlConnection(connString);
            return conn;
        }
    }
}