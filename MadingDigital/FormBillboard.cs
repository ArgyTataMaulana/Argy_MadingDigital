using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace MadingDigital
{
    public partial class FormBillboard : Form
    {
        // List untuk menampung data dari database
        List<string> listGambar = new List<string>();
        List<string> listJudul = new List<string>();
        List<string> listIsi = new List<string>();
        int indexSekarang = 0;

        public FormBillboard()
        {
            InitializeComponent();
        }

        private void FormBillboard_Load(object sender, EventArgs e)
        {
            // Menghilangkan border dan membuat layar penuh
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;

            AmbilDataDariDatabase();

            // Jalankan slide pertama kali jika ada data
            if (listGambar.Count > 0 || listJudul.Count > 0)
            {
                UpdateTampilan();
            }
            else
            {
                lblJudul.Text = "Selamat Datang";
                lblIsi.Text = "Belum ada pengumuman aktif saat ini.";
            }
        }

        private void AmbilDataDariDatabase()
        {
            Koneksi kon = new Koneksi();
            MySqlConnection conn = kon.GetConn();
            try
            {
                conn.Open();

                // 1. Ambil Data Gambar (dari tabel gambar_mading)
                MySqlCommand cmdG = new MySqlCommand("SELECT path_file FROM gambar_mading", conn);
                MySqlDataReader drG = cmdG.ExecuteReader();
                while (drG.Read())
                {
                    listGambar.Add(drG["path_file"].ToString());
                }
                drG.Close();

                // 2. Ambil Data Pengumuman (dari tabel pengumuman yang statusnya aktif)
                MySqlCommand cmdT = new MySqlCommand("SELECT judul, isi_pengumuman FROM pengumuman WHERE status='aktif'", conn);
                MySqlDataReader drT = cmdT.ExecuteReader();
                while (drT.Read())
                {
                    listJudul.Add(drT["judul"].ToString());
                    listIsi.Add(drT["isi_pengumuman"].ToString());
                }
                drT.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat konten Billboard: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void timerSlide_Tick(object sender, EventArgs e)
        {
            UpdateTampilan();
        }

        private void UpdateTampilan()
        {
            // Update Gambar (Jika ada gambar di list)
            if (listGambar.Count > 0)
            {
                int idxG = indexSekarang % listGambar.Count;
                // Menggunakan ImageLocation agar tidak berat saat loading file
                pictureBox1.ImageLocation = listGambar[idxG];
            }

            // Update Teks Pengumuman (Jika ada teks di list)
            if (listJudul.Count > 0)
            {
                int idxT = indexSekarang % listJudul.Count;
                lblJudul.Text = listJudul[idxT];
                lblIsi.Text = listIsi[idxT];
            }

            // Naikkan index untuk giliran selanjutnya
            indexSekarang++;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}