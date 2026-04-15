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
    public partial class Form1 : Form
    {
        public void TampilkanData()
        {
            Koneksi kon = new Koneksi();
            MySqlConnection conn = kon.GetConn();

            try
            {
                conn.Open();
                string query = "SELECT id_pengumuman, judul, isi_pengumuman, status, tanggal_upload FROM pengumuman";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataReader dr = cmd.ExecuteReader();

                DataTable dt = new DataTable();
                dt.Load(dr);

                
                dataGridView1.DataSource = dt;

                
                dataGridView1.Columns[0].HeaderText = "ID";
                dataGridView1.Columns[1].HeaderText = "Judul";
                dataGridView1.Columns[2].HeaderText = "Isi Pengumuman";
                dataGridView1.Columns[3].HeaderText = "Status";
                dataGridView1.Columns[4].HeaderText = "Tanggal";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal menampilkan data: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        public void HitungTotal()
        {
            Koneksi kon = new Koneksi();
            MySqlConnection conn = kon.GetConn();

            try
            {
                conn.Open();
                // Query Count
                string query = "SELECT COUNT(*) FROM pengumuman";
                MySqlCommand cmd = new MySqlCommand(query, conn);

                int total = Convert.ToInt32(cmd.ExecuteScalar());

                lblTotal.Text = "Total Pengumuman: " + total.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal menghitung data: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void labelIsi_Click(object sender, EventArgs e)
        {

        }

        private void maskedTextBox1_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }
    }
}
