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
using MadingDigital.BLL;

namespace MadingDigital
{
    public partial class FormBillboard : Form
    {
        private readonly PengumumanBLL pengumumanBLL = new PengumumanBLL();
        private readonly GambarMadingBLL gambarBLL = new GambarMadingBLL();

        private DataTable dtPengumuman;
        private DataTable dtGambar;
        private int indexSekarang = 0;
        private Timer timerSlide;

        public FormBillboard()
        {
            InitializeComponent();

           


            // Setup Timer
            timerSlide = new Timer();
            timerSlide.Interval = 5000; // 5 detik
            timerSlide.Tick += timerSlide_Tick;
        }

        private void FormBillboard_Load(object sender, EventArgs e)
        {
                    this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;

            LoadDataForBillboard();

            if (dtPengumuman.Rows.Count > 0 || dtGambar.Rows.Count > 0)
            {
                timerSlide.Start();
                UpdateTampilan();
            }
            else
            {
                lblJudul.Text = "Tidak Ada Pengumuman";
                lblIsi.Text = "Silakan tambahkan pengumuman melalui admin panel.";
            }
        }

        private void LoadDataForBillboard()
        {
            try
            {
                dtPengumuman = pengumumanBLL.GetPengumumanAktifSekarang();
                dtGambar = gambarBLL.GetAllGambarAktif();

            
                MessageBox.Show($"Pengumuman: {dtPengumuman.Rows.Count} baris\nGambar: {dtGambar.Rows.Count} baris");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat data billboard: " + ex.Message);
            }
        }

        private void timerSlide_Tick(object sender, EventArgs e)
        {
            UpdateTampilan();
        }

        private void UpdateTampilan()
        {
            if (dtPengumuman.Rows.Count > 0)
            {
                int idx = indexSekarang % dtPengumuman.Rows.Count;
                DataRow row = dtPengumuman.Rows[idx];

                lblJudul.Text = row["judul"].ToString();
                lblIsi.Text = row["isi_pengumuman"].ToString();
            }

            if (dtGambar.Rows.Count > 0)
            {
                int idxG = indexSekarang % dtGambar.Rows.Count;
                string path = dtGambar.Rows[idxG]["path_file"].ToString();
                
                try
                {
                    pictureBox1.ImageLocation = path;
                }
                catch
                {
                    pictureBox1.Image = null;
                }
            }

            indexSekarang++;
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (ModifierKeys == Keys.Control) // Tekan Ctrl + Klik untuk close
            {
                this.Close();
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            timerSlide.Stop();
            base.OnFormClosing(e);
        }
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Close();
            base.OnKeyDown(e);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }


}