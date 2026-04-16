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
    public partial class FormKoneksi : Form
    {
        public FormKoneksi()
        {
            InitializeComponent();
        }

        private void FormKoneksi_Load(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            Koneksi kon = new Koneksi();
            MySqlConnection conn = kon.GetConn();

            try
            {
                lblStatus.Text = "Menghubungkan...";
                conn.Open();

                if (conn.State == ConnectionState.Open)
                {
                    lblStatus.Text = "Status: TERHUBUNG!";
                    lblStatus.ForeColor = Color.Green;

                    // AKTIFKAN tombol masuk jika sukses
                    button1.Enabled = true;

                    MessageBox.Show("Koneksi ke Database MySQL Berhasil!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Status: GAGAL TERHUBUNG";
                lblStatus.ForeColor = Color.Red;
                MessageBox.Show("Koneksi Gagal! Pastikan XAMPP (MySQL) sudah aktif.\nError: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        
    }
}
