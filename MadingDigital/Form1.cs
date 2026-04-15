using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;


namespace MadingDigital
{
    class Koneksi
    {
        public MySqlConnection GetConn()
        {
            // menghubungkan ke database MySql
            string connString = "server=localhost;database=madingDigital_DB;uid=root;pwd=Satoru12345;";
            MySqlConnection conn = new MySqlConnection(connString);
            return conn;
        }
    }

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
